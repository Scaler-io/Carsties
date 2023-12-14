namespace AuctionService.Models.DTOs;

public class AuctionDto
{
    public Guid Id { get; set; }
    public int ReservePrice { get; set; }
    public string Seller { get; set; }
    public string Winner { get; set; }
    public int SoldAmount { get; set; }
    public int CurrentHighBid { get; set; }
    public string Status { get; set; }
    public DateTime AuctionEnd { get; set; }
    public ItemDto Item { get; set; }
    public MetaDataDto MetaData { get; set; }
}
