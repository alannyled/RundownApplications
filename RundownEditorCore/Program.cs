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

var builder = WebApplication.CreateBuilder(args);

// Add named HttpClients for different APIs
builder.Services.AddHttpClient("TemplatesAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7247/api/");
});

// Add services to the container.
builder.Services.AddHttpClient<IRundownService, RundownService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:3010/api/Rundown/"); // ikke APIGateway adresse!
});

builder.Services.AddHttpClient<IHardwareService, HardwareService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:3010/api/Hardware/"); // ikke APIGateway adresse!
});
builder.Services.AddHttpClient<IControlRoomService, ControlRoomService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:3010/api/ControlRoom/"); // ikke APIGateway adresse!
});
builder.Services.AddHttpClient<ITemplateService, TemplateService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:3010/api/Template/"); // ikke APIGateway adresse!
});

builder.Services.AddLogging(loggingBuilder =>
{
    //loggingBuilder.ClearProviders(); // Fjern standard loggere
    loggingBuilder.AddProvider(new InMemoryLoggerProvider()); // Tilføj  custom logger
    //loggingBuilder.AddFilter("Microsoft", LogLevel.Warning); // Logger kun advarsler eller højere fra Microsoft namespace
    //loggingBuilder.AddFilter("System", LogLevel.Warning); // Logger kun advarsler eller højere fra System namespace
    //loggingBuilder.AddFilter("RundownEditorCore", LogLevel.Information); // Log kun for app namespace
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
builder.Services.AddSingleton<IKafkaService, KafkaService>();
builder.Services.AddSingleton<IMessageBuilderService, MessageBuilderService>();

builder.Services.AddHostedService<KafkaBackgroundService>();
builder.Services.AddScoped<ThemeService>();



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


