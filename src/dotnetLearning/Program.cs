using dotnetLearning.JsonParserApp;
using dotnetLearning.DadataAppConsole;
using dotnetLearning.Other;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using dotnetLearning.FactoryApp.Service;
using Microsoft.Extensions.DependencyInjection;
using dotnetLearning.FactoryApp.Service.FacilityService;
internal class Program
{
    private static void Main(string[] args)
    {
        using IHost host = Host.CreateApplicationBuilder(args).Build();

        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("C:\\Users\\user\\Desktop\\dotnetLearning\\src\\dotnetLearning\\appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.Configure<FacilityServiceOptions>(config.GetSection("FilePath"));

        var options = config.GetSection("FilePath").Get<FacilityServiceOptions>() ?? throw new ArgumentNullException($"Ошибка конфигурации: не найдена секция FilePath или отсутствует файл Json");

        services.AddSingleton<FactoryAppService>();
        services.AddScoped<IFacilityService, FacilityService>();

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        var app = serviceProvider.GetService<FactoryAppService>() ?? throw new ArgumentNullException("Ошибка регистрации сервисов в контейнере зависимостей");
        app.Run();
    }
}

//StartFactoryApp.Run();
// Lesson2.Run();
// JsonLearn.Run();
// LinqLearn.Run();
// await LearnWebApi.Run();
// StartJsonParserApp.Run();
// await StartDadataAppConsole.RunAsyncUsingDadataPackage("7728437776");

//foreach (var i in new IEnumerableLearn()) { await Console.Out.WriteLineAsync(i.ToString()); }
//foreach (var i in new IEnumerableLearnWithOwnEnumerator()) { await Console.Out.WriteLineAsync(i.ToString()); }

//internal class Program
//{

//    static async Task Main(string[] args)
//    {
//        StartFactoryApp.Run();
//        // Lesson2.Run();
//        // JsonLearn.Run();
//        // LinqLearn.Run();
//        // await LearnWebApi.Run();
//        // StartJsonParserApp.Run();
//        // await StartDadataAppConsole.RunAsyncUsingDadataPackage("7728437776");

//        //foreach (var i in new IEnumerableLearn()) { await Console.Out.WriteLineAsync(i.ToString()); }
//        //foreach (var i in new IEnumerableLearnWithOwnEnumerator()) { await Console.Out.WriteLineAsync(i.ToString()); }
//    }
//}



