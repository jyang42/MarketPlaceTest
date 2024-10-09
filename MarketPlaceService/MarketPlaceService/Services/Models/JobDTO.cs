namespace MarketPlaceService.Services.Models
{
    public class JobDTO
    {
        public int? Id { get; set; }
        public int? NumberOfBids { get; set; }
        public string? Description { get; set; }
        public string? Requirements { get; set; }
        public Decimal? HighestBid { get; set; }
        public Decimal? LowestBid { get; set; }
        public DateTime? CreatedDate { get; set; }
        
        public DateTime? ExpirationDate { get; set;}
        public Boolean? Expired { get; set; }

        public IEnumerable<BidDTO>? Bids { get; set; }   

        public string? PosterId { get; set; }

        public string? PosterName { get; set; }
    }
}
