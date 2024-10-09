using MarketPlaceService.DataAccess;
using MarketPlaceService.DataAccess.Models;
using MarketPlaceService.Services.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MarketPlaceService.Services
{
    public class JobService:IJobService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;


        public JobService(ApplicationDbContext _dbContext, UserManager<User> _userManager)
        {
            this._dbContext = _dbContext;
            this._userManager = _userManager;
        }

        private int GetBid_Count(IEnumerable<Bid>? bids)
        {
            if (bids == null || !bids.Any())
            {
                return 0;
            }
            return bids.Count();
        }


        private Decimal GetHighestBid(IEnumerable<Bid>? bids)
        {
            if (bids == null || !bids.Any())
            {
                return 0;
            }

            return bids.Select(x => x.Ammount).Max(y => y);
        }


        private Decimal GetLowestBid(IEnumerable<Bid>? bids)
        {
            if (bids == null || !bids.Any())
            {
                return 0;
            }

            return bids.Select(x => x.Ammount).Min(y => y);
        }


        

        public async Task<JobDTO> CreateJob(CreateJobDTO createJobDTO)
        {
            try
            {
               
                var user = this._userManager.Users.Where(x => x.Id == createJobDTO.PosterId).FirstOrDefault();

                if (user == null)
                {
                    throw new Exception("User cannot be null");
                }

                var job = new Job
                {
                    PosterId = user.Id,
                    Poster = user,
                    Description = createJobDTO.Description,
                    Requirements = createJobDTO.Requirements,
                    ExpirationDate = createJobDTO.ExpirationDate,
                    CreatedDate = DateTime.Now
                };

                var result = this._dbContext.Jobs.Add(job);
                await this._dbContext.SaveChangesAsync();

                return new JobDTO
                {
                    Id = job.Id,
                    Description = job.Description,
                    Requirements = job.Requirements,
                    ExpirationDate = job.ExpirationDate,
                    CreatedDate = job.CreatedDate,
                    PosterId = job.Poster.Id,
                    PosterName = job.Poster.FirstName + " " + job.Poster.LastName
                };

            } catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<IEnumerable<JobDTO>> GetActiveJobs(int? page, int? pagesize)
        {
            int p = page ?? 1;
            int ps = pagesize ?? 5;

            var a_jobs = await this._dbContext.Jobs.Where(x => x.ExpirationDate > DateTime.Now).ToListAsync();

            if (a_jobs.Count == 0) { 
            
                return Enumerable.Empty<JobDTO>();
            }

            var active_job_ids = a_jobs.Select(x => new
            {
                job_id = x.Id,
                bid_count = GetBid_Count(x.Bids),
            }).OrderByDescending(x => x.bid_count).Skip((p - 1) * ps).Take(ps).Select(x => x.job_id);

            var jobs = await this._dbContext.Jobs.Where(x => active_job_ids.Contains(x.Id)).ToListAsync();

            List<JobDTO> results = new List<JobDTO>();

            foreach (var j in jobs)

            {

                var poster = this._userManager.Users.Where(x => x.Id == j.PosterId).FirstOrDefault();
                var bids = this._dbContext.Bids.Where(x => x.JobId == j.Id).ToList();
                var dto = new JobDTO()
                {
                    Id = j.Id,
                    PosterId = j.PosterId,
                    PosterName = poster.FirstName + " " + poster.LastName, 
                    HighestBid = this.GetHighestBid(bids),
                    LowestBid = this.GetLowestBid(bids),
                    CreatedDate = j.CreatedDate,
                    ExpirationDate = j.ExpirationDate,
                    Description = j.Description,
                    Requirements = j.Requirements,
                    NumberOfBids = this.GetBid_Count(bids)
                };

                results.Add(dto);
            }
            
            return results;
        }

        public async Task<IEnumerable<JobDTO>> GetRecentJobs(int? page, int? pagesize)
        {
            int p = page ?? 1;
            int ps = pagesize ?? 5;

            var most_recent_jobs = await this._dbContext.Jobs.Where(x=> x.ExpirationDate > DateTime.Now).OrderByDescending(x => x.CreatedDate).Skip((p - 1) * ps).Take(ps).ToListAsync();
            List<JobDTO> results = new List<JobDTO>();

            foreach (var j in most_recent_jobs)
            {
                var poster = this._userManager.Users.Where(x => x.Id == j.PosterId).FirstOrDefault();
                var bids = this._dbContext.Bids.Where(x => x.JobId == j.Id).ToList();
                var dto = new JobDTO()
                {
                    Id = j.Id,
                    PosterId = j.PosterId,
                    PosterName = poster.FirstName + " " + poster.LastName,
                    HighestBid = this.GetHighestBid(bids),
                    LowestBid = this.GetLowestBid(bids),
                    CreatedDate = j.CreatedDate,
                    ExpirationDate = j.ExpirationDate,
                    Description = j.Description,
                    Requirements = j.Requirements,
                    NumberOfBids = this.GetBid_Count(bids)
                };

                results.Add(dto);
            }

            return results;
        }

        public async Task<JobDTO> GetJob(int jobId)
        {
            try
            {
                var j = await this._dbContext.Jobs.Where(x => x.Id == jobId).FirstOrDefaultAsync();
                
                var poster = this._userManager.Users.Where(x => x.Id == j.PosterId).FirstOrDefault();

                if(poster == null)
                {
                    throw new Exception("job poster cannot be null");
                }

                var bids = this._dbContext.Bids.Where(x => x.JobId == jobId).ToList();

                var bidsDTOs = new List<BidDTO>();


                foreach (var b in bids)
                {
                    var bidder = this._userManager.Users.Where(x => x.Id == b.BidUserId).FirstOrDefault();
                    if(bidder == null)
                    {
                        throw new Exception("bidder cannot be null");
                    }
                    var dto = new BidDTO()
                    {
                        Id = b.Id,
                        JobId = jobId,
                        BidderName = bidder.FirstName + " " + bidder.LastName,
                        Ammount = b.Ammount,
                        CreatedDate = b.createdDate
                    };
                    bidsDTOs.Add(dto);
                }

                return new JobDTO()
                {
                    Id = j.Id,
                    PosterId = j.PosterId,
                    PosterName = poster.FirstName + " " + poster.LastName,
                    HighestBid = this.GetHighestBid(bids),
                    LowestBid = this.GetLowestBid(bids),
                    CreatedDate = j.CreatedDate,
                    ExpirationDate = j.ExpirationDate,
                    Description = j.Description,
                    Requirements = j.Requirements,
                    NumberOfBids = this.GetBid_Count(bids),
                    Bids = bidsDTOs
                };
            }
            catch(Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<BidDTO> CreateBid(CreateBidDTO createBidDTO)
        {
            try
            {
                var user = this._userManager.Users.Where(x => x.Id == createBidDTO.BidUserId).FirstOrDefault();
                if (user == null) {
                    throw new Exception("user does not exist");
                }

                var job = this._dbContext.Jobs.Where(x => x.Id == createBidDTO.JobId).FirstOrDefault();

                if (job == null) {
                    throw new Exception("job does not exist"); 
                }

                var bid = new Bid()
                {
                    Ammount = createBidDTO.Ammount ?? 0,
                    BidUserId = user.Id,
                    Bidder = user,
                    createdDate = DateTime.Now,
                    JobId = createBidDTO.JobId,
                    Job = job
                };

                this._dbContext.Bids.Add(bid);

                await this._dbContext.SaveChangesAsync();

                return new BidDTO
                {
                    Id = bid.Id,
                    CreatedDate = bid.createdDate,
                    BidderName = bid.Bidder.FirstName + " " + bid.Bidder.LastName,
                    Ammount = bid.Ammount,
                    JobId = bid.JobId
                };
            }
            catch (Exception e) {
                throw new Exception("cannot save bid", e);
            }
           
        }

        public async Task<BidDTO> GetBid(int bidId)
        {
            try
            {
                var bid = await this._dbContext.Bids.Where(x => x.Id == bidId).FirstOrDefaultAsync();
                if (bid == null) {
                    return new BidDTO();
                }

                var user = this._userManager.Users.Where(x => x.Id == bid.BidUserId).FirstOrDefault();

                var result = new BidDTO()
                {
                    Id = bid.Id,
                    CreatedDate = bid.createdDate,
                    Ammount = bid.Ammount,
                    BidderName = user.FirstName + " " + user.LastName,
                    JobId = bid.JobId
                };

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("cannot find bid", e);
            }
        }

        public async Task<IEnumerable<BidDTO>> GetBidsFromJob(int jobId)
        {
            try
            {
                var bids = await this._dbContext.Bids.Where(x => x.JobId == jobId).ToListAsync();

                var results = new List<BidDTO>();

                foreach (var b in bids)
                {
                    var user = this._userManager.Users.Where(x => x.Id == b.BidUserId).FirstOrDefault();
                    if (user == null)
                    {
                        throw new NullReferenceException("Cannot find Bidder");
                    }
                    results.Add(new BidDTO() { 
                        Id = b.Id,
                        CreatedDate = b.createdDate,
                        Ammount = b.Ammount,
                        BidderName = user.FirstName + ' ' + user.LastName,
                        JobId = b.JobId
                    });

                }
                
                return results;

            } catch (Exception e) {
                throw new Exception("Cannot find bids", e);
            } 
            
        }
    }
}
