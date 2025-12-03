namespace MeetingManager.Application.DTOs.MeetingRequest;

public class CreateMeetingRequestDto
{
    public string Notes { get; set; } = default!;
    public int StudentId { get; set; }
    public int SecretaryId { get; set; }
    public int ManagerId { get; set; }
    public int GroupId { get; set; }
}
