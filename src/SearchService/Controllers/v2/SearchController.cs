
using Carsties.Shared.Extensions.Logger;
using Microsoft.AspNetCore.Mvc;
using SearchService.Services;
using SearchService.Swagger;
using Swashbuckle.AspNetCore.Annotations;

namespace SearchService.Controllers.v2;

[ApiVersion("2")]
public class SearchController : BaseApiController
{
    private readonly ISearchService _searchService;
    public SearchController(ILogger logger, ISearchService searchService)
        : base(logger)
    {
        _searchService = searchService;
    }

    [HttpGet]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id")]
    [SwaggerOperation(OperationId = "SearchItems", Description = "Searches items base on search term")]
    public async Task<IActionResult> SearchItems([FromQuery] string searchTerm)
    {
        Logger.Here().MethodEnterd();
        var result = await _searchService.SearchAsync(searchTerm, RequestInformation.CorrelationId);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }
}
