using MarketPlaceService.Services.Models;
using Microsoft.AspNetCore.Identity;
namespace MarketPlaceService.Services
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(UserRegistrationDTO registrationDto);
    }
}
