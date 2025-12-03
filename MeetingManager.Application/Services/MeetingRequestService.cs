using MeetingManager.Application.DTOs.MeetingRequest;
using MeetingManager.Application.Interfaces;
using MeetingManager.Domain.Enums;
using MeetingManager.Domain.Interfaces;

namespace MeetingManager.Application.Services;

public class MeetingRequestService : IMeetingRequestService
{
    private readonly IMeetingRequestRepository _meetingRequestRepository;
    private readonly INotificationService _notificationService;

    public MeetingRequestService(
        IMeetingRequestRepository meetingRequestRepository,
        INotificationService notificationService)
    {
        _meetingRequestRepository = meetingRequestRepository;
        _notificationService = notificationService;
    }

    public async Task<MeetingRequestDto?> GetByIdAsync(int id)
    {
        var meetingRequest = await _meetingRequestRepository.GetByIdWithDetailsAsync(id);
        
        if (meetingRequest == null)
            return null;

        return MapToDto(meetingRequest);
    }

    public async Task<IEnumerable<MeetingRequestDto>> GetAllAsync()
    {
        var meetingRequests = await _meetingRequestRepository.GetAllWithDetailsAsync();
        return meetingRequests.Select(MapToDto);
    }

    public async Task<IEnumerable<MeetingRequestDto>> GetByStatusAsync(MeetingRequestStatus status)
    {
        var meetingRequests = await _meetingRequestRepository.GetByStatusAsync(status);
        return meetingRequests.Select(MapToDto);
    }

    public async Task<IEnumerable<MeetingRequestDto>> GetByGroupIdAsync(int groupId)
    {
        var meetingRequests = await _meetingRequestRepository.GetByGroupIdAsync(groupId);
        return meetingRequests.Select(MapToDto);
    }

    public async Task<IEnumerable<MeetingRequestDto>> GetByManagerIdAsync(int managerId)
    {
        var meetingRequests = await _meetingRequestRepository.GetByManagerIdAsync(managerId);
        return meetingRequests.Select(MapToDto);
    }

    public async Task<IEnumerable<MeetingRequestDto>> GetBySecretaryIdAsync(int secretaryId)
    {
        var meetingRequests = await _meetingRequestRepository.GetBySecretaryIdAsync(secretaryId);
        return meetingRequests.Select(MapToDto);
    }

    public async Task<MeetingRequestDto> CreateAsync(CreateMeetingRequestDto dto)
    {
        var meetingRequest = new Domain.Entities.MeetingRequest
        {
            Notes = dto.Notes,
            StudentId = dto.StudentId,
            SecretaryId = dto.SecretaryId,
            ManagerId = dto.ManagerId,
            GroupId = dto.GroupId,
            Status = MeetingRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _meetingRequestRepository.AddAsync(meetingRequest);
        var result = await _meetingRequestRepository.GetByIdWithDetailsAsync(created.Id);
        
        // Notify Manager
        if (result?.Student != null)
        {
            var MeetingRequestDto = MapToDto(result!);
            await _notificationService.NotifyManagerOfNewRequestAsync(result.ManagerId, MeetingRequestDto);
            return MeetingRequestDto;
        }

        return MapToDto(result!);
    }

    public async Task<MeetingRequestDto> CreateForSecretaryAsync(int secretaryId, CreateMeetingRequestDto dto)
    {
        // This method is called when a secretary creates a meeting request
        // It automatically assigns the manager from the secretary's group
        var meetingRequest = new Domain.Entities.MeetingRequest
        {
            Notes = dto.Notes,
            StudentId = dto.StudentId,
            SecretaryId = secretaryId,
            ManagerId = dto.ManagerId,  // Manager should be from the same group
            GroupId = dto.GroupId,
            Status = MeetingRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _meetingRequestRepository.AddAsync(meetingRequest);
        var result = await _meetingRequestRepository.GetByIdWithDetailsAsync(created.Id);
        
        // Notify Manager
        if (result?.Student != null)
        {
            var MeetingRequestDto = MapToDto(result!);
            await _notificationService.NotifyManagerOfNewRequestAsync(result.ManagerId, MeetingRequestDto);
            return MeetingRequestDto;
        }
        
        return MapToDto(result!);
    }

    public async Task<bool> UpdateAsync(UpdateMeetingRequestDto dto)
    {
        var meetingRequest = await _meetingRequestRepository.GetByIdAsync(dto.Id);
        
        if (meetingRequest == null)
            return false;

        meetingRequest.Notes = dto.Notes;
        meetingRequest.StudentId = dto.StudentId;
        meetingRequest.SecretaryId = dto.SecretaryId;
        meetingRequest.ManagerId = dto.ManagerId;
        meetingRequest.GroupId = dto.GroupId;
        meetingRequest.Status = dto.Status;
        meetingRequest.UpdatedAt = DateTime.UtcNow;

        await _meetingRequestRepository.UpdateAsync(meetingRequest);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (!await _meetingRequestRepository.ExistsAsync(id))
            return false;

        await _meetingRequestRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> UpdateStatusAsync(int id, MeetingRequestStatus status)
    {
        var meetingRequest = await _meetingRequestRepository.GetByIdAsync(id);
        
        if (meetingRequest == null)
            return false;

        meetingRequest.Status = status;
        meetingRequest.UpdatedAt = DateTime.UtcNow;

        await _meetingRequestRepository.UpdateAsync(meetingRequest);

        // Notify Secretary
        await _notificationService.NotifySecretaryOfStatusChangeAsync(
            meetingRequest.SecretaryId, 
            meetingRequest.Id, 
            status.ToString());

        return true;
    }

    private MeetingRequestDto MapToDto(Domain.Entities.MeetingRequest meetingRequest)
    {
        return new MeetingRequestDto
        {
            Id = meetingRequest.Id,
            Notes = meetingRequest.Notes,
            StudentId = meetingRequest.StudentId,
            StudentName = meetingRequest.Student?.FullName ?? "N/A",
            SecretaryId = meetingRequest.SecretaryId,
            SecretaryName = meetingRequest.Secretary?.FullName ?? "N/A",
            ManagerId = meetingRequest.ManagerId,
            ManagerName = meetingRequest.Manager?.FullName ?? "N/A",
            GroupId = meetingRequest.GroupId,
            GroupName = meetingRequest.Group?.Name ?? "N/A",
            Status = meetingRequest.Status,
            StatusText = meetingRequest.Status.ToString(),
            CreatedAt = meetingRequest.CreatedAt,
            UpdatedAt = meetingRequest.UpdatedAt
        };
    }
}
