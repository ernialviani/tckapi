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
    public class TeamController : ControllerBase
    {
        private readonly AppDBContext  _context;
        public TeamController(AppDBContext context ) { _context = context; }

        [HttpGet]
        [Authorize(Roles = RoleType.Admin)]
        public IActionResult GetTeams([FromHeader] string Authorization)
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
            var team = _context.Teams.AsNoTracking()
                        .Where(e => e.Id == id)
                        .Include(ur => ur.TeamDetails)
                        .FirstOrDefault();
                       
            if(team != null){
                return Ok(team);
            }
            return NotFound();
            
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]Team request)
        {
            var exitingsTeam = _context.Teams.FirstOrDefault(e => e.Name == request.Name);
            if(exitingsTeam != null ) {
                return BadRequest("Team Name already in use");
            }
            
            var transaction =  _context.Database.BeginTransaction();
               
            try
            {
                Team teamEntity = new Team()
                {   
                    Name = request.Name,
                    Desc = request.Desc,
                    LeaderId = request.LeaderId,
                    CreatedAt = DateTime.Now
                };

                _context.Teams.Add(teamEntity);
                _context.SaveChanges();
                transaction.Commit();
                return Ok(teamEntity);
            }
            catch (System.Exception e)
            {
               return BadRequest(e.Message);
            }                
        }

        [HttpPost("{id}")]
        public IActionResult PutTeam(int id,[FromForm]Team request)
        {
            try
            {
                 var teamExist =  _context.Teams
                        .Where(e => e.Id == id)
                        .Include(ur => ur.TeamDetails)
                        .FirstOrDefault();
                
                if (teamExist == null) { return NotFound("Team Not Found !"); }

                var transaction =  _context.Database.BeginTransaction();
            
                teamExist.Name = request.Name;
                teamExist.Desc = request.Desc;
                teamExist.LeaderId = request.LeaderId;
                teamExist.UpdatedAt = DateTime.Now;
           
                _context.SaveChanges();
                
                teamExist.TeamDetails
                .Where(eur => !request.TeamDetails.Any(mur => mur.TeamId == eur.TeamId))
                .ToList()
                .ForEach(eur => 
                    teamExist.TeamDetails.Remove(eur)
                );

                request.TeamDetails
                .Where(mur => !teamExist.TeamDetails.Any(eur => eur.TeamId == mur.TeamId))
                .ToList()
                .ForEach(mur => teamExist.TeamDetails.Add(new TeamDetail { MemberId = mur.MemberId, TeamId = mur.TeamId}));
            
                _context.SaveChanges();
                transaction.Commit();

                return Ok(teamExist);
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
               var teamExist =  _context.Teams
                        .Where(e => e.Id == id)
                        .Include(ur => ur.TeamDetails)
                        .FirstOrDefault();
               
                foreach (var itemRole in teamExist.TeamDetails)
                {
                    _context.TeamDetails.Remove(itemRole);
                }

                _context.Teams.Remove(teamExist);
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