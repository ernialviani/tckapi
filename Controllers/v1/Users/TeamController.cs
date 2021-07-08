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
        [Authorize]
        public IActionResult GetTeams([FromHeader] string Authorization)
        {
           
          var token = new JwtSecurityTokenHandler().ReadJwtToken(Authorization.Replace("Bearer ", ""));
      //    var Role = token.Claims.First(c => c.Type == "Role").Value;
          var allTeam = _context.Teams.AsNoTracking()
                        .Include(s => s.Leader)
                        .Include(ur => ur.TeamMembers).ThenInclude(s => s.Users)
                        .Select(s => new {
                            s.Id, s.Name, s.Desc, s.CreatedAt,
                            Leader = new { s.Leader.Id, s.Leader.FirstName, s.Leader.LastName, s.Leader.Email, s.Leader.Image, s.Leader.Color },
                            TeamMembers = s.TeamMembers == null ? null : s.TeamMembers.Select( tm => new {
                                tm.Id,
                                tm.TeamId,
                                Users = new {  tm.Users.Id, tm.Users.FirstName, tm.Users.LastName, tm.Users.Email, tm.Users.Image, tm.Users.Color }
                            })
                        });

           return Ok(allTeam);
        }


        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetTeamById(int id)
        {
            var team = _context.Teams.AsNoTracking()
                        .Where(e => e.Id == id)
                        .Include(ur => ur.TeamMembers)
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
                        .Include(ur => ur.TeamMembers)
                        .FirstOrDefault();
                
                if (teamExist == null) { return NotFound("Team Not Found !"); }

                var transaction =  _context.Database.BeginTransaction();
            
                teamExist.Name = request.Name;
                teamExist.Desc = request.Desc;
                teamExist.LeaderId = request.LeaderId;
                teamExist.UpdatedAt = DateTime.Now;
           
                _context.SaveChanges();
                
                teamExist.TeamMembers
                .Where(eur => !request.TeamMembers.Any(mur => mur.TeamId == eur.TeamId))
                .ToList()
                .ForEach(eur => 
                    teamExist.TeamMembers.Remove(eur)
                );

                request.TeamMembers
                .Where(mur => !teamExist.TeamMembers.Any(eur => eur.TeamId == mur.TeamId))
                .ToList()
                .ForEach(mur => teamExist.TeamMembers.Add(new TeamMember { UserId = mur.UserId, TeamId = mur.TeamId}));
            
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
                        .Include(ur => ur.TeamMembers)
                        .FirstOrDefault();
               
                foreach (var itemRole in teamExist.TeamMembers)
                {
                    _context.TeamMembers.Remove(itemRole);
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