
using Microsoft.AspNetCore.Mvc;

namespace SearchService.Controllers.v2;

[ApiVersion("2")]
public class SearchController : BaseApiController
{
    public SearchController(ILogger logger)
        : base(logger)
    {
    }

    [HttpGet]
    public IActionResult GetSearchResult()
    {
        return Ok("search is fine");
    }
}
