using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BiddingService.Swagger
{
    public class SwaggerApiVersionFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var path in swaggerDoc.Paths.ToList())
            {
                var newPath = path.Key.Replace("/api/v1", string.Empty);
                swaggerDoc.Paths.Remove(path.Key);
                swaggerDoc.Paths.Add(newPath, path.Value);
            }
        }
    }
}
