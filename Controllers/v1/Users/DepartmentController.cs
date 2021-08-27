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
    public class DepartmentController: ControllerBase
    {
 
        private readonly AppDBContext  _context;
        public DepartmentController(AppDBContext context )
        {
            _context = context; 
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetDepartments()
        {
          var allDepartment = _context.Departments.AsNoTracking();
           return Ok(allDepartment);
        }

        [HttpGet("{id}")]
         [Authorize]
        public IActionResult GetDepartmentById(int id)
        {
            var department = _context.Departments.AsNoTracking() .Where(e => e.Id == id) .FirstOrDefault();
            if(department != null){
                return Ok(department);
            }
            return NotFound();
            
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromForm]Department request)
        {
            var exitingsDepartment = _context.Departments.FirstOrDefault(e => e.Name == request.Name);
            if(exitingsDepartment != null ) {
                return BadRequest("Department already in exists");
            }
            
            var transaction =  _context.Database.BeginTransaction();
            try
            {
                Department departmentEntity = new Department()
                {   
              
                };

                _context.Departments.Add(departmentEntity);
                _context.SaveChanges();
                transaction.Commit();
                return Ok();
            }
            catch (System.Exception e)
            {
               return BadRequest(e.Message);
            }                
        }

        [HttpPost("{id}")]
        public IActionResult PutDepartment(int id,[FromForm]Department request)
        {
            try
            {
                var departmentExist =  _context.Departments .Where(e => e.Id == id) .FirstOrDefault();
                
                if (departmentExist == null) return NotFound("Department Not Found !");

                var transaction =  _context.Database.BeginTransaction();
            
                _context.SaveChanges();
    
                transaction.Commit();

                return Ok(departmentExist);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteDepartmentById(int id)
        {
            try
            {
               var transaction = _context.Database.BeginTransaction();
               var departmentExist =  _context.Departments .Where(e => e.Id == id) .FirstOrDefault();
                _context.Departments.Remove(departmentExist);
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