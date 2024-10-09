using MarketPlaceService.Services;
using MarketPlaceService.Services.Models;
using MarketPlaceService.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketPlaceService.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        IJobService _jobService;

        public JobController(IJobService _jobService) {
            this._jobService = _jobService;
        }


        [HttpGet("GetBids/{id}")]
        public async Task<IActionResult> GetBids(int id)
        {
            try
            {
                var header = Request.Headers;

                var token = header.Where(x => x.Key == "Authorization").Select(y => y.Value).FirstOrDefault().ToString();

                var claims = TokVal.ValidateJwt(token);

                var u = TokVal.GetIdFromClaim(claims);


                if (u == null)
                {
                    return Unauthorized();
                }

                var results = await this._jobService.GetBidsFromJob(id);
                return Ok(results);
            }
            catch (Exception ex) {

                return BadRequest(ex.Message);
            
            }

        }

        [HttpPost("CreateBid")]
        public async Task<IActionResult> CreateBid([FromBody] CreateBidDTO createBidDTO)
        {
            try
            {
                var header = Request.Headers;

                var token = header.Where(x => x.Key == "Authorization").Select(y => y.Value).FirstOrDefault().ToString();

                var claims = TokVal.ValidateJwt(token);

                var id = TokVal.GetIdFromClaim(claims);


                if (id == null)
                {
                    return Unauthorized();
                }


                if (createBidDTO.Ammount < 0) {
                    throw new Exception("malformed request");
                }

                createBidDTO.BidUserId = id;

                var result = await this._jobService.CreateBid(createBidDTO);

                return Ok(result);

            }
            catch (Exception ex) {

                return BadRequest(ex.Message);
            }
            
        }

        
        [HttpPost("CreateJob")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobDTO createJobDTO)
        {
            try
            {

                var header = Request.Headers;

                var token = header.Where(x => x.Key == "Authorization").Select(y => y.Value).FirstOrDefault().ToString();

                var claims = TokVal.ValidateJwt(token);

                var id = TokVal.GetIdFromClaim(claims);

                if (id == null)
                {
                    return Unauthorized();
                }

                createJobDTO.PosterId = id;

                var result = await this._jobService.CreateJob(createJobDTO);

                return Ok(result);

            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob(int id)
        {
            try
            {
                var header = Request.Headers;

                var token = header.Where(x => x.Key == "Authorization").Select(y => y.Value).FirstOrDefault().ToString();

                var claims = TokVal.ValidateJwt(token);

                var u = TokVal.GetIdFromClaim(claims);

                if (u == null)
                {
                    return Unauthorized();
                }

                var job = await this._jobService.GetJob(id);
                return Ok(job);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }

           
        
        }


            [HttpGet("ActiveJobs")]
        public async Task<IActionResult> GetActiveJobs(int? page = 1, int? pageSize = 5)
        {
            try
            {
                var header = Request.Headers;

                var token = header.Where(x => x.Key == "Authorization").Select(y => y.Value).FirstOrDefault().ToString();

                var claims = TokVal.ValidateJwt(token);

                var u = TokVal.GetIdFromClaim(claims);

                if (u == null)
                {
                    return Unauthorized();
                }



                if (page <= 0) page = 1; 
                if (pageSize <= 0) pageSize = 5;  
                var results = await this._jobService.GetActiveJobs(page,pageSize);
                return Ok(results);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
            
        }
        [HttpGet("RecentJobs")]
        public async Task<IActionResult> GetRecentJobs(int? page = 1, int? pageSize = 5)
        {
            try
            {
                var header = Request.Headers;

                var token = header.Where(x => x.Key == "Authorization").Select(y => y.Value).FirstOrDefault().ToString();

                var claims = TokVal.ValidateJwt(token);

                var u = TokVal.GetIdFromClaim(claims);

                if (u == null)
                {
                    return Unauthorized();
                }



                if (page <= 0) page = 1;
                if (pageSize <= 0) pageSize = 5;
                var results = await this._jobService.GetRecentJobs(page,pageSize);
                return Ok(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }

        }
    }
}
