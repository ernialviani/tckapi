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
using Microsoft.AspNetCore.Http;

namespace TicketingApi.Controllers.v1.Users
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        [Authorize]
        public IActionResult GetUsers([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allUser = _context.Users.AsNoTracking()
                        .Include(ur => ur.UserRoles).ThenInclude(r => r.Roles)
                        .Include(ud => ud.UserDepts).ThenInclude(d => d.Departments);
           return Ok(allUser);
        }


        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.AsNoTracking()
                        .Where(e => e.Id == id)
                        .Include(ur => ur.UserRoles).ThenInclude(r => r.Roles)
                        .Include(ud => ud.UserDepts).ThenInclude(d => d.Departments)
                        .FirstOrDefault();
                       
            if(user != null){
                return Ok(user);
            }
            return NotFound();
            
        }

        [HttpPost]
        [Authorize]
        //, [FromForm] string UserRoles, [FromForm] string UserDepts
        //, [FromForm] List<UserRole> UserRoles, [FromForm] List<UserDept> UserDepts
        public IActionResult Create([FromForm]User request)
        {
            var exitingsUser = _context.Users.FirstOrDefault(e => e.Email == request.Email);
            if(exitingsUser != null ) {
                return BadRequest("Email already in use");
            }
            
            var transaction =  _context.Database.BeginTransaction();
               
            try
            {
                var salt =  CryptoUtil.GenerateSalt();
                User userEntity = new User()
                {   
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password =  CryptoUtil.HashMultiple(request.Password, salt),
                    Salt = salt,
                    CreatedAt = DateTime.Now
                };

                if (request.File != null)
                {
                    var uploadedImage = _fileUtil.AvatarUpload(request.File, "Users");
                    userEntity.Image = uploadedImage;
                }

                _context.Users.Add(userEntity);
                _context.SaveChanges();

                var currentUser  = _context.Users.FirstOrDefault(e => e.Email == request.Email);
                
                foreach (UserRole role in request.UserRoles){
                    UserRole userRoleEntity = new UserRole(){ RoleId = role.RoleId, UserId = currentUser.Id };
                    _context.UserRoles.Add(userRoleEntity);
                };  

                foreach (var dept in request.UserDepts) {
                    UserDept userDeptEntity = new UserDept(){ DepartmentId = dept.DepartmentId, UserId = currentUser.Id };
                    _context.UserDepts.Add(userDeptEntity);
                }

            //     ICollection<UserRole> UserRoleSerialize = JsonConvert.DeserializeObject<ICollection<UserRole>>(UserRoles);
            //     foreach (var role in UserRoleSerialize)
            //     {
            //         UserRole userRoleEntity = new UserRole(){ RoleId = role.RoleId, UserId = currentUser.Id };
            //         _context.UserRoles.Add(userRoleEntity);
            //     }            

            //  ICollection<UserDept> UserDeptSerialize = JsonConvert.DeserializeObject<ICollection<UserDept>>(UserDepts);
            //     foreach (var dept in UserDeptSerialize)
            //     {
            //         UserDept userDeptEntity = new UserDept(){ DepartmentId = dept.DepartmentId, UserId = currentUser.Id };
            //         _context.UserDepts.Add(userDeptEntity);
            //     }
       
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
        public IActionResult PutUser(int id,[FromForm]User request)
        {
            try
            {
                 var userExist =  _context.Users
                        .Where(e => e.Id == id)
                        .Include(ur => ur.UserRoles)
                        .Include(ud => ud.UserDepts)
                        .FirstOrDefault();
                
                if (userExist == null)
                {
                    return NotFound("User Not Found !");
                }

                var transaction =  _context.Database.BeginTransaction();
            
                userExist.FirstName = request.FirstName;
                userExist.LastName = request.LastName;
                userExist.Email = request.Email;
                userExist.UpdatedAt = DateTime.Now;
             

                if (string.IsNullOrEmpty(request.Password) == false)
                {
                     var salt =  CryptoUtil.GenerateSalt();
                     userExist.Salt = salt;
                     userExist.Password =  CryptoUtil.HashMultiple(request.Password, salt);
                }
              
                if (request.File != null)
                {
                  var isRemovedImage = _fileUtil.Remove(userExist.Image);
                  if(isRemovedImage){
                    var uploadedImage = _fileUtil.AvatarUpload(request.File, "Users");
                    userExist.Image = uploadedImage;  
                  }
                }
           
                _context.SaveChanges();
                
                userExist.UserRoles
                .Where(eur => !request.UserRoles.Any(mur => mur.RoleId == eur.RoleId))
                .ToList()
                .ForEach(eur => 
                    userExist.UserRoles.Remove(eur)
                );

                request.UserRoles
                .Where(mur => !userExist.UserRoles.Any(eur => eur.RoleId == mur.RoleId))
                .ToList()
                .ForEach(mur => userExist.UserRoles.Add(new UserRole { RoleId = mur.RoleId, UserId = mur.UserId}));

                userExist.UserDepts
                .Where(eur => !request.UserDepts.Any(mur => mur.DepartmentId == eur.DepartmentId))
                .ToList()
                .ForEach(eur => userExist.UserDepts.Remove(eur));

                request.UserDepts
                .Where(mur => !userExist.UserDepts.Any(eur => eur.DepartmentId == mur.DepartmentId))
                .ToList()
                .ForEach(mur => userExist.UserDepts.Add(new UserDept { DepartmentId = mur.DepartmentId, UserId = mur.UserId}));

                _context.SaveChanges();
                transaction.Commit();

                return Ok(userExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //avatar update
        [HttpPut("{id}")]
        [Route("admin/avatar-update/{id}")]
        public IActionResult PutAvatar(int id,[FromForm]List<IFormFile> file)
        {
            // var rec = _context.Users.FirstOrDefault(x => x.Id == id);
            // if(model.File != null){
            //     var uploadedImage = _fileUtil.AvatarUpload(model.File, "Users");
            //     rec.Image = uploadedImage;
            // }
            // _context.SaveChanges();
            // return Ok(rec);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteUserById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var userExist =  _context.Users
                        .Where(e => e.Id == id)
                        .Include(ur => ur.UserRoles).ThenInclude(r => r.Roles)
                        .Include(ud => ud.UserDepts).ThenInclude(d => d.Departments)
                        .FirstOrDefault();
                var isRemovedImage = false;
                if(String.IsNullOrEmpty(userExist.Image) == false){
                    isRemovedImage = _fileUtil.Remove("Users/"+userExist.Image);
                }
               
                foreach (var itemRole in userExist.UserRoles)
                {
                    _context.UserRoles.Remove(itemRole);
                }
                foreach (var itemDept in userExist.UserDepts)
                {
                    _context.UserDepts.Remove(itemDept);
                }

                _context.Users.Remove(userExist);
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