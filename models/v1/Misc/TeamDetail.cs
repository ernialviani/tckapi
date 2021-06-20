using TicketingApi.Models.v1.Users;

namespace TicketingApi.Models.v1.Misc
{
    public class TeamDetail
    {

        public int Id {get; set;}

        public int TeamId {get; set;}

        public Team Team { get; set;}

        public int? UserId {get; set;}

        public User User {get;set;}

    }
}