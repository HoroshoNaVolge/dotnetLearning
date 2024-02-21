using AspNetCore.WebApi.Services;

namespace AspNetCore.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<DadataServiceOptions>(builder.Configuration.GetSection(DadataServiceOptions.SectionName));

            // Не совсем уверен, что правильно передавать options потом в ConfigureHttpClient из этого контекста.
            var options = builder.Configuration.GetSection(DadataServiceOptions.SectionName).Get<DadataServiceOptions>();
            if (string.IsNullOrEmpty(options?.DaDataApiBaseUrl))
                throw new ArgumentNullException("Нет DaDataApiBaseUrl в файле конфигурации");
            if (string.IsNullOrEmpty(options.DaDataApiToken))
                throw new ArgumentNullException("Нет DaDataAPIToken в файле конфигурации");

            // Через новый сервис провайдер, но VS ругается, что надо делать только одним провайдером
            //var serviceProvider = new ServiceCollection()
            //    .Configure<DadataServiceOptions>(builder.Configuration.GetSection(DadataServiceOptions.SectionName))
            //    .BuildServiceProvider();

            //var options = serviceProvider.GetRequiredService<IOptions<DadataServiceOptions>>().Value;
            //{
            //    if (string.IsNullOrEmpty(options.DaDataApiBaseUrl))
            //        throw new ArgumentNullException("Нет DaDataApiBaseUrl в файле конфигурации");
            //}

            builder.Services.AddHttpClient("DadataClient")
           .ConfigureHttpClient((client) =>
       {
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
