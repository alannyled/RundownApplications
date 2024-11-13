using LogStoreService;
using MongoDB.Bson.Serialization;
using LogStoreService.BLL.Services;
using LogStoreService.BLL.Interfaces;
using LogStoreService.DAL.Interfaces;
using LogStoreService.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return new KafkaServiceLibrary.KafkaService(configuration);
});
builder.Services.AddSingleton<ILogStoreService, LogService>();
builder.Services.AddSingleton<ILogStoreRepository, LogStoreRepository>();
builder.Services.AddHostedService<KafkaBackgroundService>();
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
