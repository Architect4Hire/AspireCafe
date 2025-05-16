using AspireCafe.BaristaApiDomainLayer.Data;
using AspireCafe.BaristaApiDomainLayer.Facade;
using AspireCafe.OrderSummaryApiDomainLayer.Business;
using AspireCafe.Shared.Extensions;

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
    AddFluentValidation(builder); //service based configuration - shouldn't be loaded in a shared extension method
    builder.AddVersioning(1);
    builder.AddExceptionHandling();
    builder.AddUniversalConfigurations();
    builder.AddSeq(); //if you choose to opt in to save your traces
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
void AddDatabases(WebApplicationBuilder builder)
{
    
}

void AddScopes(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IFacade, Facade>();
    builder.Services.AddScoped<IBusiness, Business>();
    builder.Services.AddScoped<IData, Data>();
}

void AddFluentValidation(WebApplicationBuilder builder)
{
    
}


