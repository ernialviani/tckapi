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
    public class ModuleController: ControllerBase
    {
         private readonly AppDBContext  _context;
        public ModuleController(AppDBContext context )
        {
            _context = context; 
        }

        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public IActionResult GetModules([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allModule = _context.Modules.AsNoTracking();
           return Ok(allModule);
        }

        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetModuleById(int id)
        {
            var module = _context.Modules.AsNoTracking()
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                       
            if(module != null){ return Ok(module); }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]Module request)
        {
            var exitingsModule = _context.Modules.FirstOrDefault(e => e.Name == request.Name);
            if(exitingsModule != null ) {
                return BadRequest("Name already exists");
            }
            
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                Module moduleEntity = new Module() { Name = request.Name, Desc = request.Desc };
                _context.Modules.Add(moduleEntity);
                _context.SaveChanges();
                transaction.Commit();
                return Ok(moduleEntity);
            }
            catch (System.Exception e)
            {
               return BadRequest(e.Message);
            }                
        }

        [HttpPost("{id}")]
        public IActionResult PutModule(int id,[FromForm]Module request)
        {
            try
            {
                 var moduleExist =  _context.Modules .Where(e => e.Id == id) .FirstOrDefault();
                
                if (moduleExist == null) { return NotFound("Module Not Found !"); }

                var transaction =  _context.Database.BeginTransaction();
            
                moduleExist.Name = request.Name;
                moduleExist.Desc = request.Desc;

                _context.SaveChanges();
                transaction.Commit();

                return Ok(moduleExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteModuleById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var moduleExist =  _context.Modules .Where(e => e.Id == id) .FirstOrDefault();
                _context.Modules.Remove(moduleExist);
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