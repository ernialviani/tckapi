using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TicketingApi.Models.v1.Notifications;  
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using TicketingApi.DBContexts;  

namespace TicketingApi.Hubs
{
    public class NotifyHub : Hub
    {
        private readonly AppDBContext  _context;

        public NotifyHub(AppDBContext dbContext)
        {
            _context = dbContext; 
        }
        public override Task OnConnectedAsync()
        {
              var httpContext = Context.GetHttpContext();
              var token = new JwtSecurityTokenHandler().ReadJwtToken(httpContext.Request.Query["token"]);
              var Email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;
              var cUser = _context.Users.Where(w => w.Email == Email).FirstOrDefault();

              var connections = _context.SignalrConnections.Where(w => w.UserId == cUser.Id).ToList();
              var transaction = _context.Database.BeginTransaction();
              if(connections == null) {
                  _context.SignalrConnections.Add(new SignalrConnection{
                      UserId = cUser.Id,
                      ConnectionId = Context.ConnectionId,
                      Connected = true
                  });
                  _context.SaveChanges();
              }
              else{
                  var disconnectedConnection = _context.SignalrConnections.Where(w => w.UserId == cUser.Id && w.Connected == false).FirstOrDefault();
                  if(disconnectedConnection == null){
                    _context.SignalrConnections.Add(new SignalrConnection{
                        UserId = cUser.Id,
                        ConnectionId = Context.ConnectionId,
                        Connected = true
                     });
                  }
                  else{
                      disconnectedConnection.ConnectionId = Context.ConnectionId;
                      disconnectedConnection.Connected = true;
                  }
                  _context.SaveChanges();
              }
              transaction.Commit();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            var conId = Context.ConnectionId;

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Notif notif)
        {
            await Clients.All.SendAsync("ReceiveMessage", notif.Message);
        }
    }
}