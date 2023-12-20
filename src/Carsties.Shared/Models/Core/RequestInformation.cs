namespace Carsties.Shared.Models.Core
{
    public class RequestInformation
    {
        public UserDto CurrentUser { get; set; }
        public string CorrelationId { get; set; }
    }
}
