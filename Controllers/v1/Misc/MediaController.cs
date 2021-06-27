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

namespace TicketingApi.Controllers.v1.Misc
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MediaController: ControllerBase
    {
        private readonly AppDBContext  _context;
        private readonly IFileUtil _fileUtil;   
        
        public MediaController(AppDBContext context, IFileUtil fileUtil){
            _context = context; 
            _fileUtil = fileUtil;   
        }

        [HttpPost("/single-post")]
        [Authorize]
        public IActionResult SingleUploadFile([FromForm] Media request ){
            var uploadedImage = _fileUtil.AvatarUpload(request.File, "Users");
            return Ok(uploadedImage);
        }
        
        [HttpPost("/multi-post")]
        [Authorize]
        public IActionResult MultiUploadFile([FromForm] Media request, [FromForm] List<Media> File){
            foreach (var item in File)
            {
                
            }
            return Ok();
        }




    }
}