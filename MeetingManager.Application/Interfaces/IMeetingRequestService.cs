using MeetingManager.Application.DTOs.MeetingRequest;
using MeetingManager.Domain.Enums;

namespace MeetingManager.Application.Interfaces;

public interface IMeetingRequestService
{
    Task<MeetingRequestDto?> GetByIdAsync(int id);
    Task<IEnumerable<MeetingRequestDto>> GetAllAsync();
    Task<IEnumerable<MeetingRequestDto>> GetByStatusAsync(MeetingRequestStatus status);
    Task<IEnumerable<MeetingRequestDto>> GetByGroupIdAsync(int groupId);
    Task<IEnumerable<MeetingRequestDto>> GetByManagerIdAsync(int managerId);
    Task<IEnumerable<MeetingRequestDto>> GetBySecretaryIdAsync(int secretaryId);
    Task<MeetingRequestDto> CreateAsync(CreateMeetingRequestDto dto);
    Task<MeetingRequestDto> CreateForSecretaryAsync(int secretaryId, CreateMeetingRequestDto dto);
    Task<bool> UpdateAsync(UpdateMeetingRequestDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateStatusAsync(int id, MeetingRequestStatus status);
}
