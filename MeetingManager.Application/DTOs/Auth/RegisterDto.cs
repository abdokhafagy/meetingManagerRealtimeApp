using MeetingManager.Domain.Enums;

namespace MeetingManager.Application.DTOs.Auth;

public class RegisterDto
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public UserRole Role { get; set; }
    public int GroupId { get; set; }
}
