using MarketPlaceService.DataAccess.Models;

namespace MarketPlaceService.Services.Models
{
    public class CreateJobDTO
    {
        public string? Description { get; set; }

        public string? Requirements { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? PosterId { get; set; }
    }
}
