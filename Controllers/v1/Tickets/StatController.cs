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
using TicketingApi.Models.v1.Tickets;
using System.IdentityModel.Tokens.Jwt;
using TicketingApi.Entities;
using TicketingApi.Utils;
using Newtonsoft.Json;

namespace TicketingApi.Controllers.v1.Tickets
{
    
    
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class StatController : ControllerBase
    {

         private readonly AppDBContext  _context;
        private readonly IFileUtil _fileUtil;
        public StatController(AppDBContext context, IFileUtil fileUtil )
        {
            _context = context; 
            _fileUtil = fileUtil;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetStats([FromHeader] string Authorization)
        {
           
          var allStat = _context.Stats.AsNoTracking().Where(w => w.Id > 2);
           return Ok(allStat);
        }


        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetStatById(int id)
        {
            var app = _context.Stats.AsNoTracking()
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                       
            if(app != null){ return Ok(app); }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]Stat request)
        {
            var exitingsStat = _context.Stats.FirstOrDefault(e => e.Name == request.Name);
            if(exitingsStat != null ) {
                return BadRequest("Name already exists");
            }
            
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                Stat appEntity = new Stat() { Name = request.Name, Desc = request.Desc, };
    
                _context.Stats.Add(appEntity);
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
        public IActionResult PutStat(int id,[FromForm]Stat request)
        {
            try
            {
                 var appExist =  _context.Stats .Where(e => e.Id == id) .FirstOrDefault();
                
                if (appExist == null) { return NotFound("Stat Not Found !"); }

                var transaction =  _context.Database.BeginTransaction();
            
                appExist.Name = request.Name;
                appExist.Desc = request.Desc;
               
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
        public IActionResult DeleteStatById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var appExist =  _context.Stats
                        .Where(e => e.Id == id)
                        .FirstOrDefault();

                _context.Stats.Remove(appExist);
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