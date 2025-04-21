using Microsoft.AspNetCore.SignalR;

namespace ProkodersECommerce.Hubs
{
    public class CartHub : Hub
    {
        public async Task NotifyCartUpdated(string userId)
        {
            await Clients.User(userId).SendAsync("ReceiveCartUpdate");
        }
    }
}
