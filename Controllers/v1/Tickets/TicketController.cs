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
using TicketingApi.Controllers.v1.Authentication;
using TicketingApi.Models.v1.Users;
using TicketingApi.Models.v1.Misc;
using System.IdentityModel.Tokens.Jwt;
using TicketingApi.Entities;
using TicketingApi.Utils;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


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
        private readonly IConfiguration _config;

        public TicketController(AppDBContext context, IFileUtil fileUtil, IMailUtil mailUtil, IWebHostEnvironment env,  IConfiguration config )
        {
            _context = context; 
            _fileUtil = fileUtil;
            _mailUtil = mailUtil;
            _env = env;   
            _config = config;
            
        }
       
        public string GenerateTicketNumber(){
            string newNumber = "";
            int lastTicket = 0;
            int lastnumber = 0;
            string nowDate = DateTime.Today.ToString("yyMMdd");

            var lastTicketNumber = _context.Tickets.OrderByDescending(x => x.Id).FirstOrDefault(w => w.TicketNumber.Contains(nowDate));
            if(lastTicketNumber != null){
                lastnumber = Convert.ToInt32(lastTicketNumber.TicketNumber.Substring(6));
                lastTicket =  lastnumber + 1;
                newNumber =  nowDate + lastTicket.ToString("D4");
            }
            else{
                int first = 1;
                newNumber = nowDate + first.ToString("D4");
            }
            return newNumber;
        }

        #region GET 

        [HttpGet]
        [Authorize]
        public IActionResult GetTickets([FromHeader] string Authorization, [FromQuery]int u, [FromQuery]int r)
        {
          var allTicket = _context.Tickets.AsNoTracking()
                        .Include(t => t.Senders)
                        .Include(t => t.Status)
                        .Include(t => t.TicketDetails).ThenInclude(s => s.Users)
                        .Include(t => t.TicketAssigns).ThenInclude(s => s.Teams).ThenInclude(s => s.TeamMembers).ThenInclude(s => s.Users)
                        .Include(t => t.Apps)
                        .Include(t => t.Modules)
                        .Include(t => t.Medias)
                    .Select(e => new {
                        e.Id, e.TicketNumber, e.Subject, e.Comment, e.PendingBy, e.PendingAt, e.SolvedBy, e.SolvedAt, e.RejectedBy, e.RejectedReason, e.RejectedAt, e.CreatedBy, e.CreatedAt, e.UpdatedAt,
                        TicketDetails = e.TicketDetails.Select(t => new { 
                            t.Id, t.Comment, t.Flag, t.CreatedAt, t.UpdatedAt, t.Private,
                            Medias = t.Medias == null ? null : t.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelId == t.Id && w.RelType == "TD"),
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image, t.Users.Color }
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
                                         FullName = td.Users.FirstName + " " + td.Users.LastName, 
                                         td.Users.Image,
                                         td.Users.Color 
                                     } 
                                 })
                             }, 
                            t.TeamAt, 
                            t.UserId,
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image, t.Users.Color }, 
                            t.UserAt, 
                            t.Viewed, 
                            t.ViewedAt
                        }),
                        e.Status, e.Apps, e.Modules, e.TicketType,
                       // RequestFrom = e.TicketType == "E" ? "" : "",
                     //   new { Id = e.Senders.Id, Email = e.Senders.Email, FirstName = e.Senders.FirstName, LastName = e.Senders.LastName, Image = e.Senders.Image, LoginStatus = e.Senders.LoginStatus } :
                     //   new { Id = e.Users.Id, Email = e.Users.Email, FirstName = e.Users.FirstName, LastName = e.Users.LastName, Image = e.Users.Image, LoginStatus = true },
                        Users = e.UserId == null ? null : new { Id = e.Users.Id, Email = e.Users.Email, FirstName = e.Users.FirstName, LastName = e.Users.LastName, Image = e.Users.Image, Color=e.Users.Color },
                        Senders = e.SenderId == null ? null : new { Id = e.Senders.Id, Email = e.Senders.Email, FirstName = e.Senders.FirstName, LastName = e.Senders.LastName, Image = e.Senders.Image, Color=e.Senders.Color },
                        Medias = e.Medias == null ? null : e.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelId == e.Id && w.RelType == "T")
                    });

            IOrderedQueryable filtered = null;
            if(r == 1){ 
                 filtered = allTicket.OrderByDescending(e => e.Id);
            }
            else if(u > 0 ){
                 var activeUser = _context.Users.Where(w=>w.Id == u).FirstOrDefault();
                 filtered = allTicket.Where(w => w.TicketAssigns.Any(a => a.UserId == u || w.CreatedBy == activeUser.Email )).OrderByDescending(e => e.Id);
            }
            else{
                filtered = allTicket.OrderByDescending(e => e.Id);
            }
            
           return Ok(filtered);
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
                        e.Id, e.TicketNumber, e.Subject, e.Comment, e.PendingBy, e.PendingAt, e.SolvedBy, e.SolvedAt, e.RejectedBy, e.RejectedReason, e.RejectedAt, e.CreatedBy, e.CreatedAt, e.UpdatedAt,
                        TicketDetails = e.TicketDetails.Select(t => new { 
                            t.Id, t.Comment, t.Flag, t.CreatedAt, t.UpdatedAt, t.Private,
                            Medias = t.Medias == null ? null : t.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelType == "TD"),
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image, t.Users.Color }
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
                                         FullName = td.Users.FirstName + " " + td.Users.LastName, td.Users.Image, td.Users.Color
                                     } 
                                 })
                             }, 
                            t.TeamAt, 
                            t.UserId,
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image, t.Users.Color }, 
                            t.UserAt, 
                            t.Viewed, 
                            t.ViewedAt
                        }),
                        e.Status, e.Apps, e.Modules, e.TicketType,
                 
                        Users = e.UserId == null ? null : new { Id = e.Users.Id, Email = e.Users.Email, FirstName = e.Users.FirstName, LastName = e.Users.LastName, Image = e.Users.Image, Color = e.Users.Color },
                        Senders = e.SenderId == null ? null : new { Id = e.Senders.Id, Email = e.Senders.Email, FirstName = e.Senders.FirstName, LastName = e.Senders.LastName, Image = e.Senders.Image, Color = e.Senders.Color },
                        Medias = e.Medias == null ? null : e.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelType == "T")
                    }).FirstOrDefault();
                       
            if(ticket != null){
                return Ok(ticket);
            }
            return NotFound();
        }


        [HttpGet("profile-tickets")]
        [Authorize]
        public IActionResult GetProfileTicketByUser([FromHeader] string Authorization){
            var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
            var Email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            var cUser = _context.Users.Where(w => w.Email == Email).FirstOrDefault();
            try
            {
                var ticketAssign = _context.TicketAssigns.AsNoTracking().Where(e => e.UserId == cUser.Id);
                List<string> ticketss = new List<string>();
                foreach (var assign in ticketAssign)
                {
                    if(!ticketss.Contains(assign.TicketId.ToString())){
                        ticketss.Add(assign.TicketId.ToString());
                    }
                }

                var allTicket = _context.Tickets.AsNoTracking()
                        .Include(t => t.Senders)
                        .Include(t => t.Status)
                        .Include(t => t.TicketDetails).ThenInclude(s => s.Users)
                        .Include(t => t.TicketAssigns).ThenInclude(s => s.Teams).ThenInclude(s => s.TeamMembers).ThenInclude(s => s.Users)
                        .Include(t => t.Apps)
                        .Include(t => t.Modules)
                        .Include(t => t.Medias)
                    .Select(e => new {
                        e.Id, e.TicketNumber, e.Subject, e.Comment, e.PendingBy, e.PendingAt, e.SolvedBy, e.SolvedAt, e.RejectedBy, e.RejectedReason, e.RejectedAt, e.CreatedBy, e.CreatedAt, e.UpdatedAt,
                        TicketDetails = e.TicketDetails.Select(t => new { 
                            t.Id, t.Comment, t.Flag, t.CreatedAt, t.UpdatedAt, 
                            Medias = t.Medias == null ? null : t.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelId == t.Id && w.RelType == "TD"),
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image, t.Users.Color }
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
                                         FullName = td.Users.FirstName + " " + td.Users.LastName, 
                                         td.Users.Image, 
                                         td.Users.Color
                                     } 
                                 })
                             }, 
                            t.TeamAt, 
                            t.UserId,
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image, t.Users.Color }, 
                            t.UserAt, 
                            t.Viewed, 
                            t.ViewedAt
                        }),
                        e.Status, e.Apps, e.Modules, e.TicketType,
                        Users = e.UserId == null ? null : new { Id = e.Users.Id, Email = e.Users.Email, FirstName = e.Users.FirstName, LastName = e.Users.LastName, Image = e.Users.Image, Color=e.Users.Color },
                        Senders = e.SenderId == null ? null : new { Id = e.Senders.Id, Email = e.Senders.Email, FirstName = e.Senders.FirstName, LastName = e.Senders.LastName, Image = e.Senders.Image, Color=e.Senders.Color },
                        Medias = e.Medias == null ? null : e.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelId == e.Id && w.RelType == "T")
                    }).Where(w => ticketss.Contains(w.Id.ToString()) || w.CreatedBy == Email);

                return Ok(allTicket);
            }
            catch (System.Exception e)
            {
                  return BadRequest(e.Message);
            }
        }

        #endregion

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]Ticket request,[FromForm] string sender, [FromForm]IList<IFormFile> file)
        {
           using (var transaction =  _context.Database.BeginTransaction())
           {
             try
              {
                Ticket ticketEntity = new Ticket()
                {   
                    TicketNumber = GenerateTicketNumber(),
                    TicketType = request.TicketType,
                    Subject = request.Subject,
                    Comment = request.Comment,
                    AppId =  request.AppId,
                    ModuleId = request.ModuleId,
                    StatId= 1,
                    CreatedBy = request.CreatedBy,
                    CreatedAt = DateTime.Now,
                };

                Sender requestSender = new Sender();

                if(request.TicketType == "E"){
                    requestSender = JsonConvert.DeserializeObject<Sender>(sender);
                    var cClient = _context.ClientDetails.Where(w => requestSender.Email.Contains(w.Domain)).FirstOrDefault();
                    if(cClient == null) {
                        transaction.Rollback();
                        return BadRequest("Sorry, no access for this email address !");
                    }
                
                    var exitingsSender = _context.Senders.AsNoTracking().Where(e => e.Email == requestSender.Email).FirstOrDefault();
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
                        //add client user if not exist
                        exitingsSender = _context.Senders.AsNoTracking().Where(e => e.Email == requestSender.Email).FirstOrDefault();
                        ticketEntity.SenderId = exitingsSender.Id;
                   }
                   else{
                        ticketEntity.SenderId = exitingsSender.Id;
                   }
                }
                else if(request.TicketType == "I"){
                    ticketEntity.UserId = request.UserId; 
                }
                _context.Tickets.Add(ticketEntity);
                _context.SaveChanges();
                //save ticket

                var newTicket = _context.Tickets.AsNoTracking().Where(w => w.TicketNumber == ticketEntity.TicketNumber).FirstOrDefault();
                var app = _context.Apps.Where(w => w.Id == newTicket.AppId).FirstOrDefault();
                var appModule = _context.Modules.Where(w => w.Id == newTicket.ModuleId).FirstOrDefault();

                foreach(var f in file){
                    Media uploadedFile = _fileUtil.FileUpload(f, "Tickets");
                    _context.Medias.Add(new Media{
                        FileName = "Tickets/"+uploadedFile.FileName,
                        FileType = uploadedFile.FileType,
                        RelId = newTicket.Id,
                        TicketId = newTicket.Id,
                        RelType = "T"
                    });
                }
                _context.SaveChanges();
                //save attachments

                //ASSIGN AND MAIL CONFIG
                 List<User> listManager = new List<User>(); 
                 if(request.TicketType == "E"){
                    List<string> listMailTo = new List<string>();
                    // listManager = (from u in _context.Users 
                    //                 join ur in _context.UserRoles on u.Id equals ur.UserId
                    //                 join ud in _context.UserDepts on u.Id equals ud.UserId
                    //                 where (ur.RoleId == 2 && ud.DepartmentId == 2)
                    //                 select u 
                    //                 ).ToList();
                    listManager = _context.Users
                                .Include(i => i.UserRoles)
                                .Include(i => i.UserDepts)
                                .Where(w => w.UserRoles.Any(a => a.RoleId.Equals(2)) 
                                && w.UserDepts.Any(a => a.DepartmentId.Equals(2)) && w.Deleted == false )
                                .ToList();

                    foreach (var mg in listManager) {
                        _context.TicketAssigns.Add(new TicketAssign{
                            TicketId = newTicket.Id,
                            UserId = mg.Id,
                            UserAt = DateTime.Now,
                            AssignType= "M",
                            Viewed = false
                        });
                        listMailTo.Add(mg.Email);
                    }

                    _mailUtil.SendEmailAsync(
                        new MailType {
                            ToEmail=listMailTo,
                            Subject= "New Ticket Number " + newTicket.TicketNumber,
                            TicketNumber= newTicket.TicketNumber,
                            Title= request.Subject,
                            Body= request.Comment,
                            TicketFrom= requestSender.Email,
                            TicketApp= app.Name,
                            TicketModule=appModule.Name,
                            Attachments = new List<IFormFile>(file),
                            HomeSite = _config.GetSection("HomeSite").Value,
                           // ButtonLink = _config.GetSection("HomeSite").Value + "admin/ticket?tid="+ newTicket.Id +"&open=true",
                            ButtonLink =  _config.GetSection("HomeSite").Value + "admin/ticket?tid="+ newTicket.Id +"&open=true",
                        }
                    );
                    List<string> listMailToSender = new List<string>();
                    listMailToSender.Add(requestSender.Email);
                    _mailUtil.SendEmailAsync(
                        new MailType {
                            ToEmail=listMailToSender,
                            Subject= "New Ticket Number " + newTicket.TicketNumber,
                            TicketNumber=newTicket.TicketNumber,
                            Title= request.Subject,
                            Body= request.Comment,
                            TicketFrom= requestSender.Email,
                            TicketApp= app.Name,
                            TicketModule=appModule.Name,
                            Attachments = new List<IFormFile>(file),
                            HomeSite = _config.GetSection("HomeSite").Value,
                            ButtonLink = _config.GetSection("HomeSite").Value + "ticket?tid="+ newTicket.Id +"&open=true",
                        }
                    );
                 }
                 else if(request.TicketType == "I"){
                    List<string> listMailTo = new List<string>();
                    listManager = (from u in _context.Users 
                                    join ur in _context.UserRoles on u.Id equals ur.UserId
                                    join ud in _context.UserDepts on u.Id equals ud.UserId
                                    where (ur.RoleId == 2 && ud.DepartmentId == 3 && u.Deleted == false)
                                    select u 
                                    ).ToList();

                    foreach (var mg in listManager) {
                        _context.TicketAssigns.Add(new TicketAssign{
                            TicketId = newTicket.Id,
                            UserId = mg.Id,
                            UserAt = DateTime.Now,
                            AssignType= "M",
                            Viewed = false
                        });
                        listMailTo.Add(mg.Email);
                    }
                    
                    _mailUtil.SendEmailAsync(
                        new MailType {
                            ToEmail=listMailTo,
                            Subject= "New Ticket Number " + newTicket.TicketNumber,
                            TicketNumber=newTicket.TicketNumber,
                            Title= request.Subject,
                            Body= request.Comment,
                            TicketFrom= request.CreatedBy,
                            TicketApp= app.Name,
                            TicketModule=appModule.Name,
                            Attachments = new List<IFormFile>(file),
                            HomeSite = _config.GetSection("HomeSite").Value,
                            ButtonLink = _config.GetSection("HomeSite").Value + "admin/ticket?tid="+ newTicket.Id +"&open=true",
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
                    var cTicket =  _context.Tickets .Where(e => e.Id == request.TicketId).FirstOrDefault();
                    if (cTicket == null) { return NotFound("ticket Not Found !"); }
                    
                    Sender requestSender = JsonConvert.DeserializeObject<Sender>(sender);
                    User requestUser = JsonConvert.DeserializeObject<User>(user);
                    IList<TicketAssign> requestAssign =  JsonConvert.DeserializeObject<IList<TicketAssign>>(ticketAssign);
                    var cUser = _context.Users.Where(w => w.Id == request.UserId).FirstOrDefault();
                    var app = _context.Apps.Where(w => w.Id == cTicket.AppId).FirstOrDefault();
                    var appModule = _context.Modules.Where(w => w.Id == cTicket.ModuleId).FirstOrDefault();

                    TicketDetail td = new TicketDetail(){
                        TicketId = request.TicketId,
                        Comment = request.Comment,
                        Flag = false,
                        Private = request.Private,
                        CreatedAt = DateTime.Now
                    };

                    if(cTicket.TicketType == "E"){
                       td.UserId = request.UserId;
                    }
                    else if(cTicket.TicketType == "I"){
                        if(cTicket.CreatedBy != requestUser.Email){
                            td.UserId = requestUser.Id;
                            if(cTicket.StatId < 3){ //auto update state form open to in progress: input mba anik 27/09/2021
                                cTicket.StatId = 3;
                            }
                        }
                    }

                    _context.TicketDetails.Add(td);
                    _context.SaveChanges();

                    var newTd = _context.TicketDetails.OrderByDescending(e => e.Id).FirstOrDefault();
                    foreach(var f in file){
                        Media uploadedFile = _fileUtil.FileUpload(f, "TicketDetails");
                        _context.Medias.Add(new Media{
                            FileName = "TicketDetails/"+uploadedFile.FileName,
                            FileType = uploadedFile.FileType,
                            RelId = newTd.Id,
                            TicketDetailId = newTd.Id,
                            RelType = "TD"
                        });
                    }
                    
                    List<string> listMailTo = new List<string>();
                    foreach (var assign in requestAssign)
                    {
                        if (requestUser.Email != assign.Users.Email)
                        {
                          listMailTo.Add(assign.Users.Email);
                        }
                    }

                    List<string> listMailToEx = new List<string>();
                    var ticketRequestFrom = "";
                    if(cTicket.TicketType == "I"){
                        var fUser = _context.Users.Where(w => w.Id == cTicket.UserId && w.Deleted == false).FirstOrDefault();
                        ticketRequestFrom = fUser.Email; 
                        listMailTo.Add(ticketRequestFrom);

                    }
                    else if(cTicket.TicketType == "E"){
                        var fSender = _context.Senders.Where(w => w.Id == cTicket.SenderId).FirstOrDefault();
                        ticketRequestFrom = fSender.Email;
                        listMailToEx.Add(ticketRequestFrom);

                        //btn link config
                        var btnLink = "";
                        var sCode = "";
                        if(fSender.LoginStatus == true ) { btnLink = "mytickets?tn="+cTicket.TicketNumber; }
                        else{

                            sCode = _mailUtil.GenerateRandom4Code();
                            var jsonString =  JsonConvert.SerializeObject(new {
                                TicketNumber = cTicket.TicketNumber,
                                CreatedBy= fSender.Email,
                                SecurityCode = sCode,
                                ExpiredAt = DateTime.Now.AddDays(1).ToString()
                            });

                            btnLink = "mytickets?tcid="+ AncDecUtil.Encrypt(jsonString, "EPSYLONHOME2021$", true);
                        }
                        
                        if(!newTd.Private){
                            _mailUtil.SendEmailPostCommentForClientAsync(
                                new MailType {
                                    ToEmail= listMailToEx,
                                    Subject= "New Comment on Ticket Number " + cTicket.TicketNumber,
                                    Title= cTicket.Subject,
                                    Body= request.Comment,
                                    TicketFrom= ticketRequestFrom,
                                    TicketApp= app.Name,
                                    TicketModule=appModule.Name,
                                    Attachments = new List<IFormFile>(file),
                                    UserFullName = cUser.FirstName + " " + cUser.LastName,
                                    VerificationCode = string.IsNullOrEmpty(sCode) ? "" : "Code : " + sCode,
                                    DescVerificationCode= string.IsNullOrEmpty(sCode) ? "":" Use the code for access ticket page.",
                                    HomeSite = _config.GetSection("HomeSite").Value,
                                    ButtonLink = _config.GetSection("HomeSite").Value + btnLink
                                }
                            );
                        }
                    }

                    _mailUtil.SendEmailPostCommentAsync(
                        new MailType {
                            ToEmail= listMailTo,
                            Subject= "New Comment on Ticket Number " + cTicket.TicketNumber,
                            TicketNumber=cTicket.TicketNumber,
                            Title= cTicket.Subject,
                            Body= request.Comment,
                            TicketFrom= ticketRequestFrom,
                            TicketApp= app.Name,
                            TicketModule=appModule.Name,
                            Attachments = new List<IFormFile>(file),
                            UserFullName = cUser.FirstName + " " + cUser.LastName,
                            HomeSite = _config.GetSection("HomeSite").Value,
                            ButtonLink = _config.GetSection("HomeSite").Value + "admin/ticket?tid="+ cTicket.Id +"&open=true",
                        }
                    );

                    _context.SaveChanges();
                    transaction.Commit();

                    return Ok();
                  //  return GetTicketById(request.TicketId);
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
        public IActionResult TicketStatusUpdate([FromBody]Ticket[] body, [FromHeader] string Authorization) {   
         var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
           var Email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;     
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var ticket in body)
                    {
                        var cTickets = _context.Tickets.Where(w => w.Id == ticket.Id).FirstOrDefault();
                        cTickets.StatId = ticket.StatId;
                        cTickets.UpdatedAt = DateTime.Now;
                        if(ticket.StatId == 2){ // OPEN
                            var assign = _context.TicketAssigns.Where(w => w.TicketId == ticket.Id && w.AssignType == "M").FirstOrDefault();
                            assign.Viewed = true;
                            assign.ViewedAt = DateTime.Now;
                        }
                        else if(ticket.StatId == 4){
                            cTickets.PendingAt = DateTime.Now;
                            cTickets.PendingBy = Email;
                        }
                        else if(ticket.StatId == 5){ //solve
                            cTickets.SolvedAt = DateTime.Now;
                            cTickets.SolvedBy = Email;
                        }
                        else if(ticket.StatId == 6){
                            cTickets.RejectedAt = DateTime.Now;
                            cTickets.RejectedBy = Email;
                            cTickets.RejectedReason = ticket.RejectedReason;
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
        public IActionResult TicketAssign([FromBody]TicketAssign[] body, [FromHeader] string Authorization){
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
                    var Email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;
                    var authUser = _context.Users.Where(w => w.Email == Email).FirstOrDefault();
                    foreach (var assign in body)
                    {
                        var cTickets = _context.Tickets.Where(w => w.Id == assign.TicketId).FirstOrDefault();
                        cTickets.UpdatedAt = DateTime.Now;
                        
                        if (assign.AssignType == "M") {
                            cTickets.StatId = 1;
                            _context.TicketAssigns.Add(new TicketAssign(){
                                    TicketId = assign.TicketId,
                                    UserId = assign.UserId,
                                    UserAt = DateTime.Now,
                                    Viewed = false,
                                    AssignType = "M"
                            });
                        }
                        else if (assign.AssignType == "T") {

                            if(cTickets.StatId < 3) { cTickets.StatId = 3; }
                           
                            var team = _context.Teams.Where(w => w.Id == assign.TeamId).FirstOrDefault();
                            var teamMember = _context.TeamMembers.Where(w => w.TeamId == team.Id).ToList();
                            var cUser = _context.Users.Where(w => w.Id == team.ManagerId && w.Deleted == false).FirstOrDefault();
                            var app = _context.Apps.Where(w => w.Id == cTickets.AppId).FirstOrDefault();
                            var appModule = _context.Modules.Where(w => w.Id == cTickets.ModuleId).FirstOrDefault();
                            var mgAssign = _context.TicketAssigns.Where(w => w.TicketId == assign.TicketId && w.AssignType == "M").FirstOrDefault();
                          
                            if (mgAssign != null && mgAssign.Viewed == false)  {
                                mgAssign.Viewed = true;
                                mgAssign.ViewedAt = DateTime.Now;
                            }

                            var tAssign = _context.TicketAssigns.Where(w => w.TicketId == assign.TicketId && w.AssignType == "T" && w.TeamId == assign.TeamId ).FirstOrDefault();
                            if(tAssign == null) {
                            
                                
                                List<string> ListToMail = new List<string>();
                                ListToMail.Add(cUser.Email);
                                foreach (var member in teamMember)
                                {
                                    _context.TicketAssigns.Add(new TicketAssign(){
                                            TicketId = assign.TicketId,
                                            UserId = member.UserId,
                                            UserAt = DateTime.Now,
                                            Viewed = false,
                                            AssignType = "U"
                                    });

                                    var userMember = _context.Users.Where(w => w.Id == member.UserId && w.Deleted == false).FirstOrDefault();
                                    if(userMember != null){
                                      ListToMail.Add(userMember.Email);
                                    }
                                }

                                _context.TicketAssigns.Add(new TicketAssign(){
                                    TicketId = assign.TicketId,
                                    TeamId = assign.TeamId,
                                    TeamAt = DateTime.Now,
                                    UserId = team.ManagerId,
                                    Viewed = false,
                                    AssignType = "T"
                                });

                                List<string> LFile = new List<string>();
                                var medias = _context.Medias.Where(w => w.RelId == assign.TicketId && w.RelType == "T").ToList();
                                foreach (var media in medias)
                                {
                                    var path = Path.Combine(_env.ContentRootPath, "Medias/");
                                    var fPath =  Path.Combine(path, media.FileName);
                                    LFile.Add(fPath);
                                }

                                var ticketRequestFrom = "";
                                if(cTickets.TicketType == "I"){
                                    var fUser = _context.Users.Where(w => w.Id == cTickets.UserId).FirstOrDefault();
                                    ticketRequestFrom = fUser.Email; 
                                }
                                else if(cTickets.TicketType == "E"){
                                    var fSender = _context.Senders.Where(w => w.Id == cTickets.SenderId).FirstOrDefault();
                                    ticketRequestFrom = fSender.Email;
                                }
                                
                                _mailUtil.SendEmailPostCommentAsync(
                                    new MailType {
                                        ToEmail= ListToMail,
                                        Subject= "Ticket need team response [" + cTickets.TicketNumber +"]",
                                        TicketNumber=cTickets.TicketNumber,
                                        Title= cTickets.Subject,
                                        Body= cTickets.Comment,
                                        TicketFrom= ticketRequestFrom,
                                        TicketApp= app.Name,
                                        TicketModule=appModule.Name,
                                        Attachments = null,
                                        AttachmentsString = LFile,
                                        UserFullName = authUser.FirstName + " " + authUser.LastName,
                                        HomeSite = _config.GetSection("HomeSite").Value,
                                        ButtonLink = _config.GetSection("HomeSite").Value + "admin/ticket?tid="+ cTickets.Id +"&open=true",
                                        
                                    }
                                    
                                );
                                
                            }
                        }
                        else
                        {
                            if(cTickets.StatId < 3) { cTickets.StatId = 3; }

                            var cUser = _context.Users.Where(w => w.Id == assign.UserId).FirstOrDefault();
                            var app = _context.Apps.Where(w => w.Id == cTickets.AppId).FirstOrDefault();
                            var appModule = _context.Modules.Where(w => w.Id == cTickets.ModuleId).FirstOrDefault();
                            var mgAssign = _context.TicketAssigns.Where(w => w.TicketId == assign.TicketId && w.AssignType == "M").FirstOrDefault();
                            if (mgAssign != null  && mgAssign.Viewed == false) {
                                mgAssign.Viewed = true;
                                mgAssign.ViewedAt = DateTime.Now;
                            }

                            var uAssign = _context.TicketAssigns.Where(w => w.TicketId == assign.TicketId && w.AssignType == "U" && w.UserId == assign.UserId).FirstOrDefault();
                            if(uAssign == null){
                                   _context.TicketAssigns.Add(new TicketAssign(){
                                            TicketId = assign.TicketId,
                                            UserId = assign.UserId,
                                            UserAt = DateTime.Now,
                                            Viewed = false,
                                            AssignType = "U"
                                    });
                                    
                                    List<string> ListToMail = new List<string>();
                                    ListToMail.Add(cUser.Email);
                                    
                                    List<string> LFile = new List<string>();
                                    var medias = _context.Medias.Where(w => w.RelId == assign.TicketId && w.RelType == "T").ToList();
                                    foreach (var media in medias)
                                    {
                                        var path = Path.Combine(_env.ContentRootPath, "Medias/");
                                        var fPath =  Path.Combine(path, media.FileName);
                                        LFile.Add(fPath);
                                    }

                                    var ticketRequestFrom = "";
                                    if(cTickets.TicketType == "I"){
                                        var fUser = _context.Users.Where(w => w.Id == cTickets.UserId).FirstOrDefault();
                                        ticketRequestFrom = fUser.Email; 
                                    }
                                    else if(cTickets.TicketType == "E"){
                                        var fSender = _context.Senders.Where(w => w.Id == cTickets.SenderId).FirstOrDefault();
                                        ticketRequestFrom = fSender.Email;
                                    }

                                    _mailUtil.SendEmailPostCommentAsync(
                                        new MailType {
                                            ToEmail = ListToMail,
                                            Subject = "Ticket need response [" + cTickets.TicketNumber +"]",
                                            Title = cTickets.Subject,
                                            Body = cTickets.Comment,
                                            TicketNumber = cTickets.TicketNumber,
                                            TicketFrom = ticketRequestFrom,
                                            TicketApp = app.Name,
                                            TicketModule = appModule.Name,
                                            Attachments = null,
                                            AttachmentsString = LFile,
                                            UserFullName = authUser.FirstName + " " + authUser.LastName,
                                            HomeSite = _config.GetSection("HomeSite").Value,
                                            ButtonLink = _config.GetSection("HomeSite").Value + "admin/ticket?tid="+ cTickets.Id +"&open=true",
                                        }
                                    );
                            }
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
        [Route("ticket-assign-viewed")]
        public IActionResult TicketAssignViewed([FromBody]TicketAssign[] body){
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var assign in body)
                    {
                        var cTickets = _context.Tickets.Where(w => w.Id == assign.TicketId).FirstOrDefault();
                        cTickets.UpdatedAt = DateTime.Now;

                        var cAssign = _context.TicketAssigns.Where(w => w.Id == assign.Id).FirstOrDefault();
                        cAssign.ViewedAt = DateTime.Now;
                        cAssign.Viewed = true;

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
        [Authorize]
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

                foreach (var item in mediasExist) {
                    var isRemoved = _fileUtil.Remove(item.FileName);
                    if(isRemoved) _context.Medias.Remove(item);
                }
      
                foreach (var itemDetail in ticketExist.TicketDetails) {
                    var mediaDetailExist = _context.Medias.Where(e => e.RelId == id && e.RelType == "TD");
                    foreach (var item in mediasExist) {
                        var isRemoved = _fileUtil.Remove(item.FileName);
                        if(isRemoved) _context.Medias.Remove(item);
                    }
                    _context.TicketDetails.Remove(itemDetail);
                }

                foreach (var itemAssign in ticketExist.TicketAssigns) {
                    _context.TicketAssigns.Remove(itemAssign);
                }

                _context.Tickets.Remove(ticketExist);
                _context.SaveChanges();
                transaction.Commit();
                return Ok();
            }
            catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        } 


        [HttpPost]
        [Authorize]
        [Route("update-comment")]
        public IActionResult UpdateTicketComment([FromForm]TicketDetail request, [FromForm]IList<IFormFile> file){
              using (var transaction = _context.Database.BeginTransaction()) {
                try {
                    var cTDetails = _context.TicketDetails.Where(w => w.Id == request.Id).FirstOrDefault();
                    cTDetails.UpdatedAt = DateTime.Now;
                    cTDetails.Comment = request.Comment;

                    var cMedias = _context.Medias.Where(w => w.RelId == request.Id && w.RelType == "TD");
                    foreach (var item in cMedias){
                        var isRemoved = _fileUtil.Remove(item.FileName);
                        if(isRemoved) _context.Medias.Remove(item);
                    }
                    foreach(var f in file){
                        Media uploadedFile = _fileUtil.FileUpload(f, "TicketDetails");
                        _context.Medias.Add(new Media{
                            FileName = "TicketDetails/"+uploadedFile.FileName,
                            FileType = uploadedFile.FileType,
                            RelId = request.Id,
                            TicketDetailId = request.Id,
                            RelType = "TD"
                        });
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                    return Ok();
                }
                catch (System.Exception e)
                {
                    transaction.Rollback();
                    return BadRequest(e);
                }
            }
        }


        [HttpPost]
        [Authorize]
        [Route("delete-comment")]
        public IActionResult DeleteTicketCommentById([FromBody]TicketDetail[] body)
        {
            try
            {
                var transaction = _context.Database.BeginTransaction();
                foreach (var detail in body) {
                    var cTDetails = _context.TicketDetails.Where(w => w.Id == detail.Id).FirstOrDefault();
                    var mediaDetailExist = _context.Medias.Where(e => e.RelId == detail.Id && e.RelType == "TD");
                    foreach (var item in mediaDetailExist)
                    {
                        var isRemoved = _fileUtil.Remove(item.FileName);
                        if(isRemoved) _context.Medias.Remove(item);
                    }
                    _context.TicketDetails.Remove(cTDetails);
                    _context.SaveChanges();
                }
                transaction.Commit();
                return Ok();
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        } 





        ///////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////// CLIENT TICKET
        //////////////////////////////////////////////////////////////////////////

        [HttpGet("client")]
        [Authorize]
        public IActionResult GetClientTickets([FromHeader] string Authorization, [FromHeader] string TicketToken)
        {

          var Email = "";
          var TicketNumber = "";

          if(!string.IsNullOrEmpty(TicketToken)){ 
            var tokenPlain = AncDecUtil.DecryptString(TicketToken.Replace("Bearer ", ""), "EPSYLONHOME2021$", true);
            var property = new { CreatedBy = "", TicketNumber= "" };
            var tokenJson = JsonConvert.DeserializeAnonymousType(tokenPlain, property);
            if(tokenJson != null){
                Email = tokenJson.CreatedBy;
                TicketNumber=tokenJson.TicketNumber;
            }
          }
          else{ // client login true
             var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
             Email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;     
          }
 
          var cSender = _context.Senders.Where(w => w.Email == Email).FirstOrDefault();
          var allTicket = _context.Tickets.AsNoTracking()
                        .Where(w => (w.CreatedBy == Email && w.TicketType == "E") || w.SenderId == cSender.Id)
                        .Include(t => t.Senders)
                        .Include(t => t.Status)
                        .Include(t => t.TicketDetails).ThenInclude(s => s.Users)
                        .Include(t => t.TicketAssigns).ThenInclude(s => s.Teams).ThenInclude(s => s.TeamMembers).ThenInclude(s => s.Users)
                        .Include(t => t.Apps)
                        .Include(t => t.Modules)
                        .Include(t => t.Medias)
                    .Select(e => new {
                        e.Id, e.TicketNumber, e.Subject, e.Comment, e.PendingBy, e.PendingAt, e.SolvedBy, e.SolvedAt, e.RejectedBy, e.RejectedReason, e.RejectedAt, e.CreatedBy, e.CreatedAt, e.UpdatedAt,
                        TicketDetails = e.TicketDetails.Select(t => new { 
                            t.Id, t.Comment, t.Flag, t.CreatedAt, t.UpdatedAt, t.Private,
                            Medias = t.Medias == null ? null : t.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelId == t.Id && w.RelType == "TD"),
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image, t.Users.Color }
                        }).Where(w => w.Private == false ),
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
                                         FullName = td.Users.FirstName + " " + td.Users.LastName, 
                                         td.Users.Image,
                                         td.Users.Color 
                                     } 
                                 })
                             }, 
                            t.TeamAt, 
                            t.UserId,
                            Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image, t.Users.Color }, 
                            t.UserAt, 
                            t.Viewed, 
                            t.ViewedAt
                        }),
                        e.Status, e.Apps, e.Modules, e.TicketType,
                        Users = e.UserId == null ? null : new { Id = e.Users.Id, Email = e.Users.Email, FirstName = e.Users.FirstName, LastName = e.Users.LastName, Image = e.Users.Image, Color=e.Users.Color },
                        Senders = e.SenderId == null ? null : new { Id = e.Senders.Id, Email = e.Senders.Email, FirstName = e.Senders.FirstName, LastName = e.Senders.LastName, Image = e.Senders.Image, Color=e.Senders.Color },
                        Medias = e.Medias == null ? null : e.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelId == e.Id && w.RelType == "T")
                    });//.OrderByDescending(e => e.Id).ToList();
                IOrderedQueryable filtered = null;
                if(string.IsNullOrEmpty(TicketNumber)){ 
                     filtered = allTicket.OrderByDescending(e => e.Id);
                }
                else{
                     filtered = allTicket.Where(w => w.TicketNumber == TicketNumber).OrderByDescending(e => e.Id);
                }
                return Ok(filtered);
            
          // return Ok(allTicket);
        }

         [HttpPost("client")]
         [Authorize]
        public IActionResult ClientCreate([FromForm]Ticket request,[FromForm] string sender, [FromForm]IList<IFormFile> file, [FromQuery] string code )
        {
           using (var transaction =  _context.Database.BeginTransaction())
           {
             try
              {
                Sender requestSender = new Sender();
                requestSender = JsonConvert.DeserializeObject<Sender>(sender);
                
                if(!string.IsNullOrEmpty(code)){
                    var cCode = _context.Verification.Where(w => w.Code == code && w.Email == requestSender.Email).FirstOrDefault();
                    if(cCode != null){
                        if(cCode.ExpiredAt < DateTime.Now){ return BadRequest("Verification code expired !"); }
                        else{ 
                            cCode.Verified = true; 
                            _context.SaveChanges();    
                        }
                    }
                    else{ return BadRequest("Verification code not found "); }
                }
              

                Ticket ticketEntity = new Ticket()
                {   
                    TicketNumber = GenerateTicketNumber(),
                    TicketType = request.TicketType,
                    Subject = request.Subject,
                    Comment = request.Comment,
                    AppId =  request.AppId,
                    ModuleId = request.ModuleId,
                    StatId= 1,
                    CreatedBy = request.CreatedBy,
                    CreatedAt = DateTime.Now,
                };

                var exitingsSender = _context.Senders.AsNoTracking().Where(e => e.Email == requestSender.Email).FirstOrDefault();
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
                    //add client user if not exist
                    exitingsSender = _context.Senders.AsNoTracking().Where(e => e.Email == requestSender.Email).FirstOrDefault();
                    ticketEntity.SenderId = exitingsSender.Id;
                }
                else{
                    ticketEntity.SenderId = exitingsSender.Id;
                }
            
                _context.Tickets.Add(ticketEntity);
                _context.SaveChanges();
                //save ticket

                var newTicket = _context.Tickets.AsNoTracking().Where(w => w.TicketNumber == ticketEntity.TicketNumber).FirstOrDefault();
                var app = _context.Apps.Where(w => w.Id == newTicket.AppId).FirstOrDefault();
                var appModule = _context.Modules.Where(w => w.Id == newTicket.ModuleId).FirstOrDefault();

                foreach(var f in file){
                    Media uploadedFile = _fileUtil.FileUpload(f, "Tickets");
                    _context.Medias.Add(new Media{
                        FileName = "Tickets/"+uploadedFile.FileName,
                        FileType = uploadedFile.FileType,
                        RelId = newTicket.Id,
                        TicketId = newTicket.Id,
                        RelType = "T"
                    });
                }
                _context.SaveChanges();
                //save attachments

                //ASSIGN AND MAIL CONFIG
                List<User> listManager = new List<User>(); 
                List<string> listMailTo = new List<string>();
                listManager = (from u in _context.Users 
                                join ur in _context.UserRoles on u.Id equals ur.UserId
                                join ud in _context.UserDepts on u.Id equals ud.UserId
                                where (ur.RoleId == 2 && ud.DepartmentId == 2)
                                select u 
                                ).ToList();

                foreach (var mg in listManager) {
                    _context.TicketAssigns.Add(new TicketAssign{
                        TicketId = newTicket.Id,
                        UserId = mg.Id,
                        UserAt = DateTime.Now,
                        AssignType= "M",
                        Viewed = false
                    });
                    listMailTo.Add(mg.Email);
                }

                _mailUtil.SendEmailAsync(
                    new MailType {
                        ToEmail=listMailTo,
                        Subject= "New Ticket Number " + newTicket.TicketNumber,
                        Title= request.Subject,
                        Body= request.Comment,
                        TicketFrom= requestSender.Email,
                        TicketApp= app.Name,
                        TicketModule=appModule.Name,
                        Attachments = new List<IFormFile>(file),
                        HomeSite = _config.GetSection("HomeSite").Value,
                        ButtonLink =  _config.GetSection("HomeSite").Value + "admin/ticket?tid="+ newTicket.Id +"&open=true",
                    }
                );
                
                List<string> listMailToSender = new List<string>();
                listMailToSender.Add(requestSender.Email);
                _mailUtil.SendEmailAsync(
                    new MailType {
                        ToEmail=listMailToSender,
                        Subject= "New Ticket Number " + newTicket.TicketNumber,
                        Title= request.Subject,
                        Body= request.Comment,
                        TicketFrom= requestSender.Email,
                        TicketApp= app.Name,
                        TicketModule=appModule.Name,
                        Attachments = new List<IFormFile>(file),
                        HomeSite = _config.GetSection("HomeSite").Value,
                        ButtonLink = _config.GetSection("HomeSite").Value + "ticket?tid="+ newTicket.Id +"&open=true",
                    }
                );
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

        [HttpPost]
        [Authorize]
        [Route("client/post-comment")]
        public IActionResult PostClientTicketDetail( [FromForm]TicketDetail request, [FromForm] string sender, [FromForm]IList<IFormFile> file, [FromForm] string ticketAssign )
        {
            using (var transaction =  _context.Database.BeginTransaction())
            {
                try {
                    var cTicket =  _context.Tickets .Where(e => e.Id == request.TicketId) .FirstOrDefault();
                    if (cTicket == null) { return NotFound("ticket Not Found !"); }
                    
                    Sender requestSender = JsonConvert.DeserializeObject<Sender>(sender);
                    IList<TicketAssign> requestAssign =  JsonConvert.DeserializeObject<IList<TicketAssign>>(ticketAssign);
                    var app = _context.Apps.Where(w => w.Id == cTicket.AppId).FirstOrDefault();
                    var appModule = _context.Modules.Where(w => w.Id == cTicket.ModuleId).FirstOrDefault();

                    TicketDetail td = new TicketDetail(){
                        TicketId = request.TicketId,
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
                            TicketDetailId = newTd.Id,
                            RelType = "TD"
                        });
                    }
                    
                    List<string> listMailTo = new List<string>();
                    foreach (var assign in requestAssign)
                    {
                          listMailTo.Add(assign.Users.Email);
                    }

                    List<string> listMailToEx = new List<string>();
                    _mailUtil.SendEmailPostCommentAsync(
                        new MailType {
                            ToEmail= listMailTo,
                            Subject= "New Comment on Ticket Number " + cTicket.TicketNumber,
                            TicketNumber=cTicket.TicketNumber,
                            Title= cTicket.Subject,
                            Body= request.Comment,
                            TicketFrom= requestSender.Email,
                            TicketApp= app.Name,
                            TicketModule=appModule.Name,
                            Attachments = new List<IFormFile>(file),
                            UserFullName = requestSender.FirstName + " " + requestSender.LastName,
                            HomeSite = _config.GetSection("HomeSite").Value,
                            ButtonLink = _config.GetSection("HomeSite").Value + "admin/ticket?tid="+ cTicket.Id +"&open=true",
                        }
                    );

                    _context.SaveChanges();
                    transaction.Commit();

                    return Ok();
                }
                catch (System.Exception e) {
                    transaction.Rollback();
                    return BadRequest(e.Message);
                }
            }
        }

        [Authorize]
        [HttpPost]
        [Route("client/create-ticket-verify")]
        public IActionResult verifyCreateTicketClientMailCode([FromBody] Verification request){ //CLIENT MAIL CODE
           
            var cClient = _context.ClientDetails.Where(w => request.Email.Contains(w.Domain)).FirstOrDefault();
            if(cClient == null) {
                return BadRequest("Sorry, This email has no access !");
            }
             else
            {
                string vCode = "";
                for (int i = 0; i < 3; i++)
                {
                    vCode = _mailUtil.GenerateRandom4Code();
                    var verified = _context.Verification.Where(w => w.Code == vCode).FirstOrDefault();
                    if(verified == null){ break; } 
                }
                _context.Verification.Add(new Verification {
                    Code = vCode,
                    Verified = false,
                    ExpiredAt = DateTime.Now.AddMinutes(30),
                    Email = request.Email,
                    CreatedAt = DateTime.Now,
                    Desc = "Create Ticket Client"
                });
                
                _context.SaveChanges();
                List<string> listMailToSender = new List<string>();
                listMailToSender.Add(request.Email);
                _mailUtil.SendEmailVerificationCodeAsync(
                    new MailType {
                        ToEmail=listMailToSender,
                        Subject= "Epsylon Ticketing Veification Code",
                        Title= "Here is your confirmation code :",
                        Body= "All you have to do is copy the code and paste it to your form to complate the email verification process",
                        HomeSite = _config.GetSection("HomeSite").Value,
                        VerificationCode=vCode,
                    }
                );

                return Ok();
            }
       
        }




        [HttpGet("client/open-ticket-verify") ]
        [Authorize]
        public IActionResult VerifyOpenTicket ([FromHeader] string Authorization, [FromHeader] string TicketToken, [FromQuery] string code ){
            if(!string.IsNullOrEmpty(TicketToken)){ 
                var tokenPlain = AncDecUtil.DecryptString(TicketToken.Replace("Bearer ", ""), "EPSYLONHOME2021$", true);
                var property = new { CreatedBy = "", TicketNumber= "", SecurityCode=""  };
                var tokenJson = JsonConvert.DeserializeAnonymousType(tokenPlain, property);
                if(tokenJson != null && tokenJson.SecurityCode == code.ToString()){
                    return Ok(true);
                }
            }
            return BadRequest("Incorrect verification code ");
        }






    }
}


