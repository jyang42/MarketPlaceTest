using MarketPlaceService.DataAccess;
using MarketPlaceService.DataAccess.Models;
using MarketPlaceService.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace MarketPlaceService.Services
{
    public class UserService:IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> _userManager)
        {
            this._userManager = _userManager;
        }

        public async Task<IdentityResult> CreateUserAsync(UserRegistrationDTO registrationDto)
        {
            var user = new User
            {
                UserName = registrationDto.Username,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                Email = registrationDto.Email
            };

            return await _userManager.CreateAsync(user, registrationDto.Password);
        }
    }


}
