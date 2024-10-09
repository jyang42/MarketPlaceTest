using MarketPlaceService.Services.Models;

namespace MarketPlaceService.Services
{
    public interface IJobService
    {
        public Task<IEnumerable<JobDTO>> GetActiveJobs(int? pagesize, int? page);

        public Task<IEnumerable<JobDTO>> GetRecentJobs(int? pagesize, int? page);

        public Task<JobDTO> CreateJob(CreateJobDTO createjobDTO);

        public Task<JobDTO> GetJob(int jobId);

        public Task<BidDTO> CreateBid(CreateBidDTO createBidDTO);

        public Task<BidDTO> GetBid(int bidId);

        public Task<IEnumerable<BidDTO>> GetBidsFromJob(int jobId);

    }
}
