using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using WavesOfFoodDemo.Server.Dtos;
using WavesOfFoodDemo.Server.Dtos.CartDetails;

namespace WavesOfFoodDemo.Server.Hubs
{
    public class CartHub : Hub
    {
        public async Task JoinCartGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }
        public async Task LeaveCartGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());
        }
        public async Task SendCartUpdate(string userId, string action, CartDetailsCreateDto cartItem)
        {
            Console.WriteLine($"Sending cart update: {action}, ProductId: {cartItem.ProductId}");

            // send to group userId để tất cả user đều nhận được event
            await Clients.Group(userId).SendAsync("ReceiveCartUpdate", action, cartItem);
        }
    }
}
