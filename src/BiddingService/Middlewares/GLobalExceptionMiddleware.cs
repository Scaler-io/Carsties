using System.Net.Mime;
using System.Net;
using Carsties.Shared.Models.Core;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Carsties.Shared.Extensions.Logger;
using Carsties.Shared.Models.Enums;

namespace BiddingService.Middlewares;

public class GLobalExceptionMiddleware
{
    public RequestDelegate _next { get; set; }
    private ILogger _logger;
    private IWebHostEnvironment _environment;

    public GLobalExceptionMiddleware(RequestDelegate next,
        IWebHostEnvironment environment,
        ILogger logger)
    {
        _next = next;
        _environment = environment;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleGlobalException(context, ex);
        }
    }

    private async Task HandleGlobalException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        var response = _environment.IsDevelopment()
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
