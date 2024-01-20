using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BiddingService.Swagger
{
    public class SwaggerApiVersionFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var description in context.ApiDescriptions)
            {
                foreach (var path in swaggerDoc.Paths.ToList())
                {
                    var newPath = path.Key.Replace($"/api/{description.GroupName}", string.Empty);
                    swaggerDoc.Paths.Remove(path.Key);
                    swaggerDoc.Paths.Add(newPath, path.Value);
                }

            }
        }
    }
}
