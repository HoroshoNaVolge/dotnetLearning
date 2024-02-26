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

            await facilityService.DeserializeDataJson(options.Value.FacilitiesJsonFilePath);

            // view.ShowMessage(facilityService.GetCurrentConfiguration());
            //view.ShowMessage(facilityService.GetTotalSummary());
            //view.ShowMessage(facilityService.GetTotalVolumeTanks());

            //var unit = facilityService.FindUnit("Резервуар 5");
            //view.ShowMessage($"Резервуар 5 расположен на {unit?.ToString()}" ?? "Unit not found");

            //var factory = facilityService.FindFactory("ГФУ-2");
            //view.ShowMessage($"Установка ГФУ-2 расположена на {factory?.ToString()}" ?? "Factory not found");

            CancellationTokenSource cancellationTokenSource = new();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            // await facilityService.SerializeDataJsonAsync(cancellationToken);
            //await facilityService.SerializeDataJsonAsync(new Tank() { Id = 6676, Description = "Ololoev", Name = "Onotole", MaxVolume = 100500, Volume = 0, UnitId = 2 }, cancellationToken);

            await facilityService.ExportDataToExcelAsync(cancellationToken);
            await facilityService.ImportDataFromExcelAsync(cancellationToken);

            // Вдруг пригодится.
            //view.ShowMessage(facilityService.GetTanksSummary());
            //view.ShowMessage(facilityService.GetFactoriesSummary());
            //view.ShowMessage(facilityService.GetUnitsSummary());
        }
    }
}