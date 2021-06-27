using System.ComponentModel.DataAnnotations.Schema;
using TicketingApi.Models.v1.Users;

namespace TicketingApi.Models.v1.Users
{
    public class TeamDetail
    {

        public int Id {get; set;}

        public int TeamId {get; set;}

        public Team Team { get; set;}

        public int? MemberId {get; set;}

        [ForeignKey("MemberId")]
        public User Member {get;set;}

    }
}
