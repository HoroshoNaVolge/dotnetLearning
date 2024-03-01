using Microsoft.Extensions.Options;
using dotnetLearning.FactoryApp.Service.FacilityService;
using dotnetLearning.FactoryApp.View;
using dotnetLearning.FactoryApp.Model;

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

            var userInteractionFinished = false;
#nullable disable
            view.InputReceived += HandleUserInput;
#nullable restore

            view.ShowMessage($"Система при инициализации: {facilityService.GetCurrentConfiguration()}");
            view.ShowMessage("Возможен CRUD JSON и Excel. Для проверки после выполнения команд используйте get total");

            while (!userInteractionFinished)
            {
                var userInput = view.GetUserInput(
                    "\nread json - десериализовать все объекты из json\n" +
                    "write json - сериализовать все объекты в json\n" +
                    "add json - добавить объект в json\n" +
                    "delete json - удалить объект из json\n\n" +
                    "read excel - импортировать все объекты из Excel\n" +
                    "write excel - экспортировать все объекты в Excel\n" +
                    "add excel - добавить объект в Excel\n" +
                    "delete excel - удалить объект из Excel\n\n" +
                    "get conf - показать текущую конфигурацию системы\n" +
                    "get total - показать полную сводку\n" +
                    "get tanksVolumeTotal - показать общую вместимость резервуаров\n" +
                    "get tanksSummary - показать сводку по резервуарам\n" +
                    "get factoriesSummary - показать сводку по установкам\n" +
                    "get unitsSummary - показать сводку по заводам\n" +
                    "find unit - найти резервуар по названию\n" +
                    "find factory - найти установку по названию\n" +
                    "search - поиск по названию\n" +
                    "write db - записать все объекты в БД\n" +
                    "exit - выход из программы\n" +
                    "Введите команду:");

                switch (userInput)
                {
                    case "get conf":
                        view.ShowMessage(facilityService.GetCurrentConfiguration());
                        break;

                    case "get total":
                        view.ShowMessage(facilityService.GetTotalSummary() ?? "Nothing");
                        break;

                    case "get tanksVolumeTotal":
                        view.ShowMessage(facilityService.GetTotalVolumeTanks() ?? "Nothing");
                        break;

                    case "get tanksSummary":
                        view.ShowMessage(facilityService.GetTanksSummary() ?? "Nothing");
                        break;

                    case "get factoriesSummary":
                        view.ShowMessage(facilityService.GetFactoriesSummary() ?? "Nothing");
                        break;

                    case "get unitsSummary":
                        view.ShowMessage(facilityService.GetUnitsSummary() ?? "Nothing");
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
                        if (result is null)
                            view.ShowMessage("Объект не найден");
                        else
                            view.ShowMessage($"Результат поиска: {result}");
                        break;

                    case "write json":
                        await facilityService.CreateOrUpdateJsonDataAsync(cancellationToken);
                        break;

                    case "add json":
                        var facilityTypeString = "dotnetLearning.FactoryApp.Model." + view.GetUserInput("Введите название типа объекта");
                        var facilityType = Type.GetType(facilityTypeString);

                        if (facilityType is null)
                        {
                            view.ShowMessage("Неизвестный тип объекта");
                            break;
                        }

                        switch (facilityType.Name)
                        {
                            case "Factory":
                                var factory = CreateFactoryByUserInput(view);
                                if (factory is null)
                                {
                                    view.ShowMessage("Ошибка ввода");
                                    break;
                                }
                                await facilityService.AddDataToJsonAsync(factory, cancellationToken);
                                break;
                            case "Unit":
                                var unit = CreateUnitByUserInput(view);
                                if (unit is null)
                                {
                                    view.ShowMessage("Ошибка ввода");
                                    break;
                                }
                                await facilityService.AddDataToJsonAsync(unit, cancellationToken);
                                break;
                            case "Tank":
                                var tank = CreateTankByUserInput(view);
                                if (tank is null)
                                {
                                    view.ShowMessage("Ошибка ввода");
                                    break;
                                }
                                await facilityService.AddDataToJsonAsync(tank, cancellationToken);
                                break;
                            default:
                                view.ShowMessage("Неизвестный тип объекта");
                                break;
                        }
                        break;

                    case "delete json":
                        var inputString = view.GetUserInput("Введите название объекта для удаления");
                        if (string.IsNullOrEmpty(inputString))
                        {
                            view.ShowMessage("Пустой ввод");
                            break;
                        }

                        var fac = facilityService.Search(inputString);
                        if (fac is null)
                        {
                            view.ShowMessage("Объект не найден");
                            break;
                        }

                        await facilityService.DeleteDataFromJsonAsync(fac, cancellationToken);
                        break;

                    case "read json":
                        await facilityService.GetAllJsonDataAsync(options.Value.FacilitiesJsonFilePath);
                        break;

                    case "write excel":
                        await facilityService.CreateOrUpdateDataExcelAsync(cancellationToken);
                        break;

                    case "read excel":
                        await facilityService.GetDataFromExcelAsync(cancellationToken);
                        break;

                    case "add excel":
                        var typeFacExcelStr = "dotnetLearning.FactoryApp.Model." + view.GetUserInput("Введите название типа объекта");
                        var excelFacType = Type.GetType(typeFacExcelStr);

                        if (excelFacType is null)
                        {
                            view.ShowMessage("Неизвестный тип объекта");
                            break;
                        }

                        switch (excelFacType.Name)
                        {
                            case "Factory":
                                var factory = CreateFactoryByUserInput(view);
                                if (factory is null)
                                {
                                    view.ShowMessage("Ошибка ввода");
                                    break;
                                }
                                await facilityService.AddDataToExcelAsync(factory, cancellationToken);
                                break;
                            case "Unit":
                                var unit = CreateUnitByUserInput(view);
                                if (unit is null)
                                {
                                    view.ShowMessage("Ошибка ввода");
                                    break;
                                }
                                await facilityService.AddDataToJsonAsync(unit, cancellationToken);
                                break;
                            case "Tank":
                                var tank = CreateTankByUserInput(view);
                                if (tank is null)
                                {
                                    view.ShowMessage("Ошибка ввода");
                                    break;
                                }
                                await facilityService.AddDataToJsonAsync(tank, cancellationToken);
                                break;
                            default:
                                view.ShowMessage("Неизвестный тип объекта");
                                break;
                        }
                        break;

                    case "delete excel":
                        var inString = view.GetUserInput("Введите название объекта для удаления");
                        if (string.IsNullOrEmpty(inString))
                        {
                            view.ShowMessage("Пустой ввод");
                            break;
                        }

                        var facExcel = facilityService.Search(inString);
                        if (facExcel is null)
                        {
                            view.ShowMessage("Объект не найден");
                            break;
                        }

                        await facilityService.DeleteDataFromJsonAsync(facExcel, cancellationToken);
                        break;

                    case "write db":
                        await facilityService.WriteAllToDbAsync(cancellationToken);
                        break;
                    case "exit":
                        userInteractionFinished = true;
                        break;

                    default:
                        view.ShowMessage("Неизвестная команда");
                        break;
                }
            }
