namespace MarketPlaceService.DataAccess.Models
{
    public class Bid
    {
        public int Id { get; set; }
        public required string BidUserId { get; set; }
        public required User Bidder { get; set; }
        public required Decimal Ammount { get; set; }
        public required DateTime createdDate { get; set; }
        public required int JobId { get; set;}
        public required Job Job { get; set;}
    }
}
