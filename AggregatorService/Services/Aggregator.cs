namespace AggregatorService.Services
{
    public abstract class Aggregator
    {
        public abstract Task<string> FetchData();
    }

}
