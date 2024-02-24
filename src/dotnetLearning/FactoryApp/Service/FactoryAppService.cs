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
            if (options.Value.FacilitiesJsonFilePath is not null)
                await facilityService.DeserializeDataJson(options.Value.FacilitiesJsonFilePath);
            else
                throw new ArgumentNullException("Ошибка в файле конфигурации appsettings.json");

            view.ShowMessage(facilityService.GetCurrentConfiguration());
            view.ShowMessage(facilityService.GetTotalSummary());
            view.ShowMessage(facilityService.GetTotalVolumeTanks());

            var unit = facilityService.FindUnit("Резервуар 5");
            view.ShowMessage($"Резервуар 5 расположен на {unit?.ToString()}" ?? "Unit not found");

            var factory = facilityService.FindFactory("ГФУ-2");
            view.ShowMessage($"Установка ГФУ-2 расположена на {factory?.ToString()}" ?? "Factory not found");
            
            // Вдруг пригодится.
            //view.ShowMessage(facilityService.GetTanksSummary());
            //view.ShowMessage(facilityService.GetFactoriesSummary());
            //view.ShowMessage(facilityService.GetUnitsSummary());
        }
    }
}