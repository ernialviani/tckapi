using System.ComponentModel.DataAnnotations.Schema;

namespace TicketingApi.Models.v1.Users
{
    public class TeamMember
    {

        public int Id {get; set;}

        public int TeamId {get; set;}

        public Team Teams { get; set;}

        public int UserId {get; set;}

        public User Users {get;set;}

    }
}
