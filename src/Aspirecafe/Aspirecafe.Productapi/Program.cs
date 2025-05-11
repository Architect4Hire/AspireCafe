using AspireCafe.ProductApiDomainLayer.Business;
using AspireCafe.ProductApiDomainLayer.Data;
using AspireCafe.ProductApiDomainLayer.Facade;
using AspireCafe.ProductApiDomainLayer.Managers.Context;
using AspireCafe.ProductApiDomainLayer.Managers.Validators;
using AspireCafe.Shared.Extensions;
using AspireCafe.Shared.Middleware;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.AddRedisDistributedCache("cache");
builder.AddCosmosDbContext<ProductContext>("aspireCafe", "AspireCafe");
builder.Services.AddScoped<IFacade, Facade>();
builder.Services.AddScoped<IBusiness, Business>();
builder.Services.AddScoped<IData, Data>();
builder.Services.AddScoped<ICatalogFacade, CatalogFacade>();
builder.Services.AddScoped<ICatalogBusiness, CatalogBusiness>();
builder.Services.AddScoped<ICatalogData, CatalogData>();
builder.Services.AddValidatorsFromAssemblyContaining<ProductViewModelValidator>();

//add exception handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//force lowercase routes (for consistency with other services)
builder.Services.AddRouting(options => options.LowercaseUrls = true);

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
