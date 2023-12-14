using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuctionService.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _descriptionProvider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider descriptionProvider)
        {
            _descriptionProvider = descriptionProvider;
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach(var description in _descriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Carsties.Auction.Api",
                Version = description.ApiVersion.ToString(),
                Description = $"Carsties auction api - {description.GroupName}"
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }
            return info;
        }
    }
}
