using Asp.Versioning;
using AspireCafe.CounterApiDomainLayer.Business;
using AspireCafe.CounterApiDomainLayer.Data;
using AspireCafe.CounterApiDomainLayer.Facade;
using AspireCafe.CounterApiDomainLayer.Managers.Context;
using AspireCafe.CounterApiDomainLayer.Managers.Validators;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.Middleware;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
AddDatabases(builder); //service based configuration - shouldn't be loaded in a shared extension method
AddScopes(builder); //service based configuration - shouldn't be loaded in a shared extension method
AddFleuntValidation(builder); //service based configuration - shouldn't be loaded in a shared extension method
AddVersioning(builder); //might be able to be abstracted out with default version loaded as a parameter
AddExceptionHandling(builder); //should be abstracted away
AddUniversalConfigurations(builder); //should be abstracted away
var app = builder.Build();
app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment())
{
    app.ConfigureOpenApiAndScaler();
}
ConfigureApplicationDefaults(app);
app.Run();

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

void AddFleuntValidation(WebApplicationBuilder builder)
{
    builder.Services.AddValidatorsFromAssemblyContaining<OrderViewModelValidator>();
}

void AddVersioning(WebApplicationBuilder builder)
{
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("x-api-version"),
            new QueryStringApiVersionReader("api-version")
        );
    })
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    // Replace the placeholder with the actual version
    options.SubstituteApiVersionInUrl = false;
});
    builder.Services.AddOpenApi();
    builder.Services.AddOpenApi("v1");
}

void AddExceptionHandling(WebApplicationBuilder builder)
{
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
}

void AddUniversalConfigurations(WebApplicationBuilder builder)
{
    builder.Services.AddRouting(options => options.LowercaseUrls = true);
    builder.Services.AddControllers();
}
void ConfigureApplicationDefaults(WebApplication app)
{
    app.UseExceptionHandler();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}

