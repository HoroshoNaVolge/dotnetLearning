using Microsoft.EntityFrameworkCore;
using Factories.WebApi.DAL.EF;
using Factories.WebApi.DAL.Interfaces;
using Factories.WebApi.DAL.Repositories;
using Serilog;
using Factories.WebApi.BLL.Services;
using Factories.WebApi.BLL.Dto;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using Factories.WebApi.BLL.Authentification;
using Factories.WebApi.DAL.Entities;

namespace Factories.WebApi.BLL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<FacilitiesDbContext>(options =>
                           options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<UsersDbContext>(options =>
                                      options.UseNpgsql(builder.Configuration.GetConnectionString("UsersConnection")));

            builder.Services.AddScoped<IRepository<Tank>, TankRepository>();
            builder.Services.AddScoped<IRepository<Unit>, UnitRepository>();
            builder.Services.AddScoped<IRepository<Factory>, FactoryRepository>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Error)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning).WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            builder.Services.AddHostedService<WorkerService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
