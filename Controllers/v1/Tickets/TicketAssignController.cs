using System.Transactions;
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
using TicketingApi.Models.v1.Users;
using TicketingApi.Models.v1.Misc;
using System.IdentityModel.Tokens.Jwt;
using TicketingApi.Entities;
using TicketingApi.Utils;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers.v1.Tickets
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TicketAssignController : ControllerBase
    {
    
        private readonly AppDBContext  _context;
        public TicketAssignController(AppDBContext context )
        {
            _context = context; 
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetTicketAssigns()
        {
          var allTicketAssign = _context.TicketAssigns.AsNoTracking();
           return Ok(allTicketAssign);
        }

        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetTicketAssignById(int id)
        {
            var ticketAssign = _context.TicketAssigns.AsNoTracking().Where(e => e.Id == id).FirstOrDefault();
            if(ticketAssign != null){
                return Ok(ticketAssign);
            }
            return Ok();
            
        }

        [HttpGet("byuser/{id}")]
        [Authorize]
        [Route("byuser/{id}")]
        public IActionResult GetTicketAssignByUser(int id)
        {
            var ticketAssign = _context.TicketAssigns.AsNoTracking().Where(e => e.UserId == id);
            if(ticketAssign != null){
                return Ok(ticketAssign);
            }
            return Ok();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]TicketAssign request)
        {
            var exitingsTicketAssign = _context.TicketAssigns.FirstOrDefault(e => e.TicketId == request.TicketId);
            if(exitingsTicketAssign != null ) {
                return BadRequest("TicketAssign already in exists");
            }
            
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                TicketAssign ticketAssignEntity = new TicketAssign()
                {   
              
                };

                _context.TicketAssigns.Add(ticketAssignEntity);
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
        public IActionResult PutTicketAssign(int id,[FromForm]TicketAssign request)
        {
            try
            {
                var ticketAssignExist =  _context.TicketAssigns.Where(e => e.Id == id) .FirstOrDefault();
                
                if (ticketAssignExist == null) return NotFound("TicketAssign Not Found !");

                var transaction =  _context.Database.BeginTransaction();
            
                _context.SaveChanges();
    
                transaction.Commit();

                return Ok(ticketAssignExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteTicketAssignById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var ticketAssignExist =  _context.TicketAssigns .Where(e => e.Id == id) .FirstOrDefault();
                _context.TicketAssigns.Remove(ticketAssignExist);
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