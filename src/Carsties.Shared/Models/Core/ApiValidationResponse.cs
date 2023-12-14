using Carsties.Shared.Models.Enums;

namespace Carsties.Shared.Models.Core
{
    public class ApiValidationResponse : ApiResponse
    {
        public ApiValidationResponse(string errorMessages = "")
            : base(ErrorCodes.BadRequest)
        {
            ErrorMessage = errorMessages;
        }

        public List<FieldLevelError> Errors { get; set; }
    }
}
