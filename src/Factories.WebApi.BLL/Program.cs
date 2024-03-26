using Microsoft.EntityFrameworkCore;
using Factories.WebApi.DAL.EF;
using Factories.WebApi.DAL.Interfaces;
using Factories.WebApi.DAL.Repositories;
using Serilog;
using Factories.WebApi.BLL.Services;
using Factories.WebApi.BLL.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Factories.WebApi.DAL.Entities;
using Factories.WebApi.BLL.Authentication;

namespace Factories.WebApi.BLL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<UserService>();

            builder.Services.AddScoped(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];
                var key = configuration["Jwt:SecretKey"];
                return new JwtService(issuer, audience, key);
            });
           
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };
            });
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<FacilitiesDbContext>(options =>
                          options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<UsersDbContext>(options =>
                                      options.UseNpgsql(builder.Configuration.GetConnectionString("UsersConnection")));

            builder.Services.AddScoped<IRepository<Tank>, TankRepository>()
                             .AddScoped<IRepository<Unit>, UnitRepository>()
                             .AddScoped<IRepository<Factory>, FactoryRepository>();


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddHostedService<WorkerService>();



            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
             .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning).WriteTo.Console()
             .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
             .CreateLogger();

            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
