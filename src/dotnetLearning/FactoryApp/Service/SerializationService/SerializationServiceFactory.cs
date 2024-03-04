using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetLearning.FactoryApp.Service.SerializationService
{
    public interface ISerializationServiceFactory
    {
        public ISerializationService CreateService(SerializationServiceType serviceType);
    }

    public class SerializationServiceFactory(IServiceProvider serviceProvider) : ISerializationServiceFactory
    {
        public ISerializationService CreateService(SerializationServiceType serviceType)
        {
            return serviceType switch
            {
                SerializationServiceType.Json => serviceProvider.GetService<JsonService>()!,
                SerializationServiceType.Excel => serviceProvider.GetService<ExcelService>()!,
                SerializationServiceType.Db => serviceProvider.GetService<DbFacilitiesService>()!,
                _ => throw new ArgumentException("Ошибка в типе сервиса сериализации"),
            };
        }
    }
    public enum SerializationServiceType
    {
        Json,
        Excel,
        Db
    }
}
