using auth_service.contracts;
using auth_service.Models;
using auth_service.Models.DTOs;
using auth_service.repository;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace auth_service.service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly authContext _context;
        private readonly JWTsettings _jwtsettings;
        public AuthService(IConfiguration configuration,authContext authContext,JWTsettings settings)
        {
            _config = configuration;
            _context = authContext;
            _jwtsettings = settings;
        }
        public async Task<AuthResponse> LoginAsync(Login request)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid username or password");
            }
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _context.tokens
                  .Include(x => x.user)
                  .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired refresh token");
            }
            var user = await _context.users.FindAsync(storedToken.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            storedToken.IsRevoked = true;
            _context.tokens.Update(storedToken);
            await _context.SaveChangesAsync();
            return await GenerateAuthResponseAsync(user);
        }

        public async Task<AuthResponse> RegisterAsync(Registration request)
        {
            var _user = await _context.users.AnyAsync(u => u.Username == request.Username || u.Email == request.Email);
            if (_user)
            {
                throw new Exception("User with this username or email already exists");
            }
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            try
            {
                _context.users.Add(user);
                await _context.SaveChangesAsync();
                return await GenerateAuthResponseAsync(user);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task RevokeTokenAsync(string refreshToken)
        {
            var storedToken = await _context.tokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null)
            {
                throw new Exception("Token not found");
            }

            storedToken.IsRevoked = true;
            _context.tokens.Update(storedToken);
            await _context.SaveChangesAsync();
        }
        private async Task<AuthResponse> GenerateAuthResponseAsync(User user)
        {
            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            _context.tokens.Add(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtsettings.RefreshTokenLifetimeDays),
                IsRevoked = false
            });

            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Token = jwtToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtsettings.TokenLifetimeMinutes),
                Username = user.Username
            };
        }
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtsettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtsettings.TokenLifetimeMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtsettings.Issuer,
                Audience = _jwtsettings.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
