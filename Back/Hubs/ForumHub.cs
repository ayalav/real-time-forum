using Microsoft.AspNetCore.SignalR;

namespace RealTimeForum.Hubs;

public class ForumHub : Hub
{
    // Broadcast an update message to everyone when a new post is added
    public async Task SendPostUpdate(string message)
    {
        await Clients.All.SendAsync("ReceivePostUpdate", message);
    }

    // Broadcast an update message to everyone when a new comment is added
    public async Task SendCommentUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveCommentUpdate", message);
    }
}
