using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseModel> RegisterAsync(RegisterRequestModel request);
    Task<AuthResponseModel> LoginAsync(LoginRequestModel request);
    Task<AuthResponseModel> RefreshTokenAsync(RefreshTokenRequestModel request);
    Task LogoutAsync(string userId, string refreshToken);
}
