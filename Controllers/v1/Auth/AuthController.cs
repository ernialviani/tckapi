using System.Drawing;
using System.Net;
using System.Net.Mime;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

using System.Threading.Tasks;
using System.Net.Http.Headers; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using TicketingApi.DBContexts;
using TicketingApi.Models.v1.Users;
using TicketingApi.Models.v1.Misc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

using TicketingApi.Utils;
using System.Diagnostics;
using TicketingApi.Entities;
using Newtonsoft.Json;
 

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
        private readonly IMailUtil _mailUtil;

        public AuthController(IConfiguration configuration, AppDBContext context, IWebHostEnvironment env,  IMailUtil mailUtil){
            _configuration = configuration;
            _context = context;
            _env = env;
            _mailUtil = mailUtil;
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
                            Color          = existingUser.Color
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
                        Color          = existingUser.Color
                    });
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("client/reg-mail-verify")]
        public IActionResult verifyRegisterClientMailCode([FromBody] Verification request){ //CLIENT REGISTER

            var cSender = _context.Senders.Where(v => v.Email.Equals(request.Email)).AsNoTracking().FirstOrDefault();
            if (cSender != null)
            {
                if(cSender.LoginStatus == true){ return BadRequest("Email Already Exists !"); } 
                else {
                   
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
                        Desc = "Register Client"
                    });
                    
                    _context.SaveChanges();
                    List<string> listMailToSender = new List<string>();
                    listMailToSender.Add(cSender.Email);
                    _mailUtil.SendEmailVerificationCodeAsync(
                        new MailType {
                            ToEmail=listMailToSender,
                            Subject= "Epsylon Ticketing Veification Code",
                            Title= "Here is your confirmation code :",
                            Body= "All you have to do is copy the code and paste it to your form to complate the email verification process",
                            VerificationCode=vCode,
                        }
                    );

                    return Ok();
                }
            }
            else {
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
                        Desc = "Register Client"
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
                            VerificationCode= vCode,
                        }
                    );
                return Ok();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("client/register")]
        public IActionResult Register([FromBody] Sender request, [FromQuery] string code){

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var cCode = _context.Verification.Where(w => w.Code == code && w.Email == request.Email).FirstOrDefault();
                    if(cCode != null){
                        if(cCode.ExpiredAt < DateTime.Now){ return BadRequest("Verification code expired !"); }
                        else{ 
                            cCode.Verified = true; 
                            _context.SaveChanges();    
                        }
                    }
                    else{
                        return BadRequest("Verification code not found ");
                    }

                    var cSender = _context.Senders.Where(x => x.Email == request.Email).FirstOrDefault();
                    var cClient = _context.ClientDetails.Where(w => request.Email.Contains(w.Domain)).FirstOrDefault();
                    if(cClient == null) {
                        transaction.Rollback();
                        return BadRequest("Sorry, Sorry, no access for this email address !");
                    }
                    if (cSender == null)
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
                         transaction.Commit();
                        return Ok();
                    }
                    else
                    {
                        if( cSender.LoginStatus == false ){
                            var email = request.Email;
                            var salt = CryptoUtil.GenerateSalt();
                            var password = request.Password;
                            var hashedPassword = CryptoUtil.HashMultiple(password, salt);
                            cSender.Email = email;
                            cSender.Salt = salt;
                            cSender.Password = hashedPassword;
                            cSender.FirstName = request.FirstName;
                            cSender.LastName = request.LastName;
                            cSender.LoginStatus = true;
                            cSender.Color = request.Color;
                            _context.SaveChanges();
                            transaction.Commit();
                            return Ok();
                        }
                        else{
                            transaction.Rollback();
                          return BadRequest("Email is already in use");
                        }
                    }
                }
                catch (System.Exception e)
                {
                    transaction.Rollback();
                    return BadRequest(e.Message);
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("client/login")]
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
                    claimList.Add(new Claim(ClaimTypes.Email, existingSender.Email));
                    // claimList.Add(new Claim(ClaimTypes.Role, "role"));
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
        [Route("client/forgot-mail-verify")]
        public IActionResult verifyForgotPasswordClientMailCode([FromBody] Verification request){ //CLIENT FORGOT PASSWORD

            var cSender = _context.Senders.Where(v => v.Email.Equals(request.Email)).AsNoTracking().FirstOrDefault();
            if (cSender != null)
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
                    Desc = "Forgot Password Client"
                });
                
                _context.SaveChanges();
                List<string> listMailToSender = new List<string>();
                listMailToSender.Add(cSender.Email);
                _mailUtil.SendEmailVerificationCodeAsync(
                    new MailType {
                        ToEmail=listMailToSender,
                        Subject= "Epsylon Ticketing Veification Code",
                        Title= "Here is your confirmation code :",
                        Body= "All you have to do is copy the code and paste it to your form to complate the email verification process",
                        VerificationCode=vCode,
                    }
                );

                return Ok();
            }
            else {
                 return BadRequest("Email Not Found !"); 
            }
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("client/forgot-password")]
        public IActionResult ClientForgotPassword([FromBody] Sender request, [FromQuery] string code){

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var cCode = _context.Verification.Where(w => w.Code == code && w.Email == request.Email).FirstOrDefault();
                    if(cCode != null){
                        if(cCode.ExpiredAt < DateTime.Now){ return BadRequest("Verification code expired !"); }
                        else{ 
                            cCode.Verified = true; 
                            _context.SaveChanges();    
                        }
                    }
                    else{
                        return BadRequest("Verification code not found ");
                    }

                    var cSender = _context.Senders.Where(x => x.Email == request.Email).FirstOrDefault();
           
                    if (cSender == null)
                    {
                          transaction.Rollback();
                         return BadRequest("Email Not Found !");
                    }
                    else
                    {
                        var salt = CryptoUtil.GenerateSalt();
                        var hashedPassword = CryptoUtil.HashMultiple(request.Password, salt);
                        cSender.Salt = salt;
                        cSender.Password = hashedPassword;
                        _context.SaveChanges();
                        transaction.Commit();
                        return Ok();
                    }
                }
                catch (System.Exception e)
                {
                    transaction.Rollback();
                    return BadRequest(e.Message);
                }
            }
        }


        [Authorize]
        [HttpGet]
        [Route("client/authcheck")]
        public IActionResult AuthorizationClientCheck([FromHeader] string Authorization){
            var clientImage = "";
            var bearer = Authorization.Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadJwtToken(bearer);
            var email = token.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            var existingClient = _context.Senders.Where(v => v.Email.Equals(email)).AsNoTracking().FirstOrDefault();

            if(existingClient != null){
                  if(String.IsNullOrEmpty(existingClient.Image) == false ){
                    var uploadPath = Path.Combine(_env.ContentRootPath, "Medias/");
                    var filePath = Path.Combine(uploadPath, existingClient.Image);
                    byte[] b = System.IO.File.ReadAllBytes(filePath);
                    clientImage = "data:image/png;base64," + Convert.ToBase64String(b);
                  }
                  return Ok(new {
                        Id             = existingClient.Id,
                        FirstName      = existingClient.FirstName,
                        LastName       = existingClient.LastName,
                        Email          = existingClient.Email,
                        Image          = clientImage,
                        Color          = existingClient.Color
                    });
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("client/free-token")]
        public IActionResult GetFreeToken([FromHeader] string Authorization){
            if(string.IsNullOrEmpty(Authorization)){
              var cookie = HttpContext.Request.Cookies;  
              var jsonString =  JsonConvert.SerializeObject(new {
                value = "epsylon-free-token",
                key = "epsylon$",
                expiredAt = DateTime.Now.AddHours(1).ToString()
              });
              return Ok(AncDecUtil.Encrypt( "[" + jsonString +"]", "EPSYLONHOME2021$"));
            }
            return Ok();
        }

      

    }

}