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
    public class TeamDetailController : ControllerBase
    {
        private readonly AppDBContext  _context;
        public TeamDetailController(AppDBContext context )
        {
            _context = context; 
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetTeamDeatails([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allTeam = _context.Teams.AsNoTracking().Where(w => w.Deleted == false)
                        .Include(ur => ur.TeamMembers);
           return Ok(allTeam);
        }


        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetTeamById(int id)
        {
            var teamDetail = _context.TeamMembers.AsNoTracking()
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
            if(teamDetail != null){
                return Ok(teamDetail);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]TeamMember request)
        {
            var exitingsTeam = _context.TeamMembers.FirstOrDefault(e => e.UserId == request.UserId);
            if(exitingsTeam != null ) {
                return BadRequest("User already in team");
            }
            
            var transaction =  _context.Database.BeginTransaction();
               
            try
            {
                TeamMember teamDetailEntity = new TeamMember()
                {   
                    TeamId = request.TeamId,
                    UserId = request.UserId,
                };

                _context.TeamMembers.Add(teamDetailEntity);
                _context.SaveChanges();
                transaction.Commit();
                return Ok(teamDetailEntity);
            }
            catch (System.Exception e)
            {
               return BadRequest(e.Message);
            }                
        }

        [HttpPost("{id}")]
        public IActionResult PutTeam(int id,[FromForm]TeamMember request)
        {
            try
            {
                 var teamDetailExist =  _context.TeamMembers
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                
                if (teamDetailExist == null) { return NotFound("TeamMember Not Found !"); }

                var transaction =  _context.Database.BeginTransaction();
            
                teamDetailExist.TeamId = request.TeamId;
                teamDetailExist.UserId = request.UserId;
          
                _context.SaveChanges();
                transaction.Commit();

                return Ok(teamDetailExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteTeamById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var teamDetailExist =  _context.TeamMembers
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
       
                _context.TeamMembers.Remove(teamDetailExist);
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