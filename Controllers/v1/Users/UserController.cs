using System;
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

namespace TicketingApi.Controllers.v1.Users
{
    [ApiController]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
       //  private readonly IConfiguration _configuration;
         private readonly AppDBContext  _context;
        private readonly IFileUtil _fileUtil;
        public UserController(AppDBContext context, IFileUtil fileUtil )
        {
            _context = context; 
            _fileUtil = fileUtil;
        }

        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public IActionResult GetUsers([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allUser = _context.Users.AsNoTracking()
                        .Include(ur => ur.UserRoles).ThenInclude(r => r.Roles)
                        .Include(ud => ud.UserDepts).ThenInclude(d => d.Departments);
           return Ok(allUser);
        }


        [HttpGet("{email}")]
        public IActionResult GetUserById(string email)
        {
            return null;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]User model)
        {
                  var salt =  CryptoUtil.GenerateSalt();
                User user = new User()
                {   
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password =  CryptoUtil.HashMultiple(model.Password, salt),
                    Salt = salt,
                    CreatedAt = DateTime.Now
                };
            if (model.File != null)
            {
                var uploadedImage = _fileUtil.Upload(model.File, "Medias/Users");
                user.Image = uploadedImage;
            }
            _context.Add(user);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User model)
        {

            return NoContent();
        }

        //avatar update
        [HttpPut("{id}")]
        [Route("admin/avatar-update/{id}")]
        public IActionResult PutAvatar(int id,[FromForm] User model)
        {
            var rec = _context.Users.FirstOrDefault(x => x.Id == id);
            if(model.File != null){
                var uploadedImage = _fileUtil.Upload(model.File, "Medias/Users");
                rec.Image = uploadedImage;
            }
            _context.SaveChanges();
            return Ok(rec);
        }

        [HttpDelete("{id}")]
        public ActionResult<User> DeleteUserById(int id)
        {
            return null;
        }
    }
}