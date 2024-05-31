using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Hubs
{
	public class ChatHub : Hub
	{
		private static readonly ConcurrentDictionary<string, string> UserConnections = new();
		private readonly ILogger<ChatHub> _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly AuthenticationStateProvider _authenticationStateProvider;

		public ChatHub(ILogger<ChatHub> logger, IHttpContextAccessor httpContextAccessor, AuthenticationStateProvider authenticationStateProvider)
		{
			_logger = logger;
			_httpContextAccessor = httpContextAccessor;
			_authenticationStateProvider = authenticationStateProvider;
		}

		public override async Task OnConnectedAsync()
		{
			var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;

			if (!string.IsNullOrEmpty(userName))
			{
				UserConnections[userName] = Context.ConnectionId;
				_logger.LogInformation($"User {userName} connected with connection ID: {Context.ConnectionId}");
			}
			else
			{
				_logger.LogWarning("User connected without a valid username.");
			}

			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;

			if (!string.IsNullOrEmpty(userName) && UserConnections.TryRemove(userName, out _))
			{
				_logger.LogInformation($"User {userName} disconnected and removed from connections.");
			}

			await base.OnDisconnectedAsync(exception);
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

			if (UserConnections.TryGetValue(recipientUserName, out var recipientConnectionId))
			{
				await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", senderUserName, senderFirstName, senderLastName, senderProfileImgUrl, message, timestamp);
				_logger.LogInformation($"Message sent to {recipientUserName} with connection ID: {recipientConnectionId}");
			}
			else
			{
				_logger.LogWarning($"Recipient {recipientUserName} not found.");
			}
		}
	}
}




//using Microsoft.AspNetCore.SignalR;
//using System.Collections.Concurrent;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Components.Authorization;


//namespace BlazorApp.Hubs;

//public class ChatHub(ILogger<ChatHub> logger, IHttpContextAccessor httpContextAccessor, AuthenticationStateProvider authenticationStateProvider) : Hub
//{
//    private static readonly ConcurrentDictionary<string, string> UserConnections = new();
//    private readonly ILogger<ChatHub> _logger = logger;
//    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
//    private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
//    public override Task OnConnectedAsync()
//    {
//         return base.OnConnectedAsync();
//    }


//    public override Task OnDisconnectedAsync(Exception? exception)
//    {
//        return base.OnDisconnectedAsync(exception);
//    }

//    public async Task Typing(string userName)
//    {
//        await Clients.Others.SendAsync("UserTyping", userName);
//    }

//    public async Task SendMessageToAll(string userName, string firstName, string lastName, string profileImgUrl, string message, DateTime timestamp)
//    {
//        await Clients.All.SendAsync("ReceiveMessage", userName, firstName, lastName, profileImgUrl, message, timestamp);
//    }

//    public async Task SendMessageToUser(string recipientUserName, string senderUserName, string senderFirstName, string senderLastName, string senderProfileImgUrl, string message, DateTime timestamp)
//    {
//        _logger.LogInformation($"Attempting to send message from {senderUserName} to {recipientUserName}");

//        var recipientConnectionId = UserConnections.GetValueOrDefault(recipientUserName);

//        if (!string.IsNullOrEmpty(recipientConnectionId))
//        {
//            await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", senderUserName, senderFirstName, senderLastName, senderProfileImgUrl, message, timestamp);
//        }
//        else
//        {
//            _logger.LogWarning($"Recipient {recipientUserName} not found.");
//        }

//    }


//}
