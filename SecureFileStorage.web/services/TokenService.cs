using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using SecureFileStorage.Core.Entities;
using SecureFileStorage.Core.Interfaces;
using SecureFileStorage.Web.Data;

namespace SecureFileStorage.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthSettings _authSettings;
        private readonly IUserRepository _userRepository;
        private readonly IJSRuntime JSRuntime;
        public TokenService(IOptions<AuthSettings> authSettings, IJSRuntime JSRuntime, IUserRepository userRepository)
        {
            _authSettings = authSettings.Value;
            this.JSRuntime = JSRuntime;
            _userRepository = userRepository;
        }

        public string GenerateToken(int userId)
        {
            if (_authSettings == null)
            {
                throw new InvalidOperationException("AuthSettings is not initialized.");
            }

            if (string.IsNullOrEmpty(_authSettings.SecretKey))
            {
                throw new InvalidOperationException("SecretKey is not configured.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userId.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(_authSettings.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<User?> GetLoggedInUser(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<int> GetLoggedInUserIdAsync()
        {
            if (JSRuntime == null) throw new InvalidOperationException("JSRuntime is not initialized.");

            var token = await JSRuntime.InvokeAsync<string>("localStorage.getItem", new object[] { "authToken" });

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Token not found.");
            }

            var userId = GetUserIdFromToken(token);

            if (userId == null)
            {
                throw new InvalidOperationException("User ID claim not found in token.");
            }

            return int.Parse(userId);
        }

        public async Task<bool> UserIsAdmin(int id)
        {
            return await _userRepository.UserIsAdminAsync(id);
        }

        private string? GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id");

            return userIdClaim?.Value;
        }
    }
}