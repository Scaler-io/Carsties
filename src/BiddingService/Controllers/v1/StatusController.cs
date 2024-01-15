using BiddingService.Swagger;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BiddingService.Controllers.v1;

[ApiVersion("1")]
public class StatusController : BaseApiController
{
    public StatusController(ILogger logger) : 
        base(logger)
    {
    }

    [HttpGet("status")]
    [SwaggerHeader("CorrelationId", Description = "expects unique correlation id", Type = "string")]
    [SwaggerOperation(OperationId = "HelathCheck", Description = "Get api health status")]
    public IActionResult HelathCheck()
    {
        var healthResult = new { Status = "Ok" };
        return Ok(healthResult);
    }
}
