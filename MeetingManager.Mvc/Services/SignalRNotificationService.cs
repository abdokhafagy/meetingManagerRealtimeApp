using MeetingManager.Application.DTOs.MeetingRequest;
using MeetingManager.Application.Interfaces;
using MeetingManager.Mvc.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MeetingManager.Mvc.Services;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyManagerOfNewRequestAsync(int managerId, MeetingRequestDto request)
    {
        await _hubContext.Clients.Group($"User_{managerId}")
            .SendAsync("ReceiveNewRequest", request);
    }

    public async Task NotifySecretaryOfStatusChangeAsync(int secretaryId, int requestId, string status)
    {
        await _hubContext.Clients.Group($"User_{secretaryId}")
            .SendAsync("ReceiveStatusUpdate", requestId, status);
    }
}
