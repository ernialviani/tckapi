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
        public TicketController(AppDBContext context, IFileUtil fileUtil )
        {
            _context = context; 
            _fileUtil = fileUtil;
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
                        .Include(td => td.TicketDetails)
                        .Include(ta => ta.TicketAssigns)
                        .Include(ap => ap.Apps)
                        .Include(md => md.Modules)
                        .Include(mda => mda.Medias.Where(item => item.RelType == "T"));
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