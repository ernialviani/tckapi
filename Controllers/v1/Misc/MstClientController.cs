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
    public class MstClientController : ControllerBase
    {

         private readonly AppDBContext  _context;
        private readonly IFileUtil _fileUtil;

        public MstClientController(AppDBContext context, IFileUtil fileUtil)
        {
            _context = context; 
            _fileUtil = fileUtil;
      
        }

        [HttpGet("/client-groups")]
        [Authorize]
        public IActionResult GetMstGroupClients([FromHeader] string Authorization)
        {
          var allMstClient = _context.ClientDetails.AsNoTracking();
           return Ok(allMstClient);
        }
        [HttpGet("clients")]
        [Authorize]
        public IActionResult GetMstClients([FromHeader] string Authorization)
        {
          var allMstClient = _context.ClientDetails.AsNoTracking();
           return Ok(allMstClient);
        }


        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetMstClientById(int id)
        {
            var mstClient = _context.ClientDetails.AsNoTracking()
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                       
            if(mstClient != null){ return Ok(mstClient); }
            return NotFound();
        }

        [HttpPost("/client-groups")]
        [Authorize]
        public IActionResult CreateGroup([FromForm]ClientGroup request)
        {
            var exitingsMstClient = _context.ClientDetails.FirstOrDefault(e => e.Name == request.Name);
            if(exitingsMstClient != null ) {
                return BadRequest("Name already exists");
            }
            
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                ClientGroup mstClientEntity = new ClientGroup() { Name = request.Name, Desc = request.Desc, };

                _context.ClientGroups.Add(mstClientEntity);
                _context.SaveChanges();
                transaction.Commit();
                return Ok(mstClientEntity);
            }
            catch (System.Exception e)
            {
               return BadRequest(e.Message);
            }                
        }
        
        [HttpPost("/client")]
        [Authorize]
        public IActionResult CreateClient([FromForm]ClientDetail request)
        {
            var exitingsMstClient = _context.ClientDetails.FirstOrDefault(e => e.Name == request.Name);
            if(exitingsMstClient != null ) {
                return BadRequest("Name already exists");
            }
            
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                ClientDetail mstClientEntity = new ClientDetail() { Name = request.Name, Desc = request.Desc, };

                _context.ClientDetails.Add(mstClientEntity);
                _context.SaveChanges();
                transaction.Commit();
                return Ok(mstClientEntity);
            }
            catch (System.Exception e)
            {
               return BadRequest(e.Message);
            }                
        }

        [HttpPost("/client-groups/{id}")]
        [Authorize]
        public IActionResult PutMstGroupClient(int id,[FromForm]ClientGroup request)
        {
            try
            {
                var mstClientExist =  _context.ClientGroups.Where(e => e.Id == id) .FirstOrDefault();
                if (mstClientExist == null) { return NotFound("Group Client Not Found !"); }
                var transaction =  _context.Database.BeginTransaction();
                mstClientExist.Name = request.Name;
                mstClientExist.Desc = request.Desc;
                _context.SaveChanges();
                transaction.Commit();

                return Ok(mstClientExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("/client/{id}")]
        [Authorize]
        public IActionResult PutMstClient(int id,[FromForm]ClientDetail request)
        {
            try
            {
                var mstClientExist =  _context.ClientDetails.Where(e => e.Id == id) .FirstOrDefault();
                if (mstClientExist == null) { return NotFound("Client Not Found !"); }
                var transaction =  _context.Database.BeginTransaction();
                mstClientExist.Name = request.Name;
                mstClientExist.Desc = request.Desc;
                _context.SaveChanges();
                transaction.Commit();

                return Ok(mstClientExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("group-client/{id}")]
        [Authorize]
        public IActionResult DeleteMstGroupClientById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var mstGroupClientExist =  _context.ClientGroups
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
    
               
                foreach (var itemRole in mstGroupClientExist.ClientDetails)
                {
                    _context.ClientDetails.Remove(itemRole);
                }

                _context.ClientGroups.Remove(mstGroupClientExist);
                _context.SaveChanges();
                transaction.Commit();
                return Ok();
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }  

        [HttpDelete("group-client/{id}")]
        [Authorize]
        public IActionResult DeleteMstClientById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var mstClient =  _context.ClientDetails
                        .Where(e => e.Id == id)
                        .FirstOrDefault();

                _context.ClientDetails.Remove(mstClient);
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