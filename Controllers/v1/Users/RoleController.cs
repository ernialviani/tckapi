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

namespace TicketingApi.Controllers.v1.Users
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RoleController: ControllerBase
    {
 
        private readonly AppDBContext  _context;
        public RoleController(AppDBContext context )
        {
            _context = context; 
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetRoles()
        {
          var allRole = _context.Roles.AsNoTracking();
           return Ok(allRole);
        }

        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetRoleById(int id)
        {
            var role = _context.Roles.AsNoTracking() .Where(e => e.Id == id) .FirstOrDefault();
            if(role != null){
                return Ok(role);
            }
            return NotFound();
            
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]Role request)
        {
            var exitingsRole = _context.Roles.FirstOrDefault(e => e.Name == request.Name);
            if(exitingsRole != null ) {
                return BadRequest("Role already in exists");
            }
            
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                Role roleEntity = new Role()
                {   
              
                };

                _context.Roles.Add(roleEntity);
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
        public IActionResult PutRole(int id,[FromForm]Role request)
        {
            try
            {
                var roleExist =  _context.Roles .Where(e => e.Id == id) .FirstOrDefault();
                
                if (roleExist == null) return NotFound("Role Not Found !");

                var transaction =  _context.Database.BeginTransaction();
            
                _context.SaveChanges();
    
                transaction.Commit();

                return Ok(roleExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteRoleById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var roleExist =  _context.Roles .Where(e => e.Id == id) .FirstOrDefault();
                _context.Roles.Remove(roleExist);
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