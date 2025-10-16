using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;
using System.Text.Json;

namespace PRN232.Lab2.CoffeeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticate user and return JWT token
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequestModel loginRequest)
        {
            var token = await _userService.AuthenticateAsync(loginRequest.Email, loginRequest.Password);
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(new { token });
        }

        /// <summary>
        /// Refresh JWT token
        /// </summary>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> RefreshToken([FromBody] RefreshTokenRequestModel refreshTokenRequest)
        {
            var token = await _userService.RefreshTokenAsync(refreshTokenRequest.RefreshToken);
            if (token == null)
            {
                return Unauthorized(new { message = "Invalid refresh token" });
            }

            return Ok(new { token });
        }

        /// <summary>
        /// Logout user
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<bool>> Logout([FromBody] RefreshTokenRequestModel refreshTokenRequest)
        {
            var result = await _userService.LogoutAsync(refreshTokenRequest.RefreshToken);
            return Ok(result);
        }

        /// <summary>
        /// Get all users with optional search, sort, paging, and field selection
        /// </summary>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")] // Only admins can get all users
        public async Task<ActionResult<IEnumerable<UserResponseModel>>> GetUsers(
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? fields = null)
        {
            var (users, totalCount) = await _userService.GetUsersAsync(search, sortBy, ascending, page, pageSize, fields);

            // Add pagination headers
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            }));

            // Apply field selection if specified
            if (!string.IsNullOrEmpty(fields))
            {
                var selectedUsers = SelectFields(users, fields);
                return Ok(selectedUsers);
            }

            return Ok(users);
        }

        /// <summary>
        /// Get a specific user by ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize] // Users can get their own info, admins can get any user info
        public async Task<ActionResult<UserResponseModel>> GetUser(int id)
        {
            // Check if user is requesting their own info or is admin
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserId != id && currentUserRole != "Admin")
            {
                return Forbid();
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")] // Only admins can create users
        public async Task<ActionResult<UserResponseModel>> CreateUser(UserRequestModel userRequest)
        {
            try
            {
                var user = await _userService.CreateUserAsync(userRequest);
                return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        [HttpPut("{id}")]
        [Authorize] // Users can update their own info, admins can update any user info
        public async Task<ActionResult<UserResponseModel>> UpdateUser(int id, UserRequestModel userRequest)
        {
            // Check if user is updating their own info or is admin
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserId != id && currentUserRole != "Admin")
            {
                return Forbid();
            }

            try
            {
                var user = await _userService.UpdateUserAsync(id, userRequest);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")] // Only admins can delete users
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }

        private string GetCurrentUserRole()
        {
            var roleClaim = User.Claims.FirstOrDefault(c => c.Type == "Role");
            return roleClaim?.Value ?? "Staff";
        }

        private IEnumerable<object> SelectFields(IEnumerable<UserResponseModel> users, string fields)
        {
            var fieldList = fields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                  .Select(f => f.Trim().ToLower())
                                  .ToList();

            foreach (var user in users)
            {
                var obj = new Dictionary<string, object>();

                if (fieldList.Contains("userid") || fieldList.Contains("id"))
                    obj["userId"] = user.UserId;

                if (fieldList.Contains("email"))
                    obj["email"] = user.Email;

                if (fieldList.Contains("firstname"))
                    obj["firstName"] = user.FirstName;

                if (fieldList.Contains("lastname"))
                    obj["lastName"] = user.LastName;

                if (fieldList.Contains("role"))
                    obj["role"] = user.Role;

                if (fieldList.Contains("createddate"))
                    obj["createdDate"] = user.CreatedDate;

                if (fieldList.Contains("isactive"))
                    obj["isActive"] = user.IsActive;

                yield return obj;
            }
        }
    }

    public class LoginRequestModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RefreshTokenRequestModel
    {
        public string RefreshToken { get; set; } = null!;
    }
}