﻿
using Microsoft.Extensions.Logging;

namespace CommonClassLibrary.DTO
{
    public class LogMessageDTO
    {
        public DateTime TimeStamp { get; set; }
        public string? Message { get; set; }
        public LogLevel LogLevel { get; set; }
        public string? Host { get; set; }
    }

}