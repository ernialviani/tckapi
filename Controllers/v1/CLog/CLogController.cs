using System;
using System.IO;    
using System.Linq;
using System.Collections.Generic;
using TicketingApi.Entities;
using TicketingApi.Utils;
using TicketingApi.DBContexts;
using Microsoft.AspNetCore.Mvc;
using TicketingApi.Models.v1.Tickets;
using TicketingApi.Models.v1.Users;
using TicketingApi.Models.v1.Misc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace TicketingApi.Controllers.v1.CLog
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CLogController : ControllerBase
    {
        private readonly AppDBContext  _context;
        private readonly IFileUtil _fileUtil;
        private readonly IMailUtil _mailUtil;
        private readonly IWebHostEnvironment _env; 
        private readonly IConfiguration _config;

        public CLogController(AppDBContext context, IFileUtil fileUtil, IMailUtil mailUtil, IWebHostEnvironment env,  IConfiguration config )
        {
            _context = context; 
            _fileUtil = fileUtil;
            _mailUtil = mailUtil;
            _env = env;   
            _config = config;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetClogs()
        {
          var allLogs = _context.CLogs.AsNoTracking()
                        .Include(t => t.CLogDetails).ThenInclude(t => t.Medias)
                        .Include(t => t.Users)
                        .Include(t => t.Apps).ThenInclude(t => t.Modules)
                    .Select(e => new {
                        e.Id, e.Version, e.Desc, e.Apps,  e.CreatedAt, e.UpdatedAt,
                        Users =  new { Id = e.Users.Id, Email = e.Users.Email, FirstName = e.Users.FirstName, LastName = e.Users.LastName, Image = e.Users.Image, Color=e.Users.Color },
                        CLogDetails = e.CLogDetails.Select(t => new { 
                            t.Title, t.Desc, t.CLogId, t.CLogTypeId, t.CLogTypes, t.Modules,
                            Medias = t.Medias == null ? null : t.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelId == t.Id && w.RelType == "CL"),
                        })
                    });

           return Ok(allLogs);
        }


        [HttpGet("type")]
        [Authorize]
        public IActionResult GetClogType()
        {
          var cLogType = _context.ClogTypes.AsNoTracking();
           return Ok(cLogType);
        }
    }
}