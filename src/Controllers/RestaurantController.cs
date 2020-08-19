using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;

//logg
using Serilog;

//services
using System.IO;
using System.Net.Http;
using hello.restaurant.api.Models;
using hello.restaurant.api.Services;
using hello.restaurant.api.Filters;

namespace hello.restaurant.api.Controllers
{
    //[Authorize]
    [ServiceFilter(typeof(EnsureUserAuthorizeInAsync))]
    [ApiVersion("1.0")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService, ICacheService cacheService)
        {
            _restaurantService = restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        //
        // Summary:
        //      list a restaurant by keyword with basic cache memory
        //
        // Returns:
        //      list of restaurant 
        //
        // Params:
        //      keyword: keyword from UI e.g. Bang Sue
        //
        /*[EnableCors("AllowCors")]
        [Route("list")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Restaurant>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Restaurant>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListAsync(string keyword)
        {
            var restaurants = await _restaurantService.ListAsync(keyword);
            return Ok(restaurants);
        }*/

        //
        // Summary:
        //      list a restaurant by keyword with full implement cache memory
        //      make this more reliable if there are multiple threads accessing our cache store
        //
        // Returns:
        //      list of restaurant 
        //
        // Params:
        //      keyword: keyword from UI e.g. Bang Sue
        //
        [EnableCors("AllowCors")]
        [Route("list")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Restaurant>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Restaurant>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CacheRestaurantAsync(string keyword)
        {
            var restaurants = await _restaurantService.ListAsync(keyword);
            var cacheEntry = restaurants;

            return Ok(cacheEntry);
        }

    }
}