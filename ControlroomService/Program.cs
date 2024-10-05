

using ControlroomService.DAL;
using ControlroomService.DAL.Interfaces;
using ControlroomService.DAL.Repositories;
using ControlroomService.BLL.Interfaces;
using ControlroomService.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// Tilføj Swagger-tjenester
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddScoped<IControlRoomRepository, ControlRoomRepository>();
builder.Services.AddScoped<IHardwareRepository, HardwareRepository>();

builder.Services.AddScoped<IControlRoomService, ControlRoomService>();
builder.Services.AddScoped<IHardwareService, HardwareService>();

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
