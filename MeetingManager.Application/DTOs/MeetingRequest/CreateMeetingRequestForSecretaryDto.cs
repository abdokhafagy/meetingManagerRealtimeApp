namespace MeetingManager.Application.DTOs.MeetingRequest;

public class CreateMeetingRequestForSecretaryDto
{
    public string Notes { get; set; } = default!;
    public int StudentId { get; set; }
    public int ManagerId { get; set; }
}
