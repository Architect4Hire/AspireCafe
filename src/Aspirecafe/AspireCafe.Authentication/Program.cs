using AspireCafe.AuthenticationDomainLayer.Business;
using AspireCafe.AuthenticationDomainLayer.Facade;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.Middleware;

var builder = WebApplication.CreateBuilder(args);
SetUpBuilder(builder);
var app = builder.Build();
SetUpApp(app);
app.Run();

void SetUpBuilder(WebApplicationBuilder builder)
{
    builder.AddServiceDefaults();
    AddScopes(builder); //service based configuration - shouldn't be loaded in a shared extension method
    builder.AddVersioning(1);
    builder.AddExceptionHandling();
    builder.AddUniversalConfigurations();
    builder.AddSeq(); //if you choose to opt in to save your traces
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

    // Add authentication and authorization middleware before app.ConfigureApplicationDefaults()
    app.UseAuthentication();
    app.UseAuthorization();

    app.ConfigureApplicationDefaults();
}
void AddScopes(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IFacade, Facade>();
    builder.Services.AddScoped<IBusiness, Business>();
}



