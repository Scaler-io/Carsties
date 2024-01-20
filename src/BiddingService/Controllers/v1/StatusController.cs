using BiddingService.Swagger;
using Microsoft.AspNetCore.Mvc;
using Carsties.Shared.Extensions.Logger;
using Swashbuckle.AspNetCore.Annotations;

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
        Logger.Here().MethodEnterd();
        var healthResult = new { Status = "Ok" };
        Logger.Here().MethodExited();
        return Ok(healthResult);
    }
}
