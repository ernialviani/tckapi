using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using TicketingApi.Models.v1;
using Microsoft.Extensions.Configuration;
using Advantage.Data.Provider;
using Microsoft.AspNetCore.Authorization;

namespace TicketingApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BdController: ControllerBase
    {
        private readonly IWebHostEnvironment _host;
        private readonly ILogger<BdController> _logger;
        private readonly IConfiguration _configuration;

        BdModel model = new BdModel();
        public BdController(IWebHostEnvironment hosting, ILogger<BdController> logger, IConfiguration configuration ) {
            _host = hosting;
            _logger = logger;
            this._configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(){
            var webRoot = _host.ContentRootPath;
            DataTable dt = new DataTable();
            using (model.conn = new AdsConnection(_configuration.GetConnectionString("DefaultConnection")))
            { 
                model.conn.Open();
                dt = model.GetData(webRoot);
                model.conn.Close();
            }
            return Ok(dt);
        }
        
    }
}