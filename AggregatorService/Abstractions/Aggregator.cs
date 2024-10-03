namespace AggregatorService.Abstractions
{
    public abstract class Aggregator
    {
        public abstract Task<string> FetchData();

    }

}
