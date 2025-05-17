using AspireCafe.KitchenApiDomainLayer.Business;
using AspireCafe.KitchenApiDomainLayer.Data;
using AspireCafe.KitchenApiDomainLayer.Facade;
using AspireCafe.KitchenApiDomainLayer.Managers.Context;
using AspireCafe.Shared.Enums;
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
    AddDatabases(builder); //service based configuration - shouldn't be loaded in a shared extension method
    AddScopes(builder); //service based configuration - shouldn't be loaded in a shared extension method
    builder.AddVersioning(1);
    builder.AddExceptionHandling();
    builder.AddUniversalConfigurations();
    builder.AddSeq(); //if you choose to opt in to save your traces
    AddServiceBus(builder);
    AddRouteConstraints(builder);
}

void AddRouteConstraints(WebApplicationBuilder builder)
{
    builder.Services.Configure<RouteOptions>(options =>
    {
        options.ConstraintMap.Add("OrderProcessStation", typeof(OrderProcessStation));
        options.ConstraintMap.Add("OrderProcessStatus", typeof(OrderProcessStatus));
    });
}

void SetUpApp(WebApplication app)
{
    app.MapDefaultEndpoints();
    if (app.Environment.IsDevelopment())
    {
        app.ConfigureOpenApiAndScaler();
    }
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



