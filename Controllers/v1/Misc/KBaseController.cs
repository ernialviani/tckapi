using System.Reflection;
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
    public class KBaseController: ControllerBase
    {
        private readonly AppDBContext  _context;
        private readonly IFileUtil _fileUtil;
        public KBaseController(AppDBContext context, IFileUtil fileUtil )
        {
            _context = context; 
            _fileUtil = fileUtil;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetKBases([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allKBase = _context.KBases.AsNoTracking();
           return Ok(allKBase);
        }


        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetKBaseById(int id)
        {
            var kbase = _context.KBases.AsNoTracking()
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                       
            if(kbase != null){
                return Ok(kbase);
            }
            return NotFound();
            
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]KBase request)
        {
            var exitingsKBase = _context.KBases.FirstOrDefault(e => e.Id == request.Id);
            if(exitingsKBase != null ) {
                return BadRequest("Email already in use");
            }
            
            var transaction =  _context.Database.BeginTransaction();
               
            try
            {
                KBase kbaseEntity = new KBase()
                {   
                    Title = request.Title,
                    Body = request.Body,
                    AppId = request.AppId,
                    ModuleId = request.ModuleId,
                    UserId = request.UserId,
                    CreatedAt = DateTime.Now
                };

                _context.KBases.Add(kbaseEntity);
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
        public IActionResult PutKBase(int id,[FromForm]KBase request)
        {
            try
            {
                 var kbaseExist =  _context.KBases
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                
                if (kbaseExist == null)
                {
                    return NotFound("KBase Not Found !");
                }

                var transaction =  _context.Database.BeginTransaction();
            
                    kbaseExist.Title = request.Title;
                    kbaseExist.Body = request.Body;
                    kbaseExist.AppId = request.AppId;
                    kbaseExist.ModuleId = request.ModuleId;
                    kbaseExist.UserId = request.UserId;
                    kbaseExist.UpdatedAt = DateTime.Now;
             
                _context.SaveChanges();
                transaction.Commit();
                return Ok(kbaseExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteKBaseById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var kbaseExist =  _context.KBases
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                _context.KBases.Remove(kbaseExist);
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