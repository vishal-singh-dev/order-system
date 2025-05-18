using auth_service.Models;
using auth_service.Models.DTOs;
using Microsoft.AspNetCore.Identity.Data;

namespace auth_service.contracts
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(Registration request);
        Task<AuthResponse> LoginAsync(Login request);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task RevokeTokenAsync(string refreshToken);
    }
}
