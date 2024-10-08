namespace AggregatorService.DTO
{
    public class ResponseDTO<T>
    {
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
