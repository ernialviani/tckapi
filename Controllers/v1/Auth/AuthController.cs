using System.Net.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using TicketingApi.DBContexts;
using TicketingApi.Models.v1.Users;

using TicketingApi.Utils;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace TicketingApi.Controllers.v1.Authentication
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
         private readonly IConfiguration _configuration;
         private readonly AppDBContext  _context;

        public AuthController(IConfiguration configuration, AppDBContext context){
            _configuration = configuration;
            _context = context;
        }    

        [AllowAnonymous]
        [HttpPost]
        [Route("admin/login")]
        public IActionResult loginAdmin([FromBody] User user){

            var existingUser = _context.Users.Where(v => v.Email.Equals(user.Email))
                             .AsNoTracking()
                             .Include(ur => ur.UserRoles).ThenInclude(r => r.Roles)
                             .Include(ud => ud.UserDepts).ThenInclude(d => d.Departments)
                             .FirstOrDefault();
                             
            if (existingUser != null)
            {
                  var isPasswordVerified = CryptoUtil.VerifyPassword(user.Password, existingUser.Salt, existingUser.Password);
                if (isPasswordVerified)
                {
                    var claimList = new List<Claim>();
                    claimList.Add(new Claim(ClaimTypes.Name, existingUser.Email));
                    claimList.Add(new Claim(ClaimTypes.Role, "role"));
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expireDate = DateTime.UtcNow.AddDays(1);
                    var timeStamp = DateUtil.ConvertToTimeStamp(expireDate);
                    var token = new JwtSecurityToken(
                        claims: claimList,
                        notBefore: DateTime.UtcNow,
                        expires: expireDate,
                        signingCredentials: creds);

                    return Ok(new {
                        token          = new JwtSecurityTokenHandler().WriteToken(token),
                        expireDat      = timeStamp,
                        Id             = existingUser.Id,
                        FirstName      = existingUser.FirstName,
                        LastName       = existingUser.LastName,
                        Email          = existingUser.Email,
                        role           = existingUser.UserRoles,
                        dept           = existingUser.UserDepts,
                        Image          = existingUser.Image,
                    });
                }
                else {
                    return BadRequest("Wrong Password");
                }
            }
            else {
                return BadRequest("User Not Found");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult login([FromBody] Sender sender){

            var existingSender = _context.Senders.Where(v => v.Email.Equals(sender.Email))
                                .AsNoTracking()
                                .FirstOrDefault();
                                
            if (existingSender != null)
            {
                var isPasswordVerified = CryptoUtil.VerifyPassword(sender.Password, existingSender.Salt, existingSender.Password);
                if (isPasswordVerified)
                {
                    var claimList = new List<Claim>();
                    claimList.Add(new Claim(ClaimTypes.Name, existingSender.Email));
                    claimList.Add(new Claim(ClaimTypes.Role, "role"));
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var expireDate = DateTime.UtcNow.AddDays(1);
                    var timeStamp = DateUtil.ConvertToTimeStamp(expireDate);
                    var token = new JwtSecurityToken(
                        claims: claimList,
                        notBefore: DateTime.UtcNow,
                        expires: expireDate,
                        signingCredentials: creds);

                    return Ok(new {
                        token          = new JwtSecurityTokenHandler().WriteToken(token),
                        expireDat      = timeStamp,
                        Id             = existingSender.Id,
                        FirstName      = existingSender.FirstName,
                        LastName       = existingSender.LastName,
                        Email          = existingSender.Email,
                        Image          = existingSender.Image,
                    });
                }
                else {
                    return BadRequest("Wrong Password");
                }
            }
            else {
                return BadRequest("User Not Found");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] Sender request){

            if (!_context.Senders.Any(x => x.Email == request.Email))
            {
                var email = request.Email;
                var salt = CryptoUtil.GenerateSalt();
                var password = request.Password;
                var hashedPassword = CryptoUtil.HashMultiple(password, salt);
                var sender = new Sender();
                sender.Email = email;
                sender.Salt = salt;
                sender.Password = hashedPassword;
                _context.Senders.Add(sender);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("Email is already in use");
            }
        }



    }

}