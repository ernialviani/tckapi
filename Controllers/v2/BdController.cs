using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using TicketingApi.Models.v2;

namespace TicketingApi.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class BdController: ControllerBase
    {
        private readonly IWebHostEnvironment _host;
        private readonly ILogger<BdController> _logger;

        BdModel model = new BdModel();
        public BdController(IWebHostEnvironment hosting, ILogger<BdController> logger) {
            _host = hosting;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get(){
            var webRoot = _host.ContentRootPath;
            DataTable dt = new DataTable();
            dt = model.GetData(webRoot);
            return Ok();
        }
        
    }
}