using System;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

using Newtonsoft.Json;
using System.Security.Claims;

using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using hello.restaurant.api.Models.DTO;

namespace hello.restaurant.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersionNeutral]
    [AllowAnonymous]
    public class HelloController : ControllerBase
    {

        private readonly IConfiguration configuration;

        public HelloController(IConfiguration _configuration)
        {
            configuration = _configuration ?? throw new ArgumentNullException(nameof(_configuration));
        }



        [EnableCors("AllowCors")]
        [Route("")]
        [HttpGet]
        public ActionResult<string> Index()
        {
            //var user = CvxClaimsPrincipal.Current;
            var user = HttpContext.User;

            return $"Hello World with .NET Framework CORE Web API at { DateTime.UtcNow }.";
        }

        //[AllowAnonymous]
        [EnableCors("AllowCors")]
        [Route("hosting")]
        [HttpGet]
        public ActionResult<string> Hosting()
        {
            return Ok(configuration["AppSettings:HostName"]);
        }

        //[AllowAnonymous]
        [EnableCors("AllowCors")]
        [Route("health")]
        [HttpGet]
        public ActionResult<Health> Health()
        {
            return new Health("Ok");
        }

        //[AllowAnonymous]
        [EnableCors("AllowCors")]
        [Route("throw")]
        [HttpGet]
        public ActionResult<string> Throw()
        {
            throw new IOException();
        }
    }
}