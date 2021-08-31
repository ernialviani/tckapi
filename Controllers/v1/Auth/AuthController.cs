using System.Net;
using System.Net.Mime;
using System;
using System.IO;
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
using Microsoft.AspNetCore.Hosting;
using TicketingApi.Utils;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace TicketingApi.Controllers.v1.Authentication
{
    [ApiController]
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
         private readonly IConfiguration _configuration;
         private readonly AppDBContext  _context;
        private readonly IWebHostEnvironment _env;

        public AuthController(IConfiguration configuration, AppDBContext context, IWebHostEnvironment env){
            _configuration = configuration;
            _context = context;
            _env = env;
        }    

        [AllowAnonymous]
        [HttpGet]
        [Route("test")]
        public IActionResult Tester(){
            return Ok("test success");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("admin/login")]
        public IActionResult loginAdmin([FromBody] User user){
            try
            {
                var existingUser = _context.Users.Where(v => v.Email.Equals(user.Email) && v.Deleted == false)
                                .AsNoTracking()
                                .Include(ur => ur.UserRoles).ThenInclude(r => r.Roles)
                                .Include(ud => ud.UserDepts).ThenInclude(d => d.Departments)
                                .FirstOrDefault();
                var userImage = "";
                                
                if (existingUser != null)
                {
                    var isPasswordVerified = CryptoUtil.VerifyPassword(user.Password, existingUser.Salt, existingUser.Password);
                    var firstRole = existingUser.UserRoles.OrderBy(o => o.RoleId).FirstOrDefault();

                    
                    if (isPasswordVerified)
                    {
                        var claimList = new List<Claim>();
                        claimList.Add(new Claim(ClaimTypes.Email, existingUser.Email));
                        claimList.Add(new Claim(ClaimTypes.Role, firstRole.Roles.Id.ToString()));
                        // claimList.Add(new Claim("Department", ))
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var expireDate = DateTime.UtcNow.AddDays(30);
                        var timeStamp = DateUtil.ConvertToTimeStamp(expireDate);
                        var token = new JwtSecurityToken(
                            claims: claimList,
                            notBefore: DateTime.UtcNow,
                            expires: expireDate,
                            signingCredentials: creds);

                        if(String.IsNullOrEmpty(existingUser.Image) == false){
                            var uploadPath = Path.Combine(_env.ContentRootPath, "Medias/");
                            var filePath = Path.Combine(uploadPath, existingUser.Image);
                            byte[] b = System.IO.File.ReadAllBytes(filePath);
                                userImage = "data:image/png;base64," + Convert.ToBase64String(b);
                        }

                        return Ok(new {
                            token          = new JwtSecurityTokenHandler().WriteToken(token),
                            expireDate      = timeStamp,
                            Id             = existingUser.Id,
                            FirstName      = existingUser.FirstName,
                            LastName       = existingUser.LastName,
                            Email          = existingUser.Email,
                            role           = existingUser.UserRoles,
                            dept           = existingUser.UserDepts,
                            Image          = userImage,
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
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
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
                    // claimList.Add(new Claim(ClaimTypes.Role, "role"));
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
                        Color          = existingSender.Color
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
                sender.FirstName = request.FirstName;
                sender.LastName = request.LastName;
                sender.LoginStatus = true;
                sender.Color = request.Color;
                _context.Senders.Add(sender);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest("Email is already in use");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("admin/authcheck")]
        public IActionResult AuthorizationCheck([FromHeader] string Authorization){
            var userImage = "";
            var bearer = Authorization.Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadJwtToken(bearer);
            var email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            var existingUser = _context.Users.Where(v => v.Email.Equals(email) && !v.Deleted)
                                .AsNoTracking()
                                .Include(ur => ur.UserRoles).ThenInclude(r => r.Roles)
                                .Include(ud => ud.UserDepts).ThenInclude(d => d.Departments)
                                .FirstOrDefault();

            if(existingUser != null){
                  if(existingUser.Deleted) {  return BadRequest("User has been deleted !"); }
                  if(String.IsNullOrEmpty(existingUser.Image) == false ){
                    var uploadPath = Path.Combine(_env.ContentRootPath, "Medias/");
                    var filePath = Path.Combine(uploadPath, existingUser.Image);
                    byte[] b = System.IO.File.ReadAllBytes(filePath);
                    userImage = "data:image/png;base64," + Convert.ToBase64String(b);
                  }
                  return Ok(new {
                        Id             = existingUser.Id,
                        FirstName      = existingUser.FirstName,
                        LastName       = existingUser.LastName,
                        Email          = existingUser.Email,
                        role           = existingUser.UserRoles,
                        dept           = existingUser.UserDepts,
                        Image          = userImage,
                    });
            }
            return NotFound();
        }



    }

}