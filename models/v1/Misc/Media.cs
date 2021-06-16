namespace TicketingApi.Models.v1.Misc
{
    public class Media
    {
        public int Id {get; set;}
        public string FileType {get; set;}
        public string FileName {get; set;}
        
        public int RelId {get;set;}
        public string RelType {get; set;}
    }
}