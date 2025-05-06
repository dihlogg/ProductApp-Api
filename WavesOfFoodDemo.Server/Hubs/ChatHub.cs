using Microsoft.AspNetCore.SignalR;

namespace WavesOfFoodDemo.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinChatGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }

        public async Task LeaveChatGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());
        }

        public async Task SendMessage(string userId, string messageText, string sender)
        {
            Console.WriteLine($"Sending message to group {userId}: {messageText}, Sender: {sender}, ConnectionId: {Context.ConnectionId}");

            if (sender == "user")
            {
                // Nếu người gửi là người dùng, chỉ gửi cho những người khác trong nhóm
                await Clients.OthersInGroup(userId.ToString()).SendAsync("ReceiveMessage", messageText, sender);
            }
            else if (sender == "bot")
            {
                // Nếu người gửi là bot, gửi cho tất cả người dùng trong nhóm (bao gồm cả người gọi)
                await Clients.Group(userId.ToString()).SendAsync("ReceiveMessage", messageText, sender);
            }
        }
    }
}
