using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TicketingApi.Models.v1.Notifications;    

namespace TicketingApi.Hubs
{
    public class NotifyHub : Hub
    {
        public async Task SendMessage(Notif notif)
        {
            await Clients.All.SendAsync("ReceiveMessage", notif.Message);
        }
    }
}