#nullable disable
            view.InputReceived -= HandleUserInput;
#nullable restore
        }
        private string? FindUnit(string tankName) => facilityService.FindUnit(tankName)?.ToString();
        private string? FindFactory(string unitName) => facilityService.FindFactory(unitName)?.ToString();

        private void HandleUserInput(object sender, UserInputEventArgs e) => view.ShowMessage($"Пользователь ввел: {e.UserInput} в {e.InputTime}");


        private static Tank? CreateTankByUserInput(IView view)
        {
            view.ShowMessage("Введите данные резервуара:");

            if (!int.TryParse(view.GetUserInput("Id:"), out var id))
            {
                view.ShowMessage("Ошибка ввода. Id должен быть числом");
                return null;
            }

            var name = view.GetUserInput("Name: ") ?? "Not set";

            var description = view.GetUserInput("Description: ") ?? "Not set";

            if (!int.TryParse(view.GetUserInput("Volume: "), out var volume))
            {
                view.ShowMessage("Ошибка ввода. Volume должен быть числом");
                return null;
            }

            if (!int.TryParse(view.GetUserInput("MaxVolume: "), out var maxVolume))
            {
                view.ShowMessage($"Ошибка ввода. MaxVolume должен быть числом");
                return null;
            }

            if (!int.TryParse(view.GetUserInput("UnitId: "), out var unitId))
            {
                view.ShowMessage($"Ошибка ввода. UnitId должен быть числом");
                return null;
            }

            return new Tank
            {
                Id = id,
                Name = name,
                Description = description,
                Volume = volume,
                MaxVolume = maxVolume,
                UnitId = unitId
            };
        }

        private static Factory? CreateFactoryByUserInput(IView view)
        {
            view.ShowMessage("Введите данные завода:");

            if (!int.TryParse(view.GetUserInput("Id:"), out var id))
            {
                view.ShowMessage("Ошибка ввода. Id должен быть числом");
                return null;
            }

            var name = view.GetUserInput("Name: ") ?? "Not set";

            var description = view.GetUserInput("Description: ") ?? "Not set";

            return new Factory
            {
                Id = id,
                Name = name,
                Description = description
            };
        }

        private static Unit? CreateUnitByUserInput(IView view)
        {

            if (!int.TryParse(view.GetUserInput("Id:"), out var id))
            {
                view.ShowMessage("Ошибка ввода. Id должен быть числом");
                return null;
            }

            var name = view.GetUserInput("Name: ") ?? "Not set";

            var description = view.GetUserInput("Description: ") ?? "Not set";

            if (!int.TryParse(view.GetUserInput("FactoryId: "), out var factoryId))
            {
                view.ShowMessage("Ошибка ввода. FactoryId должен быть числом");
                return null;
            }

            return new Unit
            {
                Id = id,
                Name = name,
                Description = description,
                FactoryId = factoryId
            };
        }
    }
}