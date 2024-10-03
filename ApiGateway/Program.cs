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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Pa55w0rd")) // vi elsker hardkodning af passwords
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

// Rute til gateway
app.Map("/{**path}", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{
    // Check om brugeren er godkendt
    if (!context.User.Identity?.IsAuthenticated ?? true)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
    }

    // Den nye URL der skal sendes til Aggragator
    var path = context.Request.Path.ToString();
    var queryString = context.Request.QueryString.Value;
    var targetUrl = $"https://localhost:3010{path}{queryString}"; // aggregator URL

    var client = httpClientFactory.CreateClient();
    var requestMessage = new HttpRequestMessage(new HttpMethod(context.Request.Method), targetUrl);

    // Kopier HTTP headers
    foreach (var header in context.Request.Headers)
    {
        if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
        {
            requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        }
    }

    // Kopier body (hvis der er nogen)
    if (context.Request.ContentLength > 0)
    {
        requestMessage.Content = new StreamContent(context.Request.Body);
    }

    // Send forespørgslen til aggregator
    var response = await client.SendAsync(requestMessage);

    // Returner svaret til klienten
    context.Response.StatusCode = (int)response.StatusCode;
    foreach (var header in response.Headers)
    {
        context.Response.Headers[header.Key] = header.Value.ToArray();
    }

    if (response.Content != null)
    {
        await response.Content.CopyToAsync(context.Response.Body);
    }
});

app.Run();
