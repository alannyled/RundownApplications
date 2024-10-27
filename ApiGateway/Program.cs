///<summary>
/// Dette er en meget simpel API Gateway.
/// Den lytter på port 3010 og videresender alle anmodninger til Aggregator.
/// Den bruger JWT-godkendelse og autorisation til at beskytte adgangen til gatewayen.
/// Den er tænkt som et Proof of Concept og bør ikke bruges i produktion.
/// Her forventer jeg at anvende Kong, som er den API Gateway, der anvendes i produktion.
/// </summary>

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Tilføj JWT-godkendelse
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Pa55w0rd"))
        };
    });

// Tilføj autorisation
builder.Services.AddAuthorization();

// Tilføj HttpClientFactory
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

///<summary>
/// Middleware til at buffer Request Body som String
/// </summary>
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering(); // Tillad at læse body'en flere gange

    if (context.Request.ContentLength > 0 || context.Request.Headers.ContainsKey("Transfer-Encoding"))
    {
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
        string bodyContent = await reader.ReadToEndAsync();
        // Reset stream-positionen for at tillade efterfølgende læsning
        context.Request.Body.Position = 0;
    }
    // Gå videre til næste middleware
    await next.Invoke();
});

///<summary>
/// API Gateway, der videresender alle requests til en aggregator
/// </summary> 
app.Map("/{**path}", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{
    var path = context.Request.Path.ToString();
    var queryString = context.Request.QueryString.Value;
    var targetUrl = $"https://localhost:3010{path}{queryString}";

    Console.WriteLine($"Videresender til {targetUrl}");

    var client = httpClientFactory.CreateClient();
    var requestMessage = new HttpRequestMessage(new HttpMethod(context.Request.Method), targetUrl);

    // Kopier alle headers undtagen Transfer-Encoding
    foreach (var header in context.Request.Headers)
    {
        if (header.Key.Equals("Transfer-Encoding", StringComparison.OrdinalIgnoreCase)) continue;
        requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
    }

    // Buffer hele request body for videresendelse, hvis der er en
    if (context.Request.ContentLength > 0 || context.Request.Headers.ContainsKey("Transfer-Encoding"))
    {
        using var reader = new StreamReader(context.Request.Body);
        var incomingBody = await reader.ReadToEndAsync();
        // Opret body-content med korrekt content type
        requestMessage.Content = new StringContent(incomingBody, Encoding.UTF8, "application/json");
    }
    var response = await client.SendAsync(requestMessage);
    context.Response.StatusCode = (int)response.StatusCode;

    // Kopier alle response headers
    foreach (var header in response.Headers)
    {
        context.Response.Headers[header.Key] = header.Value.ToArray();
    }

    foreach (var contentHeader in response.Content.Headers)
    {
        context.Response.Headers[contentHeader.Key] = contentHeader.Value.ToArray();
    }

    context.Response.Headers.Remove("Transfer-Encoding");

    if (response.Content != null)
    {
        await response.Content.CopyToAsync(context.Response.Body);
    }
});

app.Run();
