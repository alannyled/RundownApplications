using RemoteLoggerLibrary.Interfaces;
using RemoteLoggerLibrary.Providers;
using CommonClassLibrary.Services;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using RundownDbService.BLL.Interfaces;
using RundownDbService.BLL.Services;
using RundownDbService.DAL;
using RundownDbService.DAL.Interfaces;
using RundownDbService.DAL.Repositories;
using RundownDbService.Models;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
        // options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
    });



// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddScoped<IRundownRepository, RundownRepository>();
builder.Services.AddScoped<IRundownStoryRepository, RundownStoryRepository>();
builder.Services.AddScoped<IStoryDetailRepository, StoryDetailRepository>();

builder.Services.AddScoped<IRundownService, RundownService>();
builder.Services.AddScoped<IRundownStoryService, RundownStoryService>();
builder.Services.AddScoped<IStoryDetailService, StoryDetailService>();

builder.Services.AddSingleton<IKafkaService, KafkaService>();
builder.Services.AddSingleton<ResilienceService>();

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
    //loggingBuilder.AddFilter("RundownDbService", LogLevel.Information);
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();
RegisterMongoClassMaps();



app.Run();

static void RegisterMongoClassMaps()
{
    if (!BsonClassMap.IsClassMapRegistered(typeof(StoryDetail)))
    {
        BsonClassMap.RegisterClassMap<StoryDetail>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true);
            cm.SetDiscriminator("StoryDetail");
        });
    }

    if (!BsonClassMap.IsClassMapRegistered(typeof(StoryDetailVideo)))
    {
        BsonClassMap.RegisterClassMap<StoryDetailVideo>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("StoryDetailVideo");
            cm.MapMember(c => c.VideoPath).SetElementName("videoPath");
        });
    }

    if (!BsonClassMap.IsClassMapRegistered(typeof(StoryDetailTeleprompter)))
    {
        BsonClassMap.RegisterClassMap<StoryDetailTeleprompter>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("StoryDetailTeleprompter");
            cm.MapMember(c => c.PrompterText).SetElementName("prompterText");
        });
    }

    if (!BsonClassMap.IsClassMapRegistered(typeof(StoryDetailGraphic)))
    {
        BsonClassMap.RegisterClassMap<StoryDetailGraphic>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("StoryDetailGraphic");
            cm.MapMember(c => c.GraphicId).SetElementName("graphicId");
        });
    }

    if (!BsonClassMap.IsClassMapRegistered(typeof(StoryDetailComment)))
    {
        BsonClassMap.RegisterClassMap<StoryDetailComment>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("StoryDetailComment");
            cm.MapMember(c => c.Comment).SetElementName("comment");
        });
    }
}



