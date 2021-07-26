using System.Transactions;
using System.Reflection;
using Microsoft.VisualBasic;
using System;
using System.IO;    
using System.Drawing; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using TicketingApi.DBContexts;
using Microsoft.EntityFrameworkCore;
using TicketingApi.Models.v1.Tickets;
using TicketingApi.Models.v1.Users;
using TicketingApi.Models.v1.Misc;
using System.IdentityModel.Tokens.Jwt;
using TicketingApi.Entities;
using TicketingApi.Utils;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace TicketingApi.Controllers.v1.Tickets
{
     [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TicketController: ControllerBase
    {
       //  private readonly IConfiguration _configuration;
        private readonly AppDBContext  _context;
        private readonly IFileUtil _fileUtil;

        private readonly IMailUtil _mailUtil;
        private readonly IWebHostEnvironment _env; 
        public TicketController(AppDBContext context, IFileUtil fileUtil, IMailUtil mailUtil, IWebHostEnvironment env )
        {
            _context = context; 
            _fileUtil = fileUtil;
            _mailUtil = mailUtil;
            _env = env;   
        }

        public string GenerateTicketNumber(){
            string newNumber = "";
            int lastTicket = 0;
            int lastnumber = 0;
            string nowDate = DateTime.Today.ToString().Substring(0, 10).Replace("-", "").Replace("/", "");
            var lastTicketNumber = _context.Tickets.OrderByDescending(x => x.Id).FirstOrDefault(w => w.TicketNumber.Contains(nowDate));
            if(lastTicketNumber != null){
                lastnumber = Convert.ToInt32(lastTicketNumber.TicketNumber.Substring(8));
                lastTicket =  lastnumber + 1;
                newNumber =  nowDate + Convert.ToString(lastTicket);
            }
            else{
                newNumber = nowDate+"1";
            }
            return newNumber;
        }


        [HttpGet]
        [Authorize]
        public IActionResult GetTickets([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allTicket = _context.Tickets.AsNoTracking()
                        .Include(sd => sd.Senders)
                        .Include(st => st.Status)
                        .Include(td => td.TicketDetails).ThenInclude(s => s.Users)
                        .Include(ta => ta.TicketAssigns).ThenInclude(s => s.Teams).ThenInclude(s => s.TeamMembers).ThenInclude(s => s.Users)
                        .Include(ap => ap.Apps)
                        .Include(md => md.Modules)
                        .Include(mda => mda.Medias)
                    .Select(e => new {
                        e.Id, e.TicketNumber, e.Subject, e.Comment, e.SolvedBy, e.SolvedAt, e.RejectedBy, e.RejectedReason, e.RejectedAt, e.CreatedAt, e.UpdatedAt,
                        TicketDetails = e.TicketDetails.Select(t => new { 
                            t.Id, t.Comment, t.Flag, t.CreatedAt, t.UpdatedAt, 
                            Medias = t.Medias == null ? null : t.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelId == t.Id && w.RelType == "TD"),
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image }
                        }),
                        TicketAssigns = e.TicketAssigns.Select(t => new { 
                            t.Id,
                            t.AssignType, 
                            Team = t.Teams == null ? null : new { 
                                t.Teams.Id, 
                                t.Teams.Name, 
                                TeamMembers = t.Teams.TeamMembers.Select(td => new {
                                     td.Id,
                                     Users = new { 
                                         UserId = td.Users.Id, 
                                         td.Users.Email, 
                                         FullName = td.Users.FirstName + " " + td.Users.LastName, td.Users.Image 
                                     } 
                                 })
                             }, 
                            t.TeamAt, 
                            t.UserId,
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image }, 
                            t.UserAt, 
                            t.Viewed, 
                            t.ViewedAt
                        }),
                        e.Status, e.Apps, e.Modules,
                        Senders = new { e.Senders.Id, e.Senders.Email, FullName = e.Senders.FirstName + " " + e.Senders.LastName, e.Senders.Image, e.Senders.LoginStatus },
                        Medias = e.Medias == null ? null : e.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelId == e.Id && w.RelType == "T")
                    }).OrderByDescending(e => e.Id);
           return Ok(allTicket);
        }

        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetTicketById(int id)
        {
              var ticket = _context.Tickets.AsNoTracking().Where(w => w.Id == id)
                        .Include(sd => sd.Senders)
                        .Include(st => st.Status)
                        .Include(td => td.TicketDetails).ThenInclude(s => s.Users)
                        .Include(ta => ta.TicketAssigns).ThenInclude(s => s.Teams).ThenInclude(s => s.TeamMembers).ThenInclude(s => s.Users)
                        .Include(ap => ap.Apps)
                        .Include(md => md.Modules)
                        .Include(mda => mda.Medias)
                    .Select(e => new {
                        e.Id, e.TicketNumber, e.Subject, e.Comment, e.SolvedBy, e.SolvedAt, e.RejectedBy, e.RejectedReason, e.RejectedAt, e.CreatedAt, e.UpdatedAt,
                        TicketDetails = e.TicketDetails.Select(t => new { 
                            t.Id, t.Comment, t.Flag, t.CreatedAt, t.UpdatedAt, 
                            Medias = t.Medias == null ? null : t.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelType == "TD"),
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image }
                        }),
                        TicketAssigns = e.TicketAssigns.Select(t => new { 
                            t.Id,
                            t.AssignType, 
                            Team = t.Teams == null ? null : new { 
                                t.Teams.Id, 
                                t.Teams.Name, 
                                TeamMembers = t.Teams.TeamMembers.Select(td => new {
                                     td.Id,
                                     Users = new { 
                                         UserId = td.Users.Id, 
                                         td.Users.Email, 
                                         FullName = td.Users.FirstName + " " + td.Users.LastName, td.Users.Image 
                                     } 
                                 })
                             }, 
                            t.TeamAt, 
                            t.UserId,
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image }, 
                            t.UserAt, 
                            t.Viewed, 
                            t.ViewedAt
                        }),
                        e.Status, e.Apps, e.Modules,
                        Senders = new { e.Senders.Id, e.Senders.Email, FullName = e.Senders.FirstName + " " + e.Senders.LastName, e.Senders.Image, e.Senders.LoginStatus },
                        Medias = e.Medias == null ? null : e.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelType == "T")
                    }).FirstOrDefault();
                       
            if(ticket != null){
                return Ok(ticket);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        [Route("mail/send")]
        public IActionResult SendMail([FromForm]string to,[FromForm] string subject,[FromForm] string EmailBody, [FromForm]IList<IFormFile> file)
        {
            try
            {
                _mailUtil.SendEmailAsync(
                        new MailType {
                            ToEmail=to,
                            Subject=subject,
                            Body=EmailBody,
                            Attachments = new List<IFormFile>(file)
                        }
                );

                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]Ticket request,[FromForm] string sender, [FromForm]IList<IFormFile> file)
        {
           Sender requestSender = JsonConvert.DeserializeObject<Sender>(sender);
            var exitingsSender = _context.Senders.AsNoTracking().Where(e => e.Email == requestSender.Email).FirstOrDefault();
           
           using (var transaction =  _context.Database.BeginTransaction())
           {
             try
              {
                 if (exitingsSender == null){
                    Sender newSender = new Sender() {
                        FirstName = requestSender.FirstName,
                        LastName = requestSender.LastName,
                        Email = requestSender.Email,
                        Password = "",
                        Salt = "",
                        LoginStatus = false,
                        CreatedAt = DateTime.Now 
                    };
                    _context.Senders.Add(newSender);
                    _context.SaveChanges();
                    exitingsSender = _context.Senders.AsNoTracking().Where(e => e.Email == requestSender.Email).FirstOrDefault();
                }

                Ticket ticketEntity = new Ticket()
                {   
                    TicketNumber = GenerateTicketNumber(),
                    Subject = request.Subject,
                    Comment = request.Comment,
                    AppId =  request.AppId,
                    ModuleId = request.ModuleId,
                    SenderId = exitingsSender.Id,
                    StatId= 1,
                    CreatedAt = DateTime.Now
                };

                _context.Tickets.Add(ticketEntity);
                _context.SaveChanges();
                var newTicket = _context.Tickets.AsNoTracking().Where(w => w.TicketNumber == ticketEntity.TicketNumber).FirstOrDefault();
                foreach(var f in file){
                    Media uploadedFile = _fileUtil.FileUpload(f, "Tickets");
                    _context.Medias.Add(new Media{
                        FileName = "Tickets/"+uploadedFile.FileName,
                        FileType = uploadedFile.FileType,
                        RelId = newTicket.Id,
                        RelType = "T"
                    });
                }
                _context.SaveChanges();
         //       var getManager = _context.Users.Where(w => w.UserRoles.Any(a => a.RoleId == 2) && w.UserDepts.Any(a => a.DepartmentId == 2) ).ToList();
                 var listManager = (from u in _context.Users 
                                    join ur in _context.UserRoles on u.Id equals ur.UserId
                                    join ud in _context.UserDepts on u.Id equals ud.UserId
                                    where (ur.RoleId == 2 && ud.DepartmentId == 2)
                                    select u 
                                    ).ToList();

                foreach (var mg in listManager)
                {
                    _context.TicketAssigns.Add(new TicketAssign{
                        TicketId = newTicket.Id,
                        UserId = mg.Id,
                        UserAt = DateTime.Now,
                        AssignType= "M",
                        Viewed = false
                    });

                    _mailUtil.SendEmailAsync(
                        new MailType {
                            ToEmail=mg.Email,
                            Subject= "New Ticket Number " + newTicket.TicketNumber,
                            Title= request.Subject,
                            Body= request.Comment,
                            TicketFrom= requestSender.Email,
                            TicketApp= "",
                            TicketModule="",
                            Attachments = new List<IFormFile>(file),
                            ButtonLink = "http://localhost:3000/admin/ticket?tid="+ newTicket.Id +"&open=true",
                        }
                    );

                }

                _context.SaveChanges();
                transaction.Commit();
                return Ok(ticketEntity);
            }   
            catch (System.Exception e) {
                transaction.Rollback();
               return BadRequest(e.Message);
            }  
           } 
        }

        [HttpPost("{id}")]
        [Authorize]
        [Route("update")]
        public IActionResult updateTicket(int id,[FromForm]Ticket request,[FromForm] string sender,  [FromForm]IList<IFormFile> file)
        {
            using (var transaction =  _context.Database.BeginTransaction())
            {
                try {
                    var ticketExist =  _context.Tickets .Where(e => e.Id == id) .FirstOrDefault();
                    if (ticketExist == null) { return NotFound("ticket Not Found !"); }
            
                    ticketExist.Subject = request.Subject;
                    ticketExist.Comment = request.Comment;
                    ticketExist.AppId = request.AppId;
                    ticketExist.ModuleId = request.ModuleId;
                    ticketExist.StatId = request.StatId;
                    ticketExist.SenderId = request.SenderId;
                    ticketExist.SolvedBy = request.SolvedBy;
                    ticketExist.SolvedAt = request.SolvedAt;
                    ticketExist.RejectedBy = request.RejectedBy;
                    ticketExist.RejectedReason = request.RejectedReason;
                    ticketExist.RejectedAt = request.RejectedAt;
                    ticketExist.UpdatedAt = DateTime.Now;

                    _context.SaveChanges();
                    transaction.Commit();

                    return Ok(ticketExist);
                }
                catch (System.Exception e) {
                    transaction.Rollback();
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpPost]
        [Authorize]
        [Route("post-comment")]
        public IActionResult postDetail( [FromForm]TicketDetail request, [FromForm] string sender, [FromForm] string user, [FromForm]IList<IFormFile> file, [FromForm] string ticketAssign )
        {
            using (var transaction =  _context.Database.BeginTransaction())
            {
                try {
                    var currentTicket =  _context.Tickets .Where(e => e.Id == request.TicketId) .FirstOrDefault();
                    if (currentTicket == null) { return NotFound("ticket Not Found !"); }

                    TicketDetail td = new TicketDetail(){
                        TicketId = request.TicketId,
                        UserId = request.UserId,
                        Comment = request.Comment,
                        Flag = false,
                        CreatedAt = DateTime.Now
                    };

                    _context.TicketDetails.Add(td);
                    _context.SaveChanges();


                    var newTd = _context.TicketDetails.OrderByDescending(e => e.Id).FirstOrDefault();

                    foreach(var f in file){
                        Media uploadedFile = _fileUtil.FileUpload(f, "TicketDetails");
                        _context.Medias.Add(new Media{
                            FileName = "TicketDetails/"+uploadedFile.FileName,
                            FileType = uploadedFile.FileType,
                            RelId = newTd.Id,
                            RelType = "TD"
                        });
                    }
                    
                    Sender requestSender = JsonConvert.DeserializeObject<Sender>(sender);
                    User requestUser = JsonConvert.DeserializeObject<User>(user);
                    IList<TicketAssign> requestAssign =  JsonConvert.DeserializeObject<IList<TicketAssign>>(ticketAssign);
                //    var currentTicket = _context.Tickets.Where(w => w.Id == request.TicketId).FirstOrDefault();
                    var currentUser = _context.Users.Where(w => w.Id == request.UserId).FirstOrDefault();

                    foreach (var assign in requestAssign)
                    {
                        if (requestUser.Email != assign.Users.Email)
                        {
                            _mailUtil.SendEmailPostCommentAsync(
                                new MailType {
                                    ToEmail= assign.Users.Email,
                                    Subject= "New Comment on Ticket Number " + currentTicket.TicketNumber,
                                    Title= currentTicket.Subject,
                                    Body= request.Comment,
                                    TicketFrom= requestSender.Email,
                                    TicketApp= "",
                                    TicketModule="",
                                    Attachments = new List<IFormFile>(file),
                                    UserFullName = currentUser.FirstName + " " + currentUser.LastName,
                                    ButtonLink = "http://localhost:3000/admin/ticket?tid="+ currentTicket.Id +"&open=true",
                                }
                            );
                            
                        }
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    return GetTicketById(request.TicketId);
                }
                catch (System.Exception e) {
                    transaction.Rollback();
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpPost]
        [Authorize]
        [Route("status-update")]
        public IActionResult TicketStatusUpdate([FromBody]Ticket[] body){        
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var ticket in body)
                    {
                        var ctickets = _context.Tickets.Where(w => w.Id == ticket.Id).FirstOrDefault();
                        ctickets.StatId = ticket.StatId;
                        ctickets.UpdatedAt = DateTime.Now;
                        if(ticket.StatId == 2){ // OPEN
                            var assign = _context.TicketAssigns.Where(w => w.TicketId == ticket.Id && w.AssignType == "M").FirstOrDefault();
                            assign.Viewed = true;
                            assign.ViewedAt = DateTime.Now;
                        }
                        else if(ticket.StatId == 5){ //solve
                            ctickets.SolvedAt = DateTime.Now;
                            ctickets.SolvedBy = ticket.SolvedBy;
                        }
                        else if(ticket.StatId == 6){
                            ctickets.RejectedAt = DateTime.Now;
                            ctickets.RejectedBy = ticket.RejectedBy;
                            ctickets.RejectedReason = ticket.RejectedReason;
                        }
                        _context.SaveChanges();
                    }
                    transaction.Commit();
                    return Ok();
                }
                catch (System.Exception e)
                {
                    transaction.Rollback();
                    return BadRequest(e.Message);
                }
            }
        }


        [HttpPost]
        [Authorize]
        [Route("ticket-assign")]
        public IActionResult TicketAssign([FromBody]TicketAssign[] body){
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var assign in body)
                    {
                        var ctickets = _context.Tickets.Where(w => w.Id == assign.TicketId).FirstOrDefault();
                        ctickets.UpdatedAt = DateTime.Now;
                        
                        if (assign.AssignType == "M")
                        {
                            ctickets.StatId = 1;
                            _context.TicketAssigns.Add(new TicketAssign(){
                                    TicketId = assign.TicketId,
                                    UserId = assign.UserId,
                                    UserAt = DateTime.Now,
                                    Viewed = false,
                                    AssignType = "M"
                            });
                        }
                        else if (assign.AssignType == "T")
                        {
                             ctickets.StatId = 3;
                            var team = _context.Teams.Where(w => w.Id == assign.TeamId).FirstOrDefault();
                            _context.TicketAssigns.Add(new TicketAssign(){
                                    TicketId = assign.TicketId,
                                    TeamId = assign.TeamId,
                                    TeamAt = DateTime.Now,
                                    UserId = team.LeaderId,
                                    Viewed = false,
                                    AssignType = "T"
                            });
                            
                            var cSender = _context.Senders.Where(w => w.Id == ctickets.SenderId).FirstOrDefault();
                            var cUser = _context.Users.Where(w => w.Id == team.LeaderId ).FirstOrDefault();
                            _mailUtil.SendEmailPostCommentAsync(
                                new MailType {
                                    ToEmail= cUser.Email,
                                    Subject= "Ticket Number " + ctickets.TicketNumber + " has been assign to your team.",
                                    Title= ctickets.Subject,
                                    Body= ctickets.Comment,
                                    TicketFrom= cSender.Email,
                                    TicketApp= "",
                                    TicketModule="",
                                    Attachments = null,
                                    UserFullName = " ",
                                    ButtonLink = "http://localhost:3000/admin/ticket?tid="+ ctickets.Id +"&open=true",
                                    
                                }
                            );
                        }
                        else
                        {
                             ctickets.StatId = 3;
                            _context.TicketAssigns.Add(new TicketAssign(){
                                    TicketId = assign.TicketId,
                                    UserId = assign.UserId,
                                    UserAt = DateTime.Now,
                                    Viewed = false,
                                    AssignType = "U"
                            });
                            var cSender = _context.Senders.Where(w => w.Id == ctickets.SenderId).FirstOrDefault();
                            var cUser = _context.Users.Where(w => w.Id == assign.UserId).FirstOrDefault();
                            _mailUtil.SendEmailPostCommentAsync(
                                new MailType {
                                    ToEmail= cUser.Email,
                                    Subject= "Ticket Number " + ctickets.TicketNumber + " has been assign to you.",
                                    Title= ctickets.Subject,
                                    Body= ctickets.Comment,
                                    TicketFrom= cSender.Email,
                                    TicketApp= "",
                                    TicketModule="",
                                    Attachments = null,
                                    UserFullName = " ",
                                    ButtonLink = "http://localhost:3000/admin/ticket?tid="+ ctickets.Id +"&open=true",
                                }
                            );
                        }
                    
                        _context.SaveChanges();
                    }
                    transaction.Commit();
                    return Ok();
                }
                catch (System.Exception e)
                {
                    transaction.Rollback();
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpPost("{id}")]
        public IActionResult PutTicket(int id,[FromForm]Ticket request)
        {
            try
            {
                 var ticketExist =  _context.Tickets
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                
                if (ticketExist == null)
                {
                    return NotFound("ticket Not Found !");
                }

                var transaction =  _context.Database.BeginTransaction();
            
                ticketExist.Subject = request.Subject;
                ticketExist.Comment = request.Comment;
                ticketExist.AppId = request.AppId;
                ticketExist.ModuleId = request.ModuleId;
                ticketExist.StatId = request.StatId;
                ticketExist.SenderId = request.SenderId;
                ticketExist.SolvedBy = request.SolvedBy;
                ticketExist.SolvedAt = request.SolvedAt;
                ticketExist.RejectedBy = request.RejectedBy;
                ticketExist.RejectedReason = request.RejectedReason;
                ticketExist.RejectedAt = request.RejectedAt;
                ticketExist.UpdatedAt = new DateTime();

                _context.SaveChanges();
                transaction.Commit();

                return Ok(ticketExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteTicketById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var ticketExist =   _context.Tickets
                        .Where(e => e.Id == id)
                        .Include(td => td.TicketDetails)
                        .Include(ta => ta.TicketAssigns)
                        .FirstOrDefault();
                var mediasExist = _context.Medias.Where(e => e.RelId == id && e.RelType == "T");

                foreach (var item in mediasExist)
                {
                    var isRemoved = _fileUtil.Remove("Tickets/"+item.FileName);
                    if(isRemoved) _context.Medias.Remove(item);
                }
      
                foreach (var itemDetail in ticketExist.TicketDetails)
                {
                    var mediaDetailExist = _context.Medias.Where(e => e.RelId == id && e.RelType == "TD");
                    foreach (var item in mediasExist)
                    {
                        var isRemoved = _fileUtil.Remove("Tickets/"+item.FileName);
                        if(isRemoved) _context.Medias.Remove(item);
                    }
                    
                    _context.TicketDetails.Remove(itemDetail);

                }
                foreach (var itemAssign in ticketExist.TicketAssigns)
                {
                    _context.TicketAssigns.Remove(itemAssign);
                }

                _context.Tickets.Remove(ticketExist);
                _context.SaveChanges();
                transaction.Commit();
                return Ok();
            }
            catch (System.Exception e)
            {
                
                return BadRequest(e.Message);
            }
            // [Route("~/api/Delete/{fileName}")]
            // [Route("~/api/Delete/{isValid:bool}")] 
            // [Route("~/api/Delete/{fileName}/{isValid}")] 
            // public  void Delete(string fileName, bool? isValid)
            // {
            
            // }
            
        }  
    }
}