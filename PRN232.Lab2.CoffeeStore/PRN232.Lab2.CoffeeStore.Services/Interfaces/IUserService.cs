using PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;

namespace PRN232.Lab2.CoffeeStore.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseModel?> GetUserByIdAsync(int id);
        Task<UserResponseModel?> GetUserByEmailAsync(string email);
        Task<UserResponseModel> CreateUserAsync(UserRequestModel userRequest);
        Task<UserResponseModel?> UpdateUserAsync(int id, UserRequestModel userRequest);
        Task<bool> DeleteUserAsync(int id);
        Task<(IEnumerable<UserResponseModel> Users, int TotalCount)> GetUsersAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null);
        Task<string?> AuthenticateAsync(string email, string password);
        Task<string?> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAsync(string refreshToken);
    }
}