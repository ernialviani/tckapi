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
using TicketingApi.Models.v1.Users;
using System.IdentityModel.Tokens.Jwt;
using TicketingApi.Entities;
using TicketingApi.Utils;
using Newtonsoft.Json;

namespace TicketingApi.Controllers.v1.Users
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class SenderController: ControllerBase
    {
 
       //  private readonly IConfiguration _configuration;
        private readonly AppDBContext  _context;
        private readonly IFileUtil _fileUtil;
        public SenderController(AppDBContext context, IFileUtil fileUtil )
        {
            _context = context; 
            _fileUtil = fileUtil;
        }

        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public IActionResult GetSenders([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allSender = _context.Senders.AsNoTracking();
           return Ok(allSender);
        }

        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetSenderById(int id)
        {
            var sender = _context.Senders.AsNoTracking()
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                       
            if(sender != null){
                return Ok(sender);
            }
            return NotFound();
            
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]Sender request)
        {
            var exitingsSender = _context.Senders.FirstOrDefault(e => e.Email == request.Email);
            if(exitingsSender != null ) {
                return BadRequest("Email already in use");
            }
            
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                Sender senderEntity = new Sender()
                {   
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    CreatedAt = DateTime.Now
                };

                if(request.Password != null){
                    var salt =  CryptoUtil.GenerateSalt();
                    senderEntity.Password =  CryptoUtil.HashMultiple(request.Password, salt);
                    senderEntity.Salt = salt;
                    senderEntity.LoginStatus = false;
                }

                if (request.File != null)
                {
                    var uploadedImage = _fileUtil.AvatarUpload(request.File, "Senders");
                    senderEntity.Image = uploadedImage;
                }

                _context.Senders.Add(senderEntity);
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
        public IActionResult PutSender(int id,[FromForm]Sender request)
        {
            try
            {
                var senderExist =  _context.Senders .Where(e => e.Id == id) .FirstOrDefault();
                
                if (senderExist == null) return NotFound("Sender Not Found !");

                var transaction =  _context.Database.BeginTransaction();
            
                senderExist.FirstName = request.FirstName;
                senderExist.LastName = request.LastName;
                senderExist.Email = request.Email;
                senderExist.UpdatedAt = DateTime.Now;

                if (string.IsNullOrEmpty(request.Password) == false)
                {
                     var salt =  CryptoUtil.GenerateSalt();
                     senderExist.Salt = salt;
                     senderExist.Password =  CryptoUtil.HashMultiple(request.Password, salt);
                }
              
                if (request.File != null)
                {
                  var isRemovedImage = _fileUtil.Remove(senderExist.Image);
                  if(isRemovedImage){
                    var uploadedImage = _fileUtil.AvatarUpload(request.File, "Senders");
                    senderExist.Image = uploadedImage;  
                  }
                }
           
                _context.SaveChanges();
    
                transaction.Commit();

                return Ok(senderExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //avatar update
        [HttpPut("{id}")]
        [Route("admin/avatar-update/{id}")]
        public IActionResult PutAvatar(int id,[FromForm] Sender model)
        {
            var rec = _context.Senders.FirstOrDefault(x => x.Id == id);
            if(model.File != null){
                var uploadedImage = _fileUtil.AvatarUpload(model.File, "Senders");
                rec.Image = uploadedImage;
            }
            _context.SaveChanges();
            return Ok(rec);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteSenderById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var senderExist =  _context.Senders .Where(e => e.Id == id) .FirstOrDefault();
                var isRemovedImage = false;
                if(String.IsNullOrEmpty(senderExist.Image) == false){
                    isRemovedImage = _fileUtil.Remove("Senders/"+senderExist.Image);
                }
               
                _context.Senders.Remove(senderExist);
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