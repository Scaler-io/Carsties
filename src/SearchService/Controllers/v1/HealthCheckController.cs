using Microsoft.AspNetCore.Mvc;

namespace SearchService.Controllers.v1;

[ApiVersion("1")]
public class HealthCheckController : BaseApiController
{
    public HealthCheckController(ILogger logger)
        : base(logger)
    {
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok("Health is good");
    }
}
