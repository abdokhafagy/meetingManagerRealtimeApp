using MeetingManager.Domain.Enums;

namespace MeetingManager.Application.DTOs.MeetingRequest;

public class UpdateMeetingRequestDto
{
    public int Id { get; set; }
    public string Notes { get; set; } = default!;
    public int StudentId { get; set; }
    public int SecretaryId { get; set; }
    public int ManagerId { get; set; }
    public int GroupId { get; set; }
    public MeetingRequestStatus Status { get; set; }
}
