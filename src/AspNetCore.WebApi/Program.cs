using AspNetCore.WebApi.Services;

namespace AspNetCore.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Configuration.AddJsonFile("appsettings.Development.json");

            builder.Services.Configure<DadataServiceOptions>(builder.Configuration.GetSection(DadataServiceOptions.SectionName));

            builder.Services.AddHttpClient("DadataClient", client =>
            {
                var options = builder.Configuration.GetSection(DadataServiceOptions.SectionName).Get<DadataServiceOptions>();
                client.BaseAddress = new Uri(options.DaDataApiBaseUrl);
                client.DefaultRequestHeaders.Add("Authorization", $"Token {options.DaDataApiToken}");
            });

            builder.Services.AddScoped<IDadataService, DadataService>();

            // Add services to the container.
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
