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
using TicketingApi.Models.v1.CLogs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace TicketingApi.Controllers.v1.CLogs
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
                            t.Id, t.Title, t.Desc, t.CLogId, t.CLogTypeId, t.CLogTypes, t.Modules,
                            Medias = t.Medias == null ? null : t.Medias.Select(s => new { s.Id, s.FileName, s.FileType, s.RelId, s.RelType }).Where(w => w.RelId == t.Id && w.RelType == "CLD"),
                        })
                    }).OrderByDescending(e => e.Id);

           return Ok(allLogs);
        }


        [HttpGet("type")]
        [Authorize]
        public IActionResult GetClogType()
        {
          var cLogType = _context.ClogTypes.AsNoTracking();
           return Ok(cLogType);
        }

        [HttpGet("last-version/{app}")]
        [Authorize]
        public IActionResult GetLastVersion(int app)
        {
          var cLogVer = _context.CLogs.Where(w => w.AppId == app).AsNoTracking().OrderByDescending(e => e.Id).FirstOrDefault();
          if(cLogVer != null) {
             return Ok(cLogVer.Version);
          }
          else{
              return Ok("");
          }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]CLog request, [FromForm]string CLogDetails, [FromForm]IList<IFormFile> file)
        {
            var cCLog = _context.CLogs.FirstOrDefault(e => e.Version == request.Version);
            if(cCLog != null ) { return BadRequest("Version Already Exists !"); }
            IList<CLogDetail> clogDetails =  JsonConvert.DeserializeObject<IList<CLogDetail>>(CLogDetails);
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                _context.CLogs.Add(new CLog{
                    Version = request.Version,
                    AppId = request.AppId,
                    UserId = request.UserId,
                    CreatedAt = DateTime.Now
                });
                _context.SaveChanges();
                var newCLog = _context.CLogs.OrderByDescending(o => o.Id).First();
                foreach (var cd in clogDetails)
                {
                    _context.CLogDetails.Add(new CLogDetail{
                        CLogId =  newCLog.Id,
                        CLogTypeId = cd.CLogTypeId,
                        ModuleId = cd.ModuleId,
                        Title = cd.Title,
                        Desc = cd.Desc
                    });
                     _context.SaveChanges();
                    var newCLogDetail = _context.CLogDetails.OrderByDescending(o => o.Id).FirstOrDefault();
                    foreach(var fname in cd.ImageName){
                        foreach(var f in file)
                        {
                            if(f.FileName == fname){
                                Media uploadedFile = _fileUtil.FileUpload(f, "CLogs");
                                _context.Medias.Add(new Media{
                                    FileName = "CLogs/"+uploadedFile.FileName,
                                    FileType = uploadedFile.FileType,
                                    RelId = newCLogDetail.Id,
                                    CLogDetailId = newCLogDetail.Id,
                                    RelType = "CLD"
                                });
                            }
                        }
                        _context.SaveChanges();
                    }
                }
                 transaction.Commit();
                return Ok();
            }
            catch (System.Exception e)
            {
                transaction.Rollback();
               return BadRequest(e.Message);
            }                
        }
        [HttpPost("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromForm]CLog request, [FromForm]string CLogDetails, [FromForm]IList<IFormFile> file)
        {
            var cCLog = _context.CLogs.FirstOrDefault(e => e.Id == id);
            IList<CLogDetail> reqCLogDetails =  JsonConvert.DeserializeObject<IList<CLogDetail>>(CLogDetails);
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                    cCLog.UpdatedAt = DateTime.Now;
                    cCLog.Version = request.Version;
                    cCLog.AppId = request.AppId;

                    var cCLogDetail = _context.CLogDetails.Where(w => w.CLogId == id).ToList(); 

                    //update CLogDetails
                    var sUpdate = cCLogDetail.Where(r => reqCLogDetails.Any(c => c.Id == r.Id)).ToList();
                    sUpdate.ForEach(u => {
                        var cReqCLogDetail = reqCLogDetails.Where(w => w.Id == u.Id).FirstOrDefault();
                        u.ModuleId = cReqCLogDetail.ModuleId;
                        u.CLogTypeId = cReqCLogDetail.CLogTypeId;
                        u.Title = cReqCLogDetail.Title;
                        u.Desc = cReqCLogDetail.Desc;

                        //update CLogDetails images
                        var cMedias = _context.Medias.Where(w => w.CLogDetailId == u.Id).ToList();
                        foreach (var imgName in cReqCLogDetail.ImageName)
                        {
                            var existImg = cMedias.Any(a => a.FileName.Contains(imgName));
                            if(!existImg){
                                foreach(var f in file)
                                {
                                    if(f.FileName == imgName){
                                        Media uploadedFile = _fileUtil.FileUpload(f, "CLogs");
                                        _context.Medias.Add(new Media{
                                            FileName = "CLogs/"+uploadedFile.FileName,
                                            FileType = uploadedFile.FileType,
                                            RelId = u.Id,
                                            CLogDetailId = u.Id,
                                            RelType = "CLD"
                                        });
                                        break;
                                    }
                                }
                            }
                        }

                        foreach (var cMedia in cMedias)
                        {
                             var existImg = cReqCLogDetail.ImageName.Any(a => a.Contains(cMedia.FileName));
                             if(!existImg){
                                var isRemoved = _fileUtil.Remove(cMedia.FileName);
                                if(isRemoved) _context.Medias.Remove(cMedia);
                            }
                        }
                    });

                    
                    //delete CLogDetails
                    var sDeleted = cCLogDetail.Where(eur => !reqCLogDetails.Any(mur => mur.Id == eur.Id)).ToList(); 
                    sDeleted.ForEach(eur => {
                        var medias = _context.Medias.Where(w => w.CLogDetailId == eur.Id);
                        foreach (var media in medias)
                        {   
                            var isRemoved = _fileUtil.Remove(media.FileName);
                            if(isRemoved) _context.Medias.Remove(media);
                        }
                        _context.CLogDetails.Remove(eur);
                    }); 
                    _context.SaveChanges();


                    //add CLogDetails
                    var sAdd = reqCLogDetails.Where(mur => !cCLogDetail.Any(eur => eur.Id == mur.Id)).ToList();
                    sAdd.ForEach(mur => 
                    {
                        _context.CLogDetails.Add(new CLogDetail {
                            CLogId = id,
                            CLogTypeId = mur.CLogTypeId,
                            ModuleId = mur.ModuleId,
                            Title = mur.Title,
                            Desc = mur.Desc
                         });
                         _context.SaveChanges();
                         var newCLogDetail = _context.CLogDetails.OrderByDescending(o => o.Id).FirstOrDefault();
                        foreach(var fname in mur.ImageName){
                            foreach(var f in file)
                            {
                                if(f.FileName == fname){
                                    Media uploadedFile = _fileUtil.FileUpload(f, "CLogs");
                                    _context.Medias.Add(new Media{
                                        FileName = "CLogs/"+uploadedFile.FileName,
                                        FileType = uploadedFile.FileType,
                                        RelId = newCLogDetail.Id,
                                        CLogDetailId = newCLogDetail.Id,
                                        RelType = "CLD"
                                    });
                                }
                            }
                            _context.SaveChanges();
                        }
                    });
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
        public IActionResult Delete(int id)
        {
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                var cCLog = _context.CLogs.FirstOrDefault(e => e.Id == id);
                if(cCLog != null ) {
                    var CLogDetails = _context.CLogDetails.Where(w => w.CLogId == cCLog.Id).ToList();
                    foreach (var d in CLogDetails)
                    {
                        var medias = _context.Medias.Where(w => w.CLogDetailId == d.Id);
                        foreach (var media in medias)
                        {   
                            var isRemoved = _fileUtil.Remove(media.FileName);
                            if(isRemoved) _context.Medias.Remove(media);
                        }
                        _context.CLogDetails.Remove(d);
                    }
                    _context.CLogs.Remove(cCLog);
                    _context.SaveChanges();
                }
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