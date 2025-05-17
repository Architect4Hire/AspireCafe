using AspireCafe.CounterApiDomainLayer.Business;
using AspireCafe.CounterApiDomainLayer.Data;
using AspireCafe.CounterApiDomainLayer.Facade;
using AspireCafe.CounterApiDomainLayer.Managers.Context;
using AspireCafe.CounterApiDomainLayer.Managers.Validators;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.HttpClients;
using FluentValidation;
using Refit;

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
    AddHttpClient(builder);
    AddServiceBus(builder);
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

void AddHttpClient(WebApplicationBuilder builder)
{
    builder.Services.AddRefitClient<IProductHttpClient>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration["productApi"] ?? ""));
}

void AddDatabases(WebApplicationBuilder builder)
{
    builder.AddCosmosDbContext<CounterContext>("aspireCafe", "AspireCafe");
}

void AddScopes(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IFacade, Facade>();
    builder.Services.AddScoped<IBusiness, Business>();
    builder.Services.AddScoped<IData, Data>();
}

void AddFluentValidation(WebApplicationBuilder builder)
{
    builder.Services.AddValidatorsFromAssemblyContaining<OrderViewModelValidator>();
}


