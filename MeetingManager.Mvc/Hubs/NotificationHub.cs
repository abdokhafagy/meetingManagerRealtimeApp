using Microsoft.AspNetCore.SignalR;

namespace MeetingManager.Mvc.Hubs;

public class NotificationHub : Hub
{
    // Map connections to UserIds
    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()?.Session.GetString("UserId");
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
        }
        await base.OnConnectedAsync();
    }
}
