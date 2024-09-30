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

// Rute til gateway
app.Map("/{**path}", async (HttpContext context, IHttpClientFactory httpClientFactory) =>
{
    // Check om brugeren er godkendt
    if (!context.User.Identity?.IsAuthenticated ?? true)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return;
    }

    // Opbyg den nye URL til backend-tjenesten
    var path = context.Request.Path.ToString();
    var queryString = context.Request.QueryString.Value;
    var targetUrl = $"http://localhost:5001{path}{queryString}"; // Skift til din backend-tjenestes URL

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

    // Send forespørgslen til backend-tjenesten
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
