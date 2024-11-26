using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Diagnostics;
using RundownEditorCore.Components;
using RundownEditorCore.Components.Account;
using RundownEditorCore.Data;
using Microsoft.Extensions.Options;
using RundownEditorCore.Services;
using RundownEditorCore.Interfaces;
using RundownEditorCore.States;
using RemoteLoggerLibrary.Interfaces;
using RemoteLoggerLibrary.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IRundownService, RundownService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:3000/api/Rundown/"); // APIGateway adressen!
});

builder.Services.AddHttpClient<IHardwareService, HardwareService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:3000/api/Hardware/"); // APIGateway adressen!
});
builder.Services.AddHttpClient<IControlRoomService, ControlRoomService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:3000/api/ControlRoom/"); // APIGateway adressen!
});
builder.Services.AddHttpClient<ITemplateService, TemplateService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:3000/api/Template/"); // APIGateway adressen!
});


builder.Services.AddSingleton<IKafkaService, KafkaService>();
builder.Services.AddSingleton<ILogService, LogService>();
builder.Services.AddSingleton<RemoteLogger>();
builder.Services.AddSingleton<RemoteLoggerProvider>();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.Services.AddSingleton<ILoggerProvider, RemoteLoggerProvider>();

    //loggingBuilder.AddFilter("Microsoft", LogLevel.Warning);
    //loggingBuilder.AddFilter("System", LogLevel.Warning);
    //loggingBuilder.AddFilter("ControlRoomDbService", LogLevel.Information);
});


builder.Services.AddScoped<RundownState>();
builder.Services.AddScoped<ModalState>();
builder.Services.AddScoped<ToastState>();
builder.Services.AddSingleton<DetailLockState>();
builder.Services.AddSingleton<SharedStates>();
builder.Services.AddScoped<FormRenderService>();
builder.Services.AddSingleton(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    return new KafkaServiceLibrary.KafkaService(configuration);
});

builder.Services.AddSingleton<IMessageBuilderService, MessageBuilderService>();

builder.Services.AddHostedService<KafkaBackgroundService>();
builder.Services.AddScoped<ThemeService>();

builder.Services.AddHostedService(provider =>
    new HealthCheckService(
        new Dictionary<string, string>
        {
            {"Database Rundown Primary", "http://localhost:27027" },
            {"Database Rundown Seconday", "http://localhost:27028" },
            {"Database Template Primary", "http://localhost:27037" },
            {"Database Template Secondary", "http://localhost:27038" },
            {"Database ControlRoom Primary", "http://localhost:27017" },
            {"Database ControlRoom Secondary", "http://localhost:27018" },
            {"Kafka ZooKeeper", "tcp://localhost:2181" },
            {"Kafka Message Broker", "tcp://localhost:9092" },
            {"API Gateway", "tcp://localhost:3000" },
            {"MicroService LogStore SSL", "https://localhost:3050/api/Log" },
            {"MicroService LogStore", "http://localhost:3051/api/Log" },
            {"MicroService Aggregator SSL", "https://localhost:3010/health" },
            {"MicroService Aggregator", "http://localhost:3011/health" },
            {"MicroService ControlRoom SSL", "https://localhost:3020/health" },
            {"MicroService ControlRoom", "http://localhost:3021/health" },
            {"MicroService Rundown SSL", "https://localhost:3030/health" },
            {"MicroService Rundown", "http://localhost:3031/health" },
            {"MicroService Template SSL", "https://localhost:3040/health" },
            {"MicroService Template", "http://localhost:3041/health" }
        },
        provider.GetRequiredService<SharedStates>()
    ));

builder.Services.AddRazorPages();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// brug cookie til temaet
app.Use(async (context, next) =>
{
    var theme = context.Request.Cookies["theme"] ?? "light"; // Fallback til "light"
    context.Items["Theme"] = theme;

    await next.Invoke();
});


app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorPages();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();


