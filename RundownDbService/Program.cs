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
builder.Services.AddScoped<IRundownItemRepository, RundownItemRepository>();
builder.Services.AddScoped<IItemDetailRepository, ItemDetailRepository>();

builder.Services.AddScoped<IRundownService, RundownService>();
builder.Services.AddScoped<IRundownItemService, RundownItemService>();
builder.Services.AddScoped<IItemDetailService, ItemDetailService>();

builder.Services.AddSingleton<IKafkaService, KafkaService>();
builder.Services.AddSingleton<ResilienceService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rundown API", Version = "v1" });

//    // Tilføj PolymorphismSchemaFilter for nedarvede ItemDetails
//    c.SchemaFilter<PolymorphismSchemaFilter<ItemDetail>>();
//});


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

void RegisterMongoClassMaps()
{
    if (!BsonClassMap.IsClassMapRegistered(typeof(ItemDetail)))
    {
        BsonClassMap.RegisterClassMap<ItemDetail>(cm =>
        {
            cm.AutoMap();
            cm.SetIsRootClass(true);
            cm.SetDiscriminator("ItemDetail");
        });
    }

    if (!BsonClassMap.IsClassMapRegistered(typeof(ItemDetailVideo)))
    {
        BsonClassMap.RegisterClassMap<ItemDetailVideo>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("ItemDetailVideo");
            cm.MapMember(c => c.VideoPath).SetElementName("videoPath");
        });
    }

    if (!BsonClassMap.IsClassMapRegistered(typeof(ItemDetailTeleprompter)))
    {
        BsonClassMap.RegisterClassMap<ItemDetailTeleprompter>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("ItemDetailTeleprompter");
            cm.MapMember(c => c.PrompterText).SetElementName("prompterText");
        });
    }

    if (!BsonClassMap.IsClassMapRegistered(typeof(ItemDetailGraphic)))
    {
        BsonClassMap.RegisterClassMap<ItemDetailGraphic>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("ItemDetailGraphic");
            cm.MapMember(c => c.GraphicId).SetElementName("graphicId");
        });
    }

    if (!BsonClassMap.IsClassMapRegistered(typeof(ItemDetailComment)))
    {
        BsonClassMap.RegisterClassMap<ItemDetailComment>(cm =>
        {
            cm.AutoMap();
            cm.SetDiscriminator("ItemDetailComment");
            cm.MapMember(c => c.Comment).SetElementName("comment");
        });
    }
}

