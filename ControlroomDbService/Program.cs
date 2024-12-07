

using ControlRoomDbService.DAL;
using ControlRoomDbService.DAL.Interfaces;
using ControlRoomDbService.DAL.Repositories;
using ControlRoomDbService.BLL.Interfaces;
using ControlRoomDbService.BLL.Services;
using RemoteLoggerLibrary.Interfaces;
using RemoteLoggerLibrary.Providers;

var builder = WebApplication.CreateBuilder(args);

// Tilf�j Swagger-tjenester
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddScoped<IControlRoomRepository, ControlRoomRepository>();
builder.Services.AddScoped<IHardwareRepository, HardwareRepository>();

builder.Services.AddScoped<IControlRoomService, ControlRoomService>();
builder.Services.AddScoped<IHardwareService, HardwareService>();

builder.Services.AddSingleton<IKafkaService, KafkaService>();

builder.Services.AddSingleton<ILogService, LogService>();
builder.Services.AddSingleton<RemoteLogger>();
builder.Services.AddSingleton<RemoteLoggerProvider>();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.Services.AddSingleton<ILoggerProvider, RemoteLoggerProvider>();

    //loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);
    //loggingBuilder.AddFilter("System", LogLevel.Warning);
    //loggingBuilder.AddFilter("ControlRoomDbService", LogLevel.Information);
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
