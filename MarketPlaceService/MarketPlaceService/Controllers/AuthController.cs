using MarketPlaceService.DataAccess.Models;
using MarketPlaceService.Services.Models;
using MarketPlaceService.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace MarketPlaceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public AuthController(IConfiguration _configuration, UserManager<User> _userManager)
        {
            this._configuration = _configuration;
            this._userManager = _userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await this.GenerateTokenAsync(loginDto.Username, loginDto.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new {token});
        }

        
        [HttpGet("verify")]
        public IActionResult verify()
        {
            try
            {

                var header = Request.Headers;

                var token = header.Where(x => x.Key == "Authorization").Select(y => y.Value).FirstOrDefault().ToString();

                var claims = TokVal.ValidateJwt(token);

                String? id = TokVal.GetIdFromClaim(claims);

                if (id != null)
                {
                    return Ok();
                }

                return Unauthorized();

            } catch(Exception e)
            {
                Console.WriteLine(e);
                return Unauthorized();
            }
        }


        private async Task<string> GenerateTokenAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new Exception("invalid token"); // Invalid username or password
            }

            

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-long-sercret-for-testing-because-it-needs-to-be-this-is-dumb"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(5),
                Issuer = "Iamanissuer",
                Audience = "Iamanaudience",
                SigningCredentials = creds
            }; ;

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}