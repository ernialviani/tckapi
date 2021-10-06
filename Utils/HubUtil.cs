using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using TicketingApi.Entities;
using TicketingApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using TicketingApi.Models.v1.Users;
using TicketingApi.DBContexts;

namespace TicketingApi.Utils
{
    public interface IHubUtil{
        Task SendTicketHub(List<User> users, string excludedUser, int TicketId, string Message);
    }

    public class HubUtil : IHubUtil
    {
        private IHubContext<HubConnection> _hub { get; set; }
        private AppDBContext  _context;
        public HubUtil(AppDBContext context, IHubContext<HubConnection> hub){
                _context = context;
                _hub = hub;
        }
        public async Task SendTicketHub(List<User> users, string excludedUser, int TicketId, string Message)
        {
            try
            {
                List<string> sendTo = new List<string>();
                var eUser = _context.Users.Where(w => w.Email == excludedUser).FirstOrDefault();
                var allAdmin = _context.UserRoles.Where(w => w.RoleId == 1).ToList();
                var uAdmin = _context.Users.AsEnumerable().Where(w => allAdmin.Any(a => a.UserId == w.Id) && w.Id != eUser.Id).ToList();
                users.AddRange(uAdmin);

                var allAssigned = _context.TicketAssigns.Where(w => w.TicketId == TicketId && w.UserId != eUser.Id).Select(s => s.Users).ToList();
                users.AddRange(allAssigned);

                // var uAssigned = _context.Users.Where(w => allAssigned.Any(a => a.UserId))

                var listConnectionId = _context.SignalrConnections.AsEnumerable().Where(w => w.Connected == true && users.Any(a => a.Id == w.UserId) && w.UserId != eUser.Id).Select(s => s.ConnectionId).ToList<string>();
                await _hub.Clients.Clients(listConnectionId).SendAsync("ReceiveTicketHub", Message); 
            }
            catch (System.Exception e)
            {
                throw;
            }
           
        }
        
    }
}