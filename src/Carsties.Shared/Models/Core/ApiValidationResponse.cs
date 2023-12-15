using Carsties.Shared.Models.Enums;

namespace Carsties.Shared.Models.Core
{
    public sealed class ApiValidationResponse : ApiResponse
    {
        public ApiValidationResponse(string errorMessages = "")
            : base(ErrorCodes.BadRequest)
        {
            ErrorMessage = !string.IsNullOrEmpty(errorMessages) ? errorMessages : GetDefaultMessage(Code);
        }

        public List<FieldLevelError> Errors { get; set; }

        protected override string GetDefaultMessage(ErrorCodes code)
        {
            return "Invalid data provided.";
        }
    }
}
