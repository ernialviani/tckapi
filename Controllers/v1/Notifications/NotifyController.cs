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
                                Notifs = s.Notifs == null ? null :  s.Notifs.Select(n => new { n.Id, n.NotifRegisterId, n.NtfType, n.Title, n.Message, n.Viewed, n.Link, n.NtfData, n.CreatedAt, n.UpdatedAt }).Where(w => w.Viewed == false)
                            }).OrderByDescending(o => o.Id);
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

        [HttpPut]
        [Authorize]
        public IActionResult PutNotify([FromBody]Notif request, [FromHeader] string Authorization )
        {
            var transaction = _context.Database.BeginTransaction();
            var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
            var Email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            var cUser = _context.Users.Where(w => w.Email == Email).FirstOrDefault();
            var allRegistered = _context.NotifRegisters.Where(w => w.UserId == cUser.Id).ToList();
            try
            {
                var allNotifs = _context.Notifs.AsEnumerable().Where(w => allRegistered.Any(a => a.Id == w.NotifRegisterId)).ToList();
                foreach (var notif in allNotifs)
                {
                    if(notif.Title == request.Title && notif.NtfType == request.NtfType){
                        notif.Viewed = true;
                        notif.UpdatedAt = DateTime.Now;
                    }
                }
                _context.SaveChanges();
                transaction.Commit();
                return Ok();
            }
            catch (System.Exception e)
            {
                transaction.Rollback();
                return BadRequest(e.Message);
            }
        }

        
        [HttpPost("mark-all-read")]
        [Authorize]
        public IActionResult MarkAllRead([FromHeader] string Authorization)
        {
            var transaction = _context.Database.BeginTransaction();
            var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
            var Email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            var cUser = _context.Users.Where(w => w.Email == Email).FirstOrDefault();
            var allRegistered = _context.NotifRegisters.Where(w => w.UserId == cUser.Id);
            try
            {
                var allNotifs = _context.Notifs.Where(w => allRegistered.Any(a => a.Id == w.Id));
                foreach (var notif in allNotifs)
                {
                    notif.Viewed = true;
                    notif.UpdatedAt = DateTime.Now;
                }
                _context.SaveChanges();
                transaction.Commit();
                return Ok();
            }
            catch (System.Exception)
            {
                transaction.Rollback();
                return BadRequest();
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