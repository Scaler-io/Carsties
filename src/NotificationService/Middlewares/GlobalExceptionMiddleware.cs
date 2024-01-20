using System.Net;
using System.Net.Mime;
using Carsties.Shared.Models.Core;
using Carsties.Shared.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Carsties.Shared.Extensions.Logger;

namespace NotificationService.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = _env.IsDevelopment()
                                        ? new ApiExceptionResponse(ex.Message, ex.StackTrace)
                                        : new ApiExceptionResponse(ex.Message);

        var jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter()
            }
        };
        var jsonResponse = JsonConvert.SerializeObject(response, jsonSettings);
        _logger.Here().Error("{@InternalServerError} - {@response}", ErrorCodes.InternalServerError, jsonResponse);
        await context.Response.WriteAsync(jsonResponse);
    }
}
