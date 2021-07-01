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
        private readonly IWebHostEnvironment _env; 
        public TicketController(AppDBContext context, IFileUtil fileUtil, IWebHostEnvironment env )
        {
            _context = context; 
            _fileUtil = fileUtil;
            _env = env;   
        }

        public static string GenerateTicketNumber(){
            return "";
        }


        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
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
                        .Include(mda => mda.Medias.Where(item => item.RelType == "T"))
                    .Select(e => new {
                        e.Id, e.TicketNumber, e.Subject, e.Comment, e.SolvedBy, e.SolvedAt, e.RejectedBy, e.RejectedReason, e.RejectedAt, e.CreatedAt, e.UpdatedAt,
                        TicketDetails = e.TicketDetails.Select(t => new { t.Id, t.Comment, t.Flag, t.CreatedAt, t.UpdatedAt, Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image } }),
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
                                 }), 
                                 t.TeamAt, 
                                 Users = t.Users == null ? null : new { UserId = t.Users.Id, t.Users.Email, FullName = t.Users.FirstName + " " + t.Users.LastName, t.Users.Image }, 
                                 t.UserAt, 
                                 t.Viewed, 
                                 t.ViewedAt
                             }
                        }),
                        e.Status, e.Apps, e.Modules,
                        Senders = new { e.Senders.Id, e.Senders.Email, FullName = e.Senders.FirstName + " " + e.Senders.LastName, e.Senders.Image, e.Senders.LoginStatus },
                        e.Medias
                    });
           return Ok(allTicket);
        }

        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetTicketById(int id)
        {
            var ticket = _context.Tickets.AsNoTracking()
                        .Where(e => e.Id == id)
                        .Include(sd => sd.Senders)
                        .Include(st => st.Status)
                        .Include(td => td.TicketDetails)
                        .Include(ta => ta.TicketAssigns)
                        .Include(ap => ap.Apps)
                        .Include(md => md.Modules)        
                        .FirstOrDefault();
                       
            if(ticket != null){
                return Ok(ticket);
            }
            return NotFound();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]Ticket request)
        {
            var exitingsTicket = _context.Tickets.FirstOrDefault(e => e.TicketNumber == request.TicketNumber);
            if(exitingsTicket != null ) {
                return BadRequest("Email already in use");
            }
            
            var transaction =  _context.Database.BeginTransaction();
               
            try
            {
                var salt =  CryptoUtil.GenerateSalt();
                Ticket ticketEntity = new Ticket()
                {   
                    TicketNumber = GenerateTicketNumber(),
                    Subject = request.Subject,
                    Comment = request.Comment,
                    AppId =  request.AppId,
                    ModuleId = request.ModuleId,
                    SenderId = request.SenderId,
                    StatId= 1,
                    CreatedAt = DateTime.Now
                };

                _context.Tickets.Add(ticketEntity);
                _context.SaveChanges();
                transaction.Commit();
                return Ok(ticketEntity);
            }
            catch (System.Exception e)
            {
               return BadRequest(e.Message);
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