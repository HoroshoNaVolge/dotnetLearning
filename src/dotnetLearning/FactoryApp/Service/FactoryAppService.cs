using Microsoft.Extensions.Options;
using dotnetLearning.FactoryApp.Service.FacilityService;
using dotnetLearning.FactoryApp.View;

namespace dotnetLearning.FactoryApp.Service
{
    public class FactoryAppService(IFacilityService facilityService, IView view, IOptions<FacilityServiceOptions> options)
    {
        internal async Task RunAsync()
        {
            if (string.IsNullOrEmpty(options.Value.FacilitiesJsonFilePath))
                throw new ArgumentNullException("Ошибка в файле конфигурации appsettings.json");

            CancellationTokenSource cancellationTokenSource = new();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // по умолчанию нужны коллекции готовых объектов
            await facilityService.DeserializeDataJson(options.Value.FacilitiesJsonFilePath);

            var userInteractionFinished = false;
#nullable disable
            view.InputReceived += HandleUserInput;
#nullable restore
            while (!userInteractionFinished)
            {
                var userInput = view.GetUserInput(
                    "get conf - показать текущую конфигурацию системы\n" +
                    "get total - показать полную сводку\n" +
                    "get tanksVolumeTotal - показать общую вместимость резервуаров\n" +
                    "get tanksSummary - показать сводку по резервуарам\n" +
                    "get factoriesSummary - показать сводку по установкам\n" +
                    "get unitsSummary - показать сводку по заводам\n" +
                    "find unit - найти резервуар по названию\n" +
                    "find factory - найти установку по названию\n" +
                    "search - поиск по названию\n" +
                    "write json - сериализовать все объекты в json\n" +
                    "read json - десериализовать все объекты из json\n" +
                    "write excel - экспортировать все объекты в Excel\n" +
                    "read excel - импортировать все объекты из Excel\n" +
                    "exit - выход из программы\n" +
                    "Введите команду:");

                switch (userInput)
                {
                    case "get conf":
                        view.ShowMessage(facilityService.GetCurrentConfiguration());
                        break;

                    case "get total":
                        view.ShowMessage(facilityService.GetTotalSummary());
                        break;

                    case "get tanksVolumeTotal":
                        view.ShowMessage(facilityService.GetTotalVolumeTanks());
                        break;

                    case "get tanksSummary":
                        view.ShowMessage(facilityService.GetTanksSummary());
                        break;

                    case "get factoriesSummary":
                        view.ShowMessage(facilityService.GetFactoriesSummary());
                        break;

                    case "get unitsSummary":
                        view.ShowMessage(facilityService.GetUnitsSummary());
                        break;


                    case "find unit":
                        var tankName = view.GetUserInput("Введите название резервуара:");
                        if (string.IsNullOrEmpty(tankName))
                        {
                            view.ShowMessage($"Ошибка ввода");
                            return;
                        }

                        var foundUnit = FindUnit(tankName);
                        if (string.IsNullOrEmpty(foundUnit))
                            view.ShowMessage($"Резервуар {tankName} не найден");
                        else
                            view.ShowMessage($"Найдена установка: {foundUnit}");
                        break;

                    case "find factory":
                        var unitName = view.GetUserInput("Введите название установки:");

                        if (string.IsNullOrEmpty(unitName))
                        {
                            view.ShowMessage($"Ошибка ввода");
                            return;
                        }

                        var foundFactory = FindFactory(unitName);
                        if (string.IsNullOrEmpty(foundFactory))
                            view.ShowMessage($"Установка {unitName} не найдена");
                        else
                            view.ShowMessage($"Найден завод: {foundFactory}");

                        break;

                    case "search":
                        var searchName = view.GetUserInput("Введите название для поиска:");
                        if (string.IsNullOrEmpty(searchName))
                        {
                            view.ShowMessage($"Ошибка ввода");
                            return;
                        }

                        var result = facilityService.Search(searchName);
                        view.ShowMessage($"Результат поиска: {result.result} ({result.type.Name})");
                        break;

                    case "write json":
                        await facilityService.SerializeDataJsonAsync(cancellationToken);
                        break;

                    case "read json":
                        await facilityService.DeserializeDataJson(options.Value.FacilitiesJsonFilePath);
                        break;

                    case "write excel":
                        await facilityService.ExportDataToExcelAsync(cancellationToken);
                        break;

                    case "read excel":
                        await facilityService.ImportDataFromExcelAsync(cancellationToken);
                        break;

                    case "exit":
                        userInteractionFinished = true;
                        break;

                    default:
                        view.ShowMessage("Неизвестная команда");
                        break;
                }
            }

            //await facilityService.SerializeDataJsonAsync(new Tank() { Id = 6676, Description = "Ololoev", Name = "Onotole", MaxVolume = 100500, Volume = 0, UnitId = 2 }, cancellationToken);
        }
        private string? FindUnit(string tankName) => facilityService.FindUnit(tankName)?.ToString();
        private string? FindFactory(string unitName) => facilityService.FindFactory(unitName)?.ToString();

        private void HandleUserInput(object sender, UserInputEventArgs e) => view.ShowMessage($"Пользователь ввел: {e.UserInput} в {e.InputTime}");
    }
}