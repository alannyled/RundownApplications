using Microsoft.OpenApi.Models;
using RundownDbService.BLL.Interfaces;
using RundownDbService.BLL.Services;
using RundownDbService.DAL;
using RundownDbService.DAL.Interfaces;
using RundownDbService.DAL.Repositories;
using RundownDbService.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddControllers();

builder.Services.AddScoped<IRundownRepository, RundownRepository>();
builder.Services.AddScoped<IRundownItemRepository, RundownItemRepository>();
builder.Services.AddScoped<IItemDetailRepository, ItemDetailRepository>();

builder.Services.AddScoped<IRundownService, RundownService>();
builder.Services.AddScoped<IRundownItemService, RundownItemService>();
builder.Services.AddScoped<IItemDetailService, ItemDetailService>();

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

app.Run();
