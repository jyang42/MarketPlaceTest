using MarketPlaceService.DataAccess.Models;

namespace MarketPlaceService.Services.Models
{
    public class CreateBidDTO
    {
        public String? BidUserId { get; set; }
        public Decimal? Ammount { get; set; }
        public required int JobId { get; set; }
    }
}
