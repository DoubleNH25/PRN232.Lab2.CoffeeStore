using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PRN232.Lab2.CoffeeStore.Repositories.Entities;
using PRN232.Lab2.CoffeeStore.Repositories.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Configurations;
using PRN232.Lab2.CoffeeStore.Services.Interfaces;
using PRN232.Lab2.CoffeeStore.Services.Models.Responses;
using PRN232.Lab2.CoffeeStore.Services.Models.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PRN232.Lab2.CoffeeStore.Services.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUnitOfWork unitOfWork,
        IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<AuthResponseModel> RegisterAsync(RegisterRequestModel request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponseModel
            {
                Succeeded = false,
                Message = "Email already registered"
            };
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            EmailConfirmed = true,
            CreatedDate = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return new AuthResponseModel
            {
                Succeeded = false,
                Message = string.Join("; ", result.Errors.Select(error => error.Description))
            };
        }

        return await GenerateTokenResponseAsync(user);
    }

    public async Task<AuthResponseModel> LoginAsync(LoginRequestModel request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new AuthResponseModel
            {
                Succeeded = false,
                Message = "Invalid credentials"
            };
        }

        var passwordValid = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!passwordValid.Succeeded)
        {
            return new AuthResponseModel
            {
                Succeeded = false,
                Message = "Invalid credentials"
            };
        }

        return await GenerateTokenResponseAsync(user);
    }

    public async Task<AuthResponseModel> RefreshTokenAsync(RefreshTokenRequestModel request)
    {
        var principal = GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
        {
            return new AuthResponseModel
            {
                Succeeded = false,
                Message = "Invalid access token"
            };
        }

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return new AuthResponseModel
            {
                Succeeded = false,
                Message = "Invalid access token"
            };
        }

        var tokenEntity = await _unitOfWork.RefreshTokens.Query()
            .FirstOrDefaultAsync(token => token.Token == request.RefreshToken && token.UserId == userId);
        if (tokenEntity == null || tokenEntity.IsRevoked || tokenEntity.ExpiryDate < DateTime.UtcNow)
        {
            return new AuthResponseModel
            {
                Succeeded = false,
                Message = "Invalid refresh token"
            };
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new AuthResponseModel
            {
                Succeeded = false,
                Message = "User not found"
            };
        }

        tokenEntity.IsRevoked = true;
        tokenEntity.UpdatedDate = DateTime.UtcNow;
        _unitOfWork.RefreshTokens.Update(tokenEntity);
        await _unitOfWork.SaveChangesAsync();

        return await GenerateTokenResponseAsync(user);
    }

    public async Task LogoutAsync(string userId, string refreshToken)
    {
        var tokenEntity = await _unitOfWork.RefreshTokens.Query()
            .FirstOrDefaultAsync(token => token.Token == refreshToken && token.UserId == userId);
        if (tokenEntity != null)
        {
            tokenEntity.IsRevoked = true;
            tokenEntity.UpdatedDate = DateTime.UtcNow;
            _unitOfWork.RefreshTokens.Update(tokenEntity);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    private async Task<AuthResponseModel> GenerateTokenResponseAsync(ApplicationUser user)
    {
        var accessToken = GenerateJwtToken(user);
        var refreshToken = await CreateRefreshTokenAsync(user);

        return new AuthResponseModel
        {
            Succeeded = true,
            Message = "Authenticated",
            AccessToken = accessToken.Token,
            RefreshToken = refreshToken.Token,
            AccessTokenExpiration = accessToken.Expiration
        };
    }

    private (string Token, DateTime Expiration) GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.FullName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    private async Task<RefreshToken> CreateRefreshTokenAsync(ApplicationUser user)
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(tokenBytes);

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiryDate = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays),
            CreatedDate = DateTime.UtcNow,
            IsRevoked = false
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);
        await _unitOfWork.SaveChangesAsync();

        return refreshTokenEntity;
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = _jwtOptions.Audience,
            ValidateIssuer = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
