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
using Microsoft.AspNetCore.Hosting;

namespace TicketingApi.Controllers.v1.Misc
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MediaController: ControllerBase
    {
        private readonly AppDBContext  _context;
        private readonly IFileUtil _fileUtil;  
        private readonly IWebHostEnvironment _env; 
        
        public MediaController(AppDBContext context, IFileUtil fileUtil, IWebHostEnvironment env){
            _context = context; 
            _fileUtil = fileUtil;
            _env = env;   
        }

        [AllowAnonymous]
        [HttpGet("user/{id}")]
        public IActionResult GetUserImage(int id){
          //  var userImage = "";
            var existingUser = _context.Users.Where(e => e.Id == id).FirstOrDefault();
            var uploadPath = Path.Combine(_env.ContentRootPath, "Medias/");
            var filePath = Path.Combine(uploadPath, existingUser.Image);
            byte[] b = System.IO.File.ReadAllBytes(filePath);
          // var type = b.GetType();
           //userImage = "data:image/png;base64," + Convert.ToBase64String(b);
            return File(b, "image/jpeg");
          //  return Ok(File(b, "text/plain", Path.GetFileName(filePath)));
        }

        [AllowAnonymous]
        [HttpGet("sender/{id}")]
        public IActionResult GetSenderImage(int id){
            //var userImage = "";
            var existingUser = _context.Senders.Where(e => e.Id == id).FirstOrDefault();
            var uploadPath = Path.Combine(_env.ContentRootPath, "Medias/");
            var filePath = Path.Combine(uploadPath, existingUser.Image);
            byte[] b = System.IO.File.ReadAllBytes(filePath);   
  //            return Ok(File(b, "Application/octet-stream", Path.GetFileName(filePath)));
          //  var type = b.GetType();
          // userImage = "data:image/png;base64," + Convert.ToBase64String(b);
            return File(b, "image/jpeg");
        //    return Ok(userImage);
        }
        
        [AllowAnonymous]
        [HttpGet("ticket/{id}")]
        //TODO
        public IActionResult GetTicketImage(int id){
          //  var userImage = "";
            var existingMedia = _context.Medias.Where(e => e.Id == id && e.RelType == "T").FirstOrDefault();
            var uploadPath = Path.Combine(_env.ContentRootPath, "Medias/");
            var filePath = Path.Combine(uploadPath,  existingMedia.FileName);
            byte[] b = System.IO.File.ReadAllBytes(filePath);
          // var type = b.GetType();
           //userImage = "data:image/png;base64," + Convert.ToBase64String(b);
            return File(b, "image/jpeg");
          //  return Ok(File(b, "text/plain", Path.GetFileName(filePath)));
        }

        [AllowAnonymous]
        [HttpGet("ticket-detail/{id}")]
        //TODO
        public IActionResult GetTicketDetailImage(int id){
          //  var userImage = "";
            var existingMedia = _context.Medias.Where(e => e.Id == id && e.RelType == "TD").FirstOrDefault();
            var uploadPath = Path.Combine(_env.ContentRootPath, "Medias/");
            var filePath = Path.Combine(uploadPath,  existingMedia.FileName             );
            byte[] b = System.IO.File.ReadAllBytes(filePath);
          // var type = b.GetType();
           //userImage = "data:image/png;base64," + Convert.ToBase64String(b);
            return File(b, "image/jpeg");
          //  return Ok(File(b, "text/plain", Path.GetFileName(filePath)));
        }

        [AllowAnonymous]
        [HttpGet("ticket/download/{id}")]
        //TODO
        public IActionResult GetDownloadTicketFile(int id){
          //  var userImage = "";
            var existingMedia = _context.Medias.Where(e => e.Id == id && e.RelType == "T").FirstOrDefault();
            var uploadPath = Path.Combine(_env.ContentRootPath, "Medias/");
            var filePath = Path.Combine(uploadPath,  existingMedia.FileName             );
            byte[] b = System.IO.File.ReadAllBytes(filePath);
            var type = Path.GetExtension(filePath).ToLower();
            var dtype = "image/jpeg";
            if(  type == ".pdf") { dtype = "application/pdf"; }
            else if( type == ".doc") { dtype = "application/msword"; }
            else if( type == ".docs") {dtype = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";  }
            else if( type == ".xls"){ dtype = "application/vnd.ms-excel"; }  
            else if( type == ".xlsx"){ dtype = " application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ; } 
            // else if(type == ".rar" || type == ".zip"){
            // }
            return File(b, dtype, Path.GetFileName(filePath));
        }

        [AllowAnonymous]
        [HttpGet("ticket-detail/download/{id}")]
        //TODO
        public IActionResult GetDownloadTicketDetailFile(int id){
          //  var userImage = "";
            var existingMedia = _context.Medias.Where(e => e.Id == id && e.RelType == "TD").FirstOrDefault();
            var uploadPath = Path.Combine(_env.ContentRootPath, "Medias/");
            var filePath = Path.Combine(uploadPath,  existingMedia.FileName             );
            byte[] b = System.IO.File.ReadAllBytes(filePath);
            var type = Path.GetExtension(filePath).ToLower();
            var dtype = "image/jpeg";
            if(  type == ".pdf") { dtype = "application/pdf"; }
            else if( type == ".doc") { dtype = "application/msword"; }
            else if( type == ".docs") {dtype = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";  }
            else if( type == ".xls"){ dtype = "application/vnd.ms-excel"; }  
            else if( type == ".xlsx"){ dtype = " application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" ; } 
            // else if(type == ".rar" || type == ".zip"){
            // }
            return File(b, dtype, Path.GetFileName(filePath));
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