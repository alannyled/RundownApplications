using AggregatorService.Abstractions;
using AggregatorService.Factories;
using AggregatorService.Managers;
using AggregatorService.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.Configure<ApiUrls>(builder.Configuration.GetSection("ApiUrls"));

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
        // options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
    });

builder.Services.AddHttpClient<ControlRoomService>()
    .AddTypedClient<Aggregator>((httpClient, serviceProvider) =>
    {
        var apiUrls = serviceProvider.GetRequiredService<IOptions<ApiUrls>>();
        var cacheService = serviceProvider.GetRequiredService<ICacheService>();
        return new ControlRoomService(httpClient, apiUrls, cacheService);
    });

builder.Services.AddHttpClient<HardwareService>()
    .AddTypedClient<Aggregator>((httpClient, serviceProvider) =>
    {
        var apiUrls = serviceProvider.GetRequiredService<IOptions<ApiUrls>>();
        var cacheService = serviceProvider.GetRequiredService<ICacheService>();
        return new HardwareService(httpClient, apiUrls, cacheService);
    });

builder.Services.AddHttpClient<RundownService>()
    .AddTypedClient<Aggregator>((httpClient) => new RundownService(httpClient));

builder.Services.AddHttpClient<TemplateService>()
    .AddTypedClient<Aggregator>((httpClient) => new TemplateService(httpClient));


builder.Services.AddSingleton<ServiceFactory>();
builder.Services.AddSingleton<ControlRoomManager>();
builder.Services.AddSingleton<RundownManager>();
builder.Services.AddSingleton<TemplateManager>();
builder.Services.AddSingleton<HardwareManager>();


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
