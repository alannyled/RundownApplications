using AggregatorService.Abstractions;
using AggregatorService.Factories;
using AggregatorService.Managers;
using AggregatorService.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<ApiUrls>(builder.Configuration.GetSection("ApiUrls"));
builder.Services.AddControllers();

builder.Services.AddHttpClient<ControlRoomService>()
    .AddTypedClient<Aggregator>((httpClient, serviceProvider) =>
    {
        var apiUrls = serviceProvider.GetRequiredService<IOptions<ApiUrls>>().Value;
        return new ControlRoomService(httpClient, serviceProvider.GetRequiredService<IOptions<ApiUrls>>());
    });

builder.Services.AddHttpClient<HardwareService>()
    .AddTypedClient<Aggregator>((httpClient, serviceProvider) =>
    {
        var apiUrls = serviceProvider.GetRequiredService<IOptions<ApiUrls>>().Value;
        return new HardwareService(httpClient, serviceProvider.GetRequiredService<IOptions<ApiUrls>>());
    });

builder.Services.AddHttpClient<RundownService>()
    .AddTypedClient<Aggregator>((httpClient, serviceProvider) =>
    {
        var apiUrls = serviceProvider.GetRequiredService<IOptions<ApiUrls>>().Value;
        return new RundownService(httpClient, serviceProvider.GetRequiredService<IOptions<ApiUrls>>());
    });


builder.Services.AddSingleton<ServiceFactory>();
builder.Services.AddSingleton<ControlRoomManager>();
builder.Services.AddSingleton<RundownManager>();

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