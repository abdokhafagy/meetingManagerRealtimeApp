using MeetingManager.Application.DTOs.MeetingRequest;

namespace MeetingManager.Application.Interfaces;

public interface INotificationService
{
    Task NotifyManagerOfNewRequestAsync(int managerId, MeetingRequestDto request);
    Task NotifySecretaryOfStatusChangeAsync(int secretaryId, int requestId, string status);
}
