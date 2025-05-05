using AspireCafe.BaristaApiDomainLayer.Business;
using AspireCafe.BaristaApiDomainLayer.Data;
using AspireCafe.BaristaApiDomainLayer.Facade;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddScoped<IFacade,Facade>();
builder.Services.AddScoped<IBusiness, Business>();
builder.Services.AddScoped<IData, Data>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
