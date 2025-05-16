using Asp.Versioning;
using AspireCafe.Shared.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace AspireCafe.Shared.Extensions
{
    public static class PipelineExtensions
    {
        public static WebApplication ConfigureOpenApiAndScaler(this WebApplication app)
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options
                    .WithPreferredScheme("Bearer") // Security scheme name from the OpenAPI document
                    .WithHttpBearerAuthentication(bearer =>
                    {
                        bearer.Token = "your-bearer-token";
                    });
            });
            return app;
        }

        public static void AddExceptionHandling(this WebApplicationBuilder builder)
        {
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
        }

        public static void AddUniversalConfigurations(this WebApplicationBuilder builder)
        {
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddControllers();
        }
        public static void ConfigureApplicationDefaults(this WebApplication app)
        {
            app.UseExceptionHandler();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }

        public static void AddSeq(this WebApplicationBuilder builder)
        {
            builder.AddSeqEndpoint("seq");
        }

        public static void AddVersioning(this WebApplicationBuilder builder, int primaryVersion)
        {
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(primaryVersion, 0);
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
            builder.Services.AddOpenApi($"v{primaryVersion}");
        }
    }
}
