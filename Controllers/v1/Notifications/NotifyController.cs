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
using TicketingApi.Models.v1.Notifications;
using TicketingApi.Models.v1.Users;
using System.IdentityModel.Tokens.Jwt;
using TicketingApi.Entities;
using TicketingApi.Utils;
using Newtonsoft.Json;
using System.Security.Claims;


namespace TicketingApi.Controllers.v1.Notifications
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class NotifyController: ControllerBase
    {
        private readonly AppDBContext  _context;

        public NotifyController(AppDBContext context)
        {
            _context = context; 
        }


        [HttpPost("register-notify")]
        [Authorize]
        public IActionResult RegisterUserNotif([FromHeader] string Authorization, [FromBody]NotifRegister request){
            
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                _context.NotifRegisters.Add(new NotifRegister{
                    UserId = request.UserId,
                    FcmToken = request.FcmToken,
                    Os = request.Os,
                    OsVersion = request.OsVersion,
                    Browser = request.Browser,
                    BrowserVersion = request.BrowserVersion,
                    CreatedAt = DateTime.Now
                });
                _context.SaveChanges();
                transaction.Commit();
                return Ok();
            }
            catch (System.Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetNotifys([FromHeader] string Authorization)
        {
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
          var Email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;
          var cUser = _context.Users.Where(w => w.Email == Email).FirstOrDefault();
          var allNotify = _context.NotifRegisters.AsNoTracking().Where(w => w.UserId == cUser.Id)
                            .Include(i => i.Users)
                            .Include(i => i.Notifs)
                            .Select(s => new{
                                s.Id, s.UserId, s.SenderId, s.Os, s.OsVersion, s.Browser, s.BrowserVersion,
                                s.FcmToken, s.CreatedAt, s.UpdatedAt,
                                Users = s.Users == null ? null : new { UserId = s.Users.Id, s.Users.Email, FullName = s.Users.FirstName + " " + s.Users.LastName, s.Users.Image, s.Users.Color },
                                Notifs = s.Notifs == null ? null :  s.Notifs.Select(n => new { n.Id, n.NtfType, n.Title, n.Message, n.Viewed, n.NtfData, n.CreatedAt, n.UpdatedAt }).Where(w => w.Viewed == false)
                            });
          return Ok(allNotify);
        }


        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetNotifyById(int id)
        {
            var notif = _context.Notifs.AsNoTracking()
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
            if(notif != null){
                return Ok(notif);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]Notif request)
        {
            var exitingsNotify = _context.Notifs.FirstOrDefault(e => e.Id == request.Id);
            if(exitingsNotify != null ) {
                return BadRequest("Email already in use");
            }
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                Notif notifEntity = new Notif()
                {   
                    Title = request.Title,
                    CreatedAt = DateTime.Now
                };
                _context.Notifs.Add(notifEntity);
                _context.SaveChanges();
                transaction.Commit();
                return Ok();
            }
            catch (System.Exception e)
            {
               return BadRequest(e.Message);
            }                
        }

        [HttpPost("{id}")]
        public IActionResult PutNotify(int id,[FromForm]Notif request)
        {
            try
            {
                var notifExist =  _context.Notifs.Where(e => e.Id == id).FirstOrDefault();
                if (notifExist == null)
                {
                    return NotFound("Notify Not Found !");
                }
                var transaction =  _context.Database.BeginTransaction();
                notifExist.Viewed = true;
                notifExist.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                transaction.Commit();
                return Ok(notifExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteNotifyById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var notifExist =  _context.Notifs
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                _context.Notifs.Remove(notifExist);
                _context.SaveChanges();
                transaction.Commit();
                return Ok();
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}