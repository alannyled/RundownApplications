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
    loggingBuilder.ClearProviders(); // Fjerner alle standard loggere
    loggingBuilder.AddProvider(new InMemoryLoggerProvider()); // Tilføj  custom logger
    loggingBuilder.AddFilter("Microsoft", LogLevel.Warning); // Logger kun advarsler eller højere fra Microsoft namespace
    loggingBuilder.AddFilter("System", LogLevel.Warning); // Logger kun advarsler eller højere fra System namespace
    loggingBuilder.AddFilter("RundownEditorCore", LogLevel.Information); // Log kun for app namespace
});



builder.Services.AddScoped<FormRenderService>(); 



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


