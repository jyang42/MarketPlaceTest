using MarketPlaceService.Services;
using MarketPlaceService.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService _userService)
        {
            this._userService = _userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO registrationDto)
        {
            var result = await _userService.CreateUserAsync(registrationDto);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User created successfully!" });
            }

            return BadRequest(result.Errors);
        }
    }
}
