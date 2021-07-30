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
using TicketingApi.Models.v1.Misc;
using System.IdentityModel.Tokens.Jwt;
using TicketingApi.Entities;
using TicketingApi.Utils;
using Newtonsoft.Json;

namespace TicketingApi.Controllers.v1.Misc
{
    
    
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AppController : ControllerBase
    {

         private readonly AppDBContext  _context;
        private readonly IFileUtil _fileUtil;
        public AppController(AppDBContext context, IFileUtil fileUtil )
        {
            _context = context; 
            _fileUtil = fileUtil;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetApps([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allApp = _context.Apps.AsNoTracking()
                        .Include(ur => ur.Modules);
           return Ok(allApp);
        }


        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetAppById(int id)
        {
            var app = _context.Apps.AsNoTracking()
                        .Where(e => e.Id == id)
                        .Include(ur => ur.Modules)
                        .FirstOrDefault();
                       
            if(app != null){ return Ok(app); }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]App request)
        {
            var exitingsApp = _context.Apps.FirstOrDefault(e => e.Name == request.Name);
            if(exitingsApp != null ) {
                return BadRequest("Name already exists");
            }
            
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                App appEntity = new App() { Name = request.Name, Desc = request.Desc, };
                if (request.File != null)
                {
                    var uploadedImage = _fileUtil.AvatarUpload(request.File, "Apps");
                    appEntity.Logo = uploadedImage;
                }

                _context.Apps.Add(appEntity);
                _context.SaveChanges();
                transaction.Commit();
                return Ok(appEntity);
            }
            catch (System.Exception e)
            {
               return BadRequest(e.Message);
            }                
        }

        [HttpPost("{id}")]
        public IActionResult PutApp(int id,[FromForm]App request)
        {
            try
            {
                 var appExist =  _context.Apps .Where(e => e.Id == id) .FirstOrDefault();
                
                if (appExist == null) { return NotFound("App Not Found !"); }

                var transaction =  _context.Database.BeginTransaction();
            
                appExist.Name = request.Name;
                appExist.Desc = request.Desc;

                if (request.File != null) {
                  var isRemovedImage = _fileUtil.Remove(appExist.Logo);
                  if(isRemovedImage){
                    var uploadedImage = _fileUtil.AvatarUpload(request.File, "Apps");
                    appExist.Logo = uploadedImage;  
                  }
                }
               
                _context.SaveChanges();
                transaction.Commit();

                return Ok(appExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteAppById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var appExist =  _context.Apps
                        .Where(e => e.Id == id)
                        .Include(ur => ur.Modules)
                        .FirstOrDefault();
                var isRemovedImage = false;
                if(String.IsNullOrEmpty(appExist.Logo) == false){
                    isRemovedImage = _fileUtil.Remove("Apps/"+appExist.Logo);
                }
               
                foreach (var itemRole in appExist.Modules)
                {
                    _context.Modules.Remove(itemRole);
                }

                _context.Apps.Remove(appExist);
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