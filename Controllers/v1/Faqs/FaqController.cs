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
using TicketingApi.Models.v1.Faqs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace TicketingApi.Controllers.v1.Faqs
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class FaqController: ControllerBase
    {
        private readonly AppDBContext  _context;

        public FaqController(AppDBContext context){
            _context = context;
        } 

        [HttpGet]
        [Authorize]
        public IActionResult GetFaqs()
        {
          var allFaqs =  _context.Faqs.AsNoTracking()
                            .Include(t => t.Apps)
                            .Include(t => t.Modules)
                            .Include(t => t.Users)
                            .Select(s  => new{
                                s.Id, s.UserId, s.Question, s.Desc,  s.AppId, s.ModuleId, s.CreatedAt, s.UpdatedAt,
                                Users =  new { Id = s.Users.Id, Email = s.Users.Email, FirstName = s.Users.FirstName, LastName = s.Users.LastName, Image = s.Users.Image, Color=s.Users.Color },
                            });

           return Ok(allFaqs);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetFaqsByApp(int id){
            var faqByApp  = _context.Faqs.AsNoTracking()
                            .Include(t => t.Apps)
                            .Include(t => t.Modules)
                            .Include(t => t.Users)
                            .Select(s  => new{
                                s.Id, s.UserId, s.Question, s.Desc,  s.AppId, s.ModuleId, s.CreatedAt, s.UpdatedAt,
                                Users =  new { Id = s.Users.Id, Email = s.Users.Email, FirstName = s.Users.FirstName, LastName = s.Users.LastName, Image = s.Users.Image, Color=s.Users.Color },
                            }).Where(w => w.AppId == id);
            return Ok(faqByApp);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody]List<Faq> request, [FromHeader] string Authorization)
        {
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                _context.Faqs.AddRange(request);
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

        [HttpPost("put")]
        [Authorize]
        public IActionResult Put([FromForm]string request, [FromForm] int app, [FromForm] int module, [FromForm] int user, [FromHeader] string Authorization )
        {
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                    var faqByApp = _context.Faqs.Where(w => w.AppId == app && w.ModuleId == module).ToList();
                    IList<Faq> reqFaqList =  JsonConvert.DeserializeObject<IList<Faq>>(request);
                    foreach (var faq in reqFaqList)
                    {
                        var cFaq = _context.Faqs.Where(w => w.Id == faq.Id).FirstOrDefault();
                        if(cFaq != null){
                            cFaq.UserId = user;
                            cFaq.AppId = app;
                            cFaq.ModuleId = module;
                            cFaq.Question = faq.Question;
                            cFaq.Desc = faq.Desc;
                            cFaq.UpdatedAt = DateTime.Now;
                            _context.SaveChanges();
                        }
                    }
                    //delete FaqDetails
                    var sDeleted = faqByApp.Where(eur => !reqFaqList.Any(mur => mur.Id == eur.Id)).ToList(); 
                    sDeleted.ForEach(eur => {
                        _context.Faqs.Remove(eur);
                    }); 
                    _context.SaveChanges();

                    //add FaqDetails
                    var sAdd = reqFaqList.Where(mur => !faqByApp.Any(eur => eur.Id == mur.Id)).ToList();
                    sAdd.ForEach(mur => 
                    {
                        _context.Faqs.Add(new Faq {
                            AppId = app,
                            ModuleId = module,
                            UserId = user,
                            Question = mur.Question,
                            Desc = mur.Desc,
                            CreatedAt = DateTime.Now
                         });
                         _context.SaveChanges();
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


    }
}