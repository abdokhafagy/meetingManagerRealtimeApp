using MeetingManager.Domain.Enums;

namespace MeetingManager.Application.DTOs.MeetingRequest;

public class MeetingRequestDto
{
    public int Id { get; set; }
    public string Notes { get; set; } = default!;
    public int StudentId { get; set; }
    public string StudentName { get; set; } = default!;
    public int SecretaryId { get; set; }
    public string SecretaryName { get; set; } = default!;
    public int ManagerId { get; set; }
    public string ManagerName { get; set; } = default!;
    public int GroupId { get; set; }
    public string GroupName { get; set; } = default!;
    public MeetingRequestStatus Status { get; set; }
    public string StatusText { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
