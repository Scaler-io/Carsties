using System.Runtime.Serialization;

namespace BiddingService.Models;

public enum BidStatus
{
    [EnumMember(Value = "Accepted")]
    Accepted,
    [EnumMember(Value = "Accepted below reserve")]
    AcceptedBelowReserve,
    [EnumMember(Value = "Too low")]
    TooLow,
    [EnumMember(Value = "Finished")]
    Finished
}
