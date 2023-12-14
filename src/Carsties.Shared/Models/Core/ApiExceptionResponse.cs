using Carsties.Shared.Models.Enums;

namespace Carsties.Shared.Models.Core
{
    public class ApiExceptionResponse : ApiResponse
    {
        public string StackTrace { get; set; }
        public ApiExceptionResponse(string errorMessages = "", string stackTrace = "")
            : base(ErrorCodes.InternalServerError)
        {
            ErrorMessage = errorMessages ?? GetDefaultMessage(Code);
            StackTrace = stackTrace;
        }
    }
}
