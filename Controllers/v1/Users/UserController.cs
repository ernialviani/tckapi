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
         private readonly IMailUtil _mailUtil;
        private readonly IConfiguration _config;
        public UserController(AppDBContext context, IFileUtil fileUtil, IMailUtil mailUtil, IConfiguration config  )
        {
            _context = context; 
            _fileUtil = fileUtil;
            _mailUtil = mailUtil;
            _config = config;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetUsers([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allUser = _context.Users.AsNoTracking().Where(wr => wr.Deleted == false)
                        .Include(ur => ur.UserRoles).ThenInclude(r => r.Roles)
                        .Include(ud => ud.UserDepts).ThenInclude(d => d.Departments)
                        .Include(i => i.Teams).ThenInclude(ti => ti.TeamMembers)
                        .Select(s => new {
                           s.Id,
                           s.FirstName,
                           s.LastName,
                           s.Email,
                           s.Image,
                           s.Color,
                           s.UserDepts,
                           s.UserRoles,
                           s.CreatedAt,
                           s.UpdatedAt,
                           Teams = s.Teams.Select(st => new { st.Id, st.Name, st.Desc, st.ManagerId, st.TeamMembers  }).Where(w => w.ManagerId.Equals(s.Id) || w.TeamMembers.Any(a => a.UserId == s.Id) )
                           //|| w.TeamMembers.Any(a => a.UserId == s.Id)
                        }).OrderByDescending(e => e.Id);
           return Ok(allUser);
        }


        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.AsNoTracking()
                        .Where(e => e.Id == id && e.Deleted == false)
                        .Include(ur => ur.UserRoles).ThenInclude(r => r.Roles)
                        .Include(ud => ud.UserDepts).ThenInclude(d => d.Departments)
                        .Include(i => i.Teams).ThenInclude(ti => ti.TeamMembers)
                        .Select(s => new {
                           s.Id,
                           s.FirstName,
                           s.LastName,
                           s.Email,
                           s.Image,
                           s.Color,
                           s.UserDepts,
                           s.UserRoles,
                           s.CreatedAt,
                           s.UpdatedAt,
                           Teams = s.Teams.Select(st => new { st.Id, st.Name, st.Desc, st.ManagerId, st.TeamMembers  }).Where(w => w.ManagerId.Equals(s.Id) || w.TeamMembers.Any(a => a.UserId == s.Id) )
                           //|| w.TeamMembers.Any(a => a.UserId == s.Id)
                        }).FirstOrDefault();
                       
            if(user != null){
                return Ok(user);
            }
            return NotFound();
            
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]User request, [FromForm]string UserRoles,  [FromForm]string UserDepts, [FromForm]IList<IFormFile> file )
        {
            
            var exitingsUser = _context.Users.FirstOrDefault(e => e.Email == request.Email && e.Deleted == false);
            IList<UserRole> reqUR =  JsonConvert.DeserializeObject<IList<UserRole>>(UserRoles);
            IList<UserDept> reqUD =  JsonConvert.DeserializeObject<IList<UserDept>>(UserDepts);

            if(exitingsUser != null ) { return BadRequest("Email already in use"); }
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
                    Color = request.Color,
                    CreatedAt = DateTime.Now
                };

                if (request.File != null)
                {
                    var uploadedImage = _fileUtil.AvatarUpload(request.File, "Users");
                    userEntity.Image = "Users/"+uploadedImage;
                }

                _context.Users.Add(userEntity);
                _context.SaveChanges();

                var currentUser  = _context.Users.FirstOrDefault(e => e.Email == request.Email);
                
                foreach (UserRole role in reqUR){
                    UserRole userRoleEntity = new UserRole(){ RoleId = role.RoleId, UserId = currentUser.Id };
                    _context.UserRoles.Add(userRoleEntity);
                };  

                foreach (var dept in reqUD) {
                    UserDept userDeptEntity = new UserDept(){ DepartmentId = dept.DepartmentId, UserId = currentUser.Id };
                    _context.UserDepts.Add(userDeptEntity);
                }
                
                List<string> listMailTo = new List<string>();
                listMailTo.Add(request.Email);
                  _mailUtil.SendEmailWelcomeAsync(
                        new MailType {
                            ToEmail=listMailTo,
                            Subject= "Your email has been registered in Epsylon Ticketing",
                            UserFullName = request.FirstName + " " + request.LastName,
                            WelcomeEmail = request.Email,
                            WelcomePass = request.Password,
                            ButtonLink =  _config.GetSection("HomeSite").Value + "admin/login",
                        }
                    );
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

        [HttpPost("{id}")]
        [Authorize]
        public IActionResult PutUser(int id,[FromForm]User request, [FromForm]string UserRoles,  [FromForm]string UserDepts)
        {
            var transaction =  _context.Database.BeginTransaction();
            try
            {
            //    var hasSuper = _context.UserRoles.Where(w => w.UserId == id && w.RoleId == 1).FirstOrDefault();
            //    if(hasSuper == null) { return BadRequest("You have not access !"); }
                 var userExist =  _context.Users.Where(e => e.Id == id) .FirstOrDefault();
                
                if (userExist == null) {
                    return NotFound("User Not Found !");
                }

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
                    userExist.Image = "Users/"+uploadedImage;  
                  }
                }
           
                 _context.SaveChanges();

                IList<UserRole> reqUR =  JsonConvert.DeserializeObject<IList<UserRole>>(UserRoles);
                IList<UserDept> reqUD =  JsonConvert.DeserializeObject<IList<UserDept>>(UserDepts);
                
                var cUR = _context.UserRoles.Where(w => w.UserId == id).ToList(); //get user roles list by id
                var excludeInReqUR = cUR.Where(eur => !reqUR.Any(mur => mur.RoleId == eur.RoleId)).ToList(); // find cUR data where not exist in reqUR
                excludeInReqUR.ForEach(eur => {
                      //  _context.Entry(eur).State = EntityState.Deleted; 
                    _context.UserRoles.Remove(eur);
                }); // remove excluded data
                 _context.SaveChanges();

                // _context.Entry(cUR).State = EntityState.Added; 
                var excludeInCUR = reqUR.Where(mur => !cUR.Any(eur => eur.RoleId == mur.RoleId)).ToList();
                excludeInCUR.ForEach(mur => 
                {
                     _context.UserRoles.Add(new UserRole { RoleId = mur.RoleId, UserId = id});
                });
                _context.SaveChanges();


                var cUD = _context.UserDepts.Where(w => w.UserId == id).ToList();
              //  _context.Entry(cUD).State = EntityState.Deleted; 
                var excludeInReqUD = cUD.Where(eur => !reqUD.Any(mur => mur.DepartmentId == eur.DepartmentId)).ToList();
                excludeInReqUD.ForEach(eur => _context.UserDepts.Remove(eur));
                 _context.SaveChanges();

              //  _context.Entry(cUD).State = EntityState.Added; 
                var excludeInCUD = reqUD.Where(mur => !cUD.Any(eur => eur.DepartmentId == mur.DepartmentId)).ToList();
                excludeInCUD.ForEach(mur => _context.UserDepts.Add(new UserDept { DepartmentId = mur.DepartmentId, UserId = id}));

                _context.SaveChanges();
                transaction.Commit();

                return Ok(userExist);
            }
            catch (System.Exception e)
            {
                transaction.Rollback();
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
            var transaction = _context.Database.BeginTransaction();

            try
            {
            //   var hasSuper = _context.UserRoles.Where(w => w.Id == id && w.RoleId == 1).FirstOrDefault();
            //   if(hasSuper == null) {
            //       return BadRequest("You have not access !");
            //   }

               var userExist =  _context.Users.Where(e => e.Id == id) .FirstOrDefault();
                var isRemovedImage = false;
                if(String.IsNullOrEmpty(userExist.Image) == false){
                    isRemovedImage = _fileUtil.Remove(userExist.Image);
                }
                userExist.Image = null;
               userExist.Deleted = true;
                // foreach (var itemRole in userExist.UserRoles)
                // {
                //     _context.UserRoles.Remove(itemRole);
                // }
                // foreach (var itemDept in userExist.UserDepts)
                // {
                //     _context.UserDepts.Remove(itemDept);
                // }


               // _context.Users.Remove(userExist);
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
    }
}