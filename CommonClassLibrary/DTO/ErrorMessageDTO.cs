

namespace CommonClassLibrary.DTO
{
    public class ErrorMessageDTO
    {
        public string Action { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Time { get; set; }
    }
}
