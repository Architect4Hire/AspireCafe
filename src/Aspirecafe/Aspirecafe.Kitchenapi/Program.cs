using AspireCafe.KitchenApiDomainLayer.Business;
using AspireCafe.KitchenApiDomainLayer.Data;
using AspireCafe.KitchenApiDomainLayer.Facade;
using AspireCafe.KitchenApiDomainLayer.Managers.Context;
using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
SetUpBuilder(builder);
var app = builder.Build();
SetUpApp(app);
app.Run();

void SetUpBuilder(WebApplicationBuilder builder)
{
    builder.AddServiceDefaults();
    AddDatabases(builder); //service based configuration - shouldn't be loaded in a shared extension method
    AddScopes(builder); //service based configuration - shouldn't be loaded in a shared extension method
    builder.AddVersioning(1);
    builder.AddExceptionHandling();
    builder.AddUniversalConfigurations();
    builder.AddSeq(); //if you choose to opt in to save your traces
    AddServiceBus(builder);
    AddRouteConstraints(builder);
    AddAuthentication(builder);
}

void AddAuthentication(WebApplicationBuilder builder)
{
    // Add JWT Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "your-256-bit-secret");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // In production, set to true with a valid issuer
            ValidateAudience = false, // In production, set to true with a valid audience
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

    // Add Authorization policies
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAuthenticatedUser", policy =>
            policy.RequireAuthenticatedUser());
    });
}

void AddRouteConstraints(WebApplicationBuilder builder)
{
    builder.Services.Configure<RouteOptions>(options =>
    {
        options.ConstraintMap.Add("OrderProcessStation", typeof(OrderProcessStationRouteConstraint));
        options.ConstraintMap.Add("OrderProcessStatus", typeof(OrderProcessStatusRouteConstraint));
    });
}

void SetUpApp(WebApplication app)
{
    app.MapDefaultEndpoints();
    if (app.Environment.IsDevelopment())
    {
        app.ConfigureOpenApiAndScaler();
    }
    app.UseAuthentication();
    app.UseAuthorization();
    app.ConfigureApplicationDefaults();
}

void AddServiceBus(WebApplicationBuilder builder)
{
    builder.AddAzureServiceBusClient("serviceBusConnection");

}

void AddDatabases(WebApplicationBuilder builder)
{
    builder.AddCosmosDbContext<KitchenContext>("aspireCafe", "AspireCafe");
}

void AddScopes(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IFacade, Facade>();
    builder.Services.AddScoped<IBusiness, Business>();
    builder.Services.AddScoped<IData, Data>();
}



