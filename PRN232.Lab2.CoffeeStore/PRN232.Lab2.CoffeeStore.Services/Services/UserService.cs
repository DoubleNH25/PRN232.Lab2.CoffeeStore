using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.BusinessModels;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;
using PRN232.Lab2.CoffeeStore.Services.Services;
using PRN232.Lab2.CoffeeStore.Repositories.Models;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace PRN232.Lab2.CoffeeStore.Services.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<UserResponseModel?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserResponseModel
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                CreatedDate = user.CreatedDate,
                IsActive = user.IsActive
            };
        }

        public async Task<UserResponseModel?> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            return new UserResponseModel
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                CreatedDate = user.CreatedDate,
                IsActive = user.IsActive
            };
        }

        public async Task<UserResponseModel> CreateUserAsync(UserRequestModel userRequest)
        {
            // Hash the password
            var passwordHash = HashPassword(userRequest.PasswordHash);

            var user = new User
            {
                Email = userRequest.Email,
                PasswordHash = passwordHash,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Role = userRequest.Role,
                CreatedDate = DateTime.Now,
                IsActive = userRequest.IsActive ?? true
            };

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserResponseModel
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                CreatedDate = user.CreatedDate,
                IsActive = user.IsActive
            };
        }

        public async Task<UserResponseModel?> UpdateUserAsync(int id, UserRequestModel userRequest)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return null;

            // Only hash password if it's being updated
            if (!string.IsNullOrEmpty(userRequest.PasswordHash))
            {
                user.PasswordHash = HashPassword(userRequest.PasswordHash);
            }

            user.Email = userRequest.Email;
            user.FirstName = userRequest.FirstName;
            user.LastName = userRequest.LastName;
            user.Role = userRequest.Role;
            user.IsActive = userRequest.IsActive ?? user.IsActive;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserResponseModel
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                CreatedDate = user.CreatedDate,
                IsActive = user.IsActive
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return false;

            _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<UserResponseModel> Users, int TotalCount)> GetUsersAsync(
            string? search = null,
            string? sortBy = null,
            bool ascending = true,
            int page = 1,
            int pageSize = 10,
            string? fields = null)
        {
            // Get all users
            var allUsers = (await _unitOfWork.UserRepository.GetAllAsync()).AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                allUsers = allUsers.Where(u => u.Email.Contains(search) || 
                                             u.FirstName.Contains(search) || 
                                             u.LastName.Contains(search));
            }

            // Get total count before pagination
            var totalCount = allUsers.Count();

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                var sortDirection = ascending ? "asc" : "desc";
                allUsers = allUsers.OrderBy($"{sortBy} {sortDirection}");
            }
            else
            {
                allUsers = allUsers.OrderBy(u => u.UserId);
            }

            // Apply pagination
            var users = allUsers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Map to response models
            var userResponses = users.Select(u => new UserResponseModel
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = u.Role,
                CreatedDate = u.CreatedDate,
                IsActive = u.IsActive
            });

            return (userResponses, totalCount);
        }

        public async Task<string?> AuthenticateAsync(string email, string password)
        {
            var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            // Verify password
            if (!VerifyPassword(password, user.PasswordHash)) return null;

            // Generate JWT token (simplified for this example)
            // In a real implementation, you would use a proper JWT library
            var token = GenerateJwtToken(user);
            return token;
        }

        public async Task<string?> RefreshTokenAsync(string refreshToken)
        {
            var token = await _unitOfWork.RefreshTokenRepository.FirstOrDefaultAsync(t => t.Token == refreshToken && t.Expires > DateTime.Now);
            if (token == null) return null;

            var user = await _unitOfWork.UserRepository.GetByIdAsync(token.UserId);
            if (user == null) return null;

            // Generate new JWT token
            var newToken = GenerateJwtToken(user);
            return newToken;
        }

        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var token = await _unitOfWork.RefreshTokenRepository.FirstOrDefaultAsync(t => t.Token == refreshToken);
            if (token == null) return false;

            token.Revoked = DateTime.Now;
            _unitOfWork.RefreshTokenRepository.Update(token);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                // Convert to hex string to match SQL Server's HASHBYTES output
                return Convert.ToHexString(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("ThisIsASecretKeyForJwtAuthentication12345"); // Same key as in Program.cs
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("Role", user.Role),
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expires in 1 hour
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}