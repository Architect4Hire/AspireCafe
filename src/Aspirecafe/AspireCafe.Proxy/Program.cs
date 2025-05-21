using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Caching.Distributed;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Forwarder;
using System.Text;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-scary-password-a4h-aspire")), // TODO: Use secure config
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Register TokenProvider using IDistributedCache
builder.Services.AddSingleton<TokenProvider>();

// Add YARP reverse proxy with custom transform
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builderContext =>
    {
        builderContext.AddRequestTransform(async context =>
        {
            var tokenProvider = context.HttpContext.RequestServices.GetRequiredService<TokenProvider>();
            var headerToken = context.ProxyRequest.Headers.Authorization?.ToString();
            // Console.WriteLine($"[Proxy] Token from header: {headerToken}");
            var token = headerToken;
            // Console.WriteLine($"[Proxy] Token fetched {token}");
            if (string.IsNullOrEmpty(headerToken))
            {
                token = await tokenProvider.GetTokenAsync();
                // Console.WriteLine($"[Proxy] Token overridden from header");
            }
            if (!string.IsNullOrEmpty(token))
            {
                // Console.WriteLine($"[Proxy] Token set to header");
                context.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        });
    });

builder.AddRedisDistributedCache("cache");

var app = builder.Build();

app.MapDefaultEndpoints();
app.Use(async (context, next) =>
{
    // Console.WriteLine($"[Proxy] {context.Request.Method} {context.Request.Path} - Header Token: {context.Request.Headers["Authorization"]}");
    await next();
});
app.UseRouting();

// Require authentication for all proxied routes except /auth
app.MapWhen(ctx => !ctx.Request.Path.StartsWithSegments("/auth"), appBuilder =>
{
    appBuilder.Use(async (context, next) =>
    {
        await next();
    });
    // Use endpoints to map the reverse proxy
    appBuilder.UseEndpoints(endpoints =>
    {
        endpoints.MapReverseProxy();
    });
});

// Allow /auth endpoints without authentication
app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/auth"), appBuilder =>
{
    appBuilder.UseEndpoints(endpoints =>
    {
        endpoints.MapReverseProxy();
    });
});

app.Run();

// TokenProvider implementation using IDistributedCache
public class TokenProvider
{
    private readonly IDistributedCache _cache;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private static readonly string TokenCacheKey = "AspireCafe_AuthToken";
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(29);

    public TokenProvider(IDistributedCache cache, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _cache = cache;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<string?> GetTokenAsync()
    {
        var token = await _cache.GetStringAsync(TokenCacheKey);
        if (!string.IsNullOrEmpty(token))
        {
            return token;
        }
        // Fetch new token
        token = await FetchTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            await _cache.SetStringAsync(TokenCacheKey, token, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TokenLifetime });
        }
        return token;
    }

    private async Task<string?> FetchTokenAsync()
    {
        var client = _httpClientFactory.CreateClient();
        // TODO: Use config for URL and credentials
        var authApiUrl = _configuration["AuthenticationApi:Url"] ?? "http://localhost:5022/api/v1/authentication/generate";
        var username = _configuration["AuthenticationApi:Username"] ?? "admin";
        var password = _configuration["AuthenticationApi:Password"] ?? "password";
        var payload = new { UserName = username, Password = password };
        Console.WriteLine($"[Proxy] authApiUrl: {authApiUrl} - UserName: {username} - Password: {password}");
        var response = await client.PostAsJsonAsync(authApiUrl, payload);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResult>();
            return result?.data?.token;
        }
        return null;
    }

    private class AuthResult
    {
        public AuthData? data { get; set; }
    }
    private class AuthData
    {
        public string? token { get; set; }
    }
}