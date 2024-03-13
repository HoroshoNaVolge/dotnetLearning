using Factories.WebApi.DAL.Entities;
using Factories.WebApi.DAL.Interfaces;
using Factories.WebApi.DAL.Repositories;

namespace Factories.WebApi.BLL.Services
{
    public interface IWorkerService
    {
        void UpdateAllVolumesRandomly();
    }
    public class WorkerService(ILogger<WorkerService> logger, IUnitOfWork unitOfWork) : BackgroundService, IWorkerService
    {
        private readonly ILogger logger = logger;
        private readonly TankRepository? tankRepository = unitOfWork.Tanks as TankRepository;

        public async void UpdateAllVolumesRandomly()
        {
            await ExecuteAsync(new CancellationToken());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    tankRepository?.UpdateAllVolumesRandomly();
                }
                catch (InvalidOperationException ex)
                {
                    logger.LogError(ex, "Ошибка при обновлении объема резервуара.");
                }
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}