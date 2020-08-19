using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//apis
using hello.restaurant.api.APIs.Services;
using hello.restaurant.api.APIs.Model.Gateway;

namespace hello.restaurant.api.APIs.Controller
{
    /*
    * this is a debug purpose controller, it no longer use when production 's time
    *
    *
    */

    [ApiVersion("1.0")]
    [Route("apis/{version:apiVersion}/[controller]")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        private readonly IGoogleService _googleService;

        public GoogleController(IGoogleService googleService)
        {
            _googleService = googleService ?? throw new ArgumentNullException(nameof(googleService));
        }


        [EnableCors("AllowCors")]
        [Route("place/textsearch")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlaceAsync>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListPlaceByTextSeachAsync(string keyword, string type)
        {
            var entities = await _googleService.ListPlaceByTextSeachAsync(keyword, type);
            return Ok(entities);
        }

    }
}
