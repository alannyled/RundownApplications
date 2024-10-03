using AggregatorService.Abstractions;
using AggregatorService.Factories;
using AggregatorService.Managers;
using AggregatorService.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();


builder.Services.AddHttpClient<ControlRoomService>()
    .AddTypedClient<Aggregator>(client => new ControlRoomService(client));

builder.Services.AddHttpClient<HardwareService>()
    .AddTypedClient<Aggregator>(client => new HardwareService(client));

builder.Services.AddSingleton<ServiceFactory>();

builder.Services.AddSingleton<AggregatorManager>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
