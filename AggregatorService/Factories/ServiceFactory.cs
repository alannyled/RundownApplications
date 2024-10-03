using AggregatorService.Abstractions;

namespace AggregatorService.Factories
{
    public class ServiceFactory(IEnumerable<Aggregator> services)
    {
        private readonly IEnumerable<Aggregator> _services = services;

        public T GetService<T>() where T : Aggregator
        {
            return (T)_services.First(s => s is T);
        }
    }
}

