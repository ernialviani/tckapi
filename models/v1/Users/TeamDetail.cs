using System.ComponentModel.DataAnnotations.Schema;
using TicketingApi.Models.v1.Users;

namespace TicketingApi.Models.v1.Users
{
    public class TeamDetail
    {

        public int Id {get; set;}

        public int TeamId {get; set;}

        public Team Teams { get; set;}

        public int UserId {get; set;}

        public User Users {get;set;}

    }
}
