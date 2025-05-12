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


// Add services to the container.
builder.AddCosmosDbContext<CounterContext>("aspireCafe", "AspireCafe");
builder.Services.AddScoped<IFacade, Facade>();
builder.Services.AddScoped<IBusiness, Business>();
builder.Services.AddScoped<IData, Data>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<OrderViewModelValidator>();
// Add Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//force lowercase routes (for consistency with other services)
builder.Services.AddRouting(options => options.LowercaseUrls = true);

/* API VERSIONING */

builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

/* END API VERSIONING */

var app = builder.Build();


app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureOpenApiAndScaler();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
