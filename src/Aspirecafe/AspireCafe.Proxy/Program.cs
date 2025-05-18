using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-256-bit-secret")), // TODO: Use secure config
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.MapDefaultEndpoints();

// Use authentication and authorization
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

// Require authentication for all proxied routes except /auth
app.MapWhen(ctx => !ctx.Request.Path.StartsWithSegments("/auth"), appBuilder =>
{
    appBuilder.Use(async (context, next) =>
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Response.StatusCode = 401;
            await context.Response.CompleteAsync();
            return;
        }
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
