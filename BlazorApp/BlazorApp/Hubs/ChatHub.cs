using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Hubs
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
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
            await Clients.User(recipientUserName).SendAsync("ReceiveMessage", senderUserName, senderFirstName, senderLastName, senderProfileImgUrl, message, timestamp);
        }


    }
}
