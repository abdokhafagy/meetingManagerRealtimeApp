using MeetingManager.Application.DTOs.Auth;

namespace MeetingManager.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
    Task<bool> ValidateTokenAsync(string token);
}
