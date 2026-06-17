using Microsoft.AspNetCore.SignalR;

namespace Server_Project_01
{
    public class ChatHub : Hub
    {

    }

    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.GetHttpContext().Request.Query["username"];
        }
    }
}
