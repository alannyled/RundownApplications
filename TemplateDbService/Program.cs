using RemoteLoggerLibrary.Interfaces;
using RemoteLoggerLibrary.Providers;
using CommonClassLibrary.Services;
using MongoDB.Driver;
using TemplateDbService.BLL.Interfaces;
using TemplateDbService.BLL.Services;
using TemplateDbService.DAL;
using TemplateDbService.DAL.Interfaces;
using TemplateDbService.DAL.Repositories;
using TemplateDbService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRundownTemplateRepository, RundownTemplateRepository>();
builder.Services.AddScoped<IItemTemplateRepository, ItemTemplateRepository>();
builder.Services.AddScoped<IRundownTemplateService, RundownTemplateService>();
builder.Services.AddScoped<IItemTemplateService, ItemTemplateService>();

builder.Services.AddSingleton<ResilienceService>();
builder.Services.AddSingleton<IKafkaService, KafkaService>();

builder.Services.AddSingleton<ILogService, LogService>();
builder.Services.AddSingleton<RemoteLogger>();
builder.Services.AddSingleton<RemoteLoggerProvider>();

builder.Services.AddLogging(loggingBuilder =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    var remoteLoggerProvider = serviceProvider.GetRequiredService<RemoteLoggerProvider>();

    loggingBuilder.AddProvider(remoteLoggerProvider);

    //loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);
    //loggingBuilder.AddFilter("System", LogLevel.Warning);
    //loggingBuilder.AddFilter("TemplateDbService", LogLevel.Information);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
