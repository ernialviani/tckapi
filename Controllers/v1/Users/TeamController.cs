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
                        .Include(s => s.Manager)
                        .Include(ur => ur.TeamMembers).ThenInclude(s => s.Users)
                        .Select(s => new {
                            s.Id, s.Name, s.Desc, s.Image, s.Color, s.ManagerId, s.CreatedAt, s.UpdatedAt,
                            Manager = new { s.Manager.Id, s.Manager.FirstName, s.Manager.LastName, s.Manager.Email, s.Manager.Image, s.Manager.Color },
                            TeamMembers = s.TeamMembers == null ? null : s.TeamMembers.Select( tm => new {
                                tm.Id,
                                tm.TeamId,
                                tm.UserId,
                                Users = new {  tm.Users.Id, tm.Users.FirstName, tm.Users.LastName, tm.Users.Email, tm.Users.Image, tm.Users.Color }
                            })
                        }).OrderByDescending(e => e.Id);

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
        public IActionResult Create([FromBody]Team request, string Members)
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
                    ManagerId = request.ManagerId,
                    CreatedAt = DateTime.Now
                };

                _context.Teams.Add(teamEntity);
                _context.SaveChanges();

                 var cTeam = _context.Teams.Where(w => w.Name == request.Name).FirstOrDefault();
                 foreach (var member in request.TeamMembers)
                 {
                     _context.TeamMembers.Add(new TeamMember(){
                         TeamId = cTeam.Id,
                         UserId = member.UserId
                      });
                      _context.SaveChanges();
                 }
                transaction.Commit();
                return Ok(teamEntity);
            }
            catch (System.Exception e)
            {
                transaction.Rollback();
               return BadRequest(e.Message);
            }                
        }

        [HttpPost("{id}")]
        public IActionResult PutTeam(int id,[FromBody]Team request)
        {
            var transaction =  _context.Database.BeginTransaction();

            try
            {
                 var teamExist =  _context.Teams
                        .Where(e => e.Id == id)
                        .Include(ur => ur.TeamMembers)
                        .FirstOrDefault();
                
                if (teamExist == null) { return NotFound("Team Not Found !"); }
                teamExist.Name = request.Name;
                teamExist.Desc = request.Desc;
                teamExist.ManagerId = request.ManagerId;
                teamExist.UpdatedAt = DateTime.Now;
                _context.SaveChanges();

                var cTM = _context.TeamMembers.Where(w => w.TeamId == teamExist.Id).ToList();
                var excludeInReqTM = cTM.Where(eur => !request.TeamMembers.Any(mur => mur.UserId == eur.UserId)).ToList();
                excludeInReqTM.ForEach(eur => _context.TeamMembers.Remove(eur));
                 _context.SaveChanges();

                var excludeInCUD = request.TeamMembers.Where(mur => !cTM.Any(eur => eur.UserId == mur.UserId)).ToList();
                excludeInCUD.ForEach(mur => _context.TeamMembers.Add(new TeamMember {TeamId = id , UserId = mur.UserId}));

                _context.SaveChanges();
                transaction.Commit();

                return Ok();
            }
            catch (System.Exception e)
            {
                transaction.Rollback();
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteTeamById(int id)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
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
                transaction.Rollback();
                return BadRequest(e.Message);
            }
            
        }  

    }
}