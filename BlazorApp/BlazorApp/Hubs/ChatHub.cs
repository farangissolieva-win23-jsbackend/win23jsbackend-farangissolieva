//using Microsoft.AspNetCore.SignalR;
//using System.Collections.Concurrent;

//namespace BlazorApp.Hubs
//{
//    public class ChatHub : Hub
//    {
//        private static readonly ConcurrentDictionary<string, string> UserConnections = new();

//        public override Task OnConnectedAsync()
//        {
//            var userName = Context.User?.Identity?.Name;
//            if (!string.IsNullOrEmpty(userName))
//            {
//                UserConnections[userName] = Context.ConnectionId;
//            }

//            return base.OnConnectedAsync();
//        }

//        public override Task OnDisconnectedAsync(Exception? exception)
//        {
//            var userName = Context.User?.Identity?.Name;
//            if (!string.IsNullOrEmpty(userName))
//            {
//                UserConnections.TryRemove(userName, out _);
//            }
//            return base.OnDisconnectedAsync(exception);
//        }

//        public async Task Typing(string userName)
//        {
//            await Clients.Others.SendAsync("UserTyping", userName);
//        }

//        public async Task SendMessageToAll(string userName, string firstName, string lastName, string profileImgUrl, string message, DateTime timestamp)
//        {
//            await Clients.All.SendAsync("ReceiveMessage", userName, firstName, lastName, profileImgUrl, message, timestamp);
//        }

//        public async Task SendMessageToUser(string recipientUserName, string senderUserName, string senderFirstName, string senderLastName, string senderProfileImgUrl, string message, DateTime timestamp)
//        {
//            if (UserConnections.TryGetValue(recipientUserName, out var recipientConnectionId))
//            {
//                await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", senderUserName, senderFirstName, senderLastName, senderProfileImgUrl, message, timestamp);
//            }
//        }


//    }
//}



using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;


namespace BlazorApp.Hubs;

public class ChatHub(ILogger<ChatHub> logger, IHttpContextAccessor httpContextAccessor) : Hub
{
    private static readonly ConcurrentDictionary<string, string> UserConnections = new();
    private readonly ILogger<ChatHub> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public override Task OnConnectedAsync()
    {

        var userName = _httpContextAccessor.HttpContext!.User.Identity!.Name;
       //var userName = Context.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(userName))
        {
            UserConnections[userName] = Context.ConnectionId;
        }
        return base.OnConnectedAsync();
    }


    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }

    public async Task Typing(string userName)
    {
        await Clients.Others.SendAsync("UserTyping", userName);
    }

    public async Task SendMessageToAll(string userName, string firstName, string lastName, string profileImgUrl, string message, DateTime timestamp)
    {
        await Clients.All.SendAsync("ReceiveMessage", userName, firstName, lastName, profileImgUrl, message, timestamp);
    }

    public async Task SendMessageToUser(string recipientUserName, string senderUserName, string senderFirstName, string senderLastName, string senderProfileImgUrl, string message, DateTime timestamp)
    {
        _logger.LogInformation($"Attempting to send message from {senderUserName} to {recipientUserName}");

        var recipientConnectionId = UserConnections.GetValueOrDefault(recipientUserName);

        if (!string.IsNullOrEmpty(recipientConnectionId))
        {
            await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", senderUserName, senderFirstName, senderLastName, senderProfileImgUrl, message, timestamp);
        }
        else
        {
            _logger.LogWarning($"Recipient {recipientUserName} not found.");
        }

    }
}
