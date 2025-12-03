using MeetingManager.Domain.Enums;

namespace MeetingManager.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = default!;
    public int UserId { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public UserRole Role { get; set; }
    public int GroupId { get; set; }
}
