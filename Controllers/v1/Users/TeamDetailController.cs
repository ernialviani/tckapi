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
        [Authorize(Roles = RoleType.Admin)]
        public IActionResult GetTeamDeatails([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allTeam = _context.Teams.AsNoTracking()
                        .Include(ur => ur.TeamDetails);
           return Ok(allTeam);
        }


        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetTeamById(int id)
        {
            var teamDetail = _context.TeamDetails.AsNoTracking()
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
            if(teamDetail != null){
                return Ok(teamDetail);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]TeamDetail request)
        {
            var exitingsTeam = _context.TeamDetails.FirstOrDefault(e => e.MemberId == request.MemberId);
            if(exitingsTeam != null ) {
                return BadRequest("User already in team");
            }
            
            var transaction =  _context.Database.BeginTransaction();
               
            try
            {
                TeamDetail teamDetailEntity = new TeamDetail()
                {   
                    TeamId = request.TeamId,
                    MemberId = request.MemberId,
                };

                _context.TeamDetails.Add(teamDetailEntity);
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
        public IActionResult PutTeam(int id,[FromForm]TeamDetail request)
        {
            try
            {
                 var teamDetailExist =  _context.TeamDetails
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
                
                if (teamDetailExist == null) { return NotFound("TeamDetail Not Found !"); }

                var transaction =  _context.Database.BeginTransaction();
            
                teamDetailExist.TeamId = request.TeamId;
                teamDetailExist.MemberId = request.MemberId;
          
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
               var teamDetailExist =  _context.TeamDetails
                        .Where(e => e.Id == id)
                        .FirstOrDefault();
       
                _context.TeamDetails.Remove(teamDetailExist);
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