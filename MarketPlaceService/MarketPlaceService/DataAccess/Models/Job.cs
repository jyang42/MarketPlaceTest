namespace MarketPlaceService.DataAccess.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        public string? Requirements { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public required string PosterId { get; set; }
        public required User Poster { get; set;}
        public ICollection<Bid>? Bids { get; set;}

    }
}
