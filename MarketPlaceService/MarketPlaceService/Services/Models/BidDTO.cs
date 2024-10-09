namespace MarketPlaceService.Services.Models
{
    public class BidDTO
    {
        public int? Id { get; set; }
        public decimal? Ammount { get; set; }
        public string? BidderName { get; set; }
        public int? JobId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
