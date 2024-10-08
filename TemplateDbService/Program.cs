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
