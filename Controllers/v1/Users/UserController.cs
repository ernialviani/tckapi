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

namespace TicketingApi.Controllers.v1.Users
{
    [ApiController]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
       //  private readonly IConfiguration _configuration;
         private readonly AppDBContext  _context;
        public UserController(AppDBContext context)
        {
            _context = context; 
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

        [HttpPost("")]
        public ActionResult<User> PostUser(User model)
        {
            return null;
        }

        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User model)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<User> DeleteUserById(int id)
        {
            return null;
        }
    }
}