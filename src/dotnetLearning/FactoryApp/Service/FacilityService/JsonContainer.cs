using dotnetLearning.FactoryApp.Model;

namespace dotnetLearning.FactoryApp.Service.FacilityService
{
    internal class JsonContainer(IEnumerable<Factory> factory, IEnumerable<Unit> unit, IEnumerable<Tank> tank)
    {
        public IEnumerable<Factory> Factory { get; set; } = factory;
        public IEnumerable<Unit> Unit { get; set; } = unit;
        public IEnumerable<Tank> Tank { get; set; } = tank;
    }
}
