using Microsoft.AspNetCore.Builder;
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
    }
}
