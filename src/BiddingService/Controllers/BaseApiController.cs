using BiddingService.Extensions;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BiddingService.Controllers
{
    [Route("api/v{version:apiVersion}/bids")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly ILogger Logger;

        public BaseApiController(ILogger logger)
        {
            Logger = logger;
        }

        protected UserDto CurrentUser => User.Identity.IsAuthenticated ? GetCurrentUser() : null;

        protected RequestInformation RequestInformation => new RequestInformation
        {
            CurrentUser = CurrentUser,
            CorrelationId = GetOrGenerateCorelationId()
        };

        protected IActionResult OkOrFailure<T>(Result<T> result)
        {
            if (result == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if (result.IsSuccess && result.Value == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);

            return result.ErrorCode switch
            {
                ErrorCodes.BadRequest => BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage)),
                ErrorCodes.InternalServerError => InternalServerError(new ApiResponse(ErrorCodes.InternalServerError, result.ErrorMessage)),
                ErrorCodes.NotFound => NotFound(new ApiResponse(ErrorCodes.NotFound, result.ErrorMessage)),
                ErrorCodes.Unauthorized => Unauthorized(new ApiResponse(ErrorCodes.Unauthorized, result.ErrorMessage)),
                ErrorCodes.OperationFailed => BadRequest(new ApiResponse(ErrorCodes.OperationFailed, result.ErrorMessage)),
                _ => BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage))
            };
        }

        public IActionResult CreatedResult<T>(Result<T> result, string routeName, object param)
        {
            if (result.IsSuccess && result.Value != null) return CreatedAtRoute(
                    routeName,
                    param,
                    result.Value
                );

            return OkOrFailure(result);
        }

        private UserDto GetCurrentUser()
        {
            return new UserDto
            {
                Id = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Name = User.FindFirst("name").Value,
                Username = User.FindFirst("username").Value
            };
        }

        protected string GetOrGenerateCorelationId() => Request?.GetRequestHeaderOrdefault("CorrelationId", $"GEN-{Guid.NewGuid().ToString()}");

        private ObjectResult InternalServerError(ApiResponse response)
        {
            return new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                ContentTypes = new Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection
                {
                    "application/json"
                }
            };
        }
    }
}
