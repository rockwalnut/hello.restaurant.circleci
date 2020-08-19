
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using hello.restaurant.api.APIs.Services;
using hello.restaurant.api.Exceptions;
using hello.restaurant.api.Extensions;

//model
using hello.restaurant.api.Models;
using Microsoft.Extensions.Caching.Memory;
//using hello.restaurant.api.Repositories;

namespace hello.restaurant.api.Services
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> 
        ListAsync(string keyword);

    }

    public class RestaurantService : IRestaurantService
    {
        private readonly IGoogleService _googleService;
        private readonly IMapper _autoMapper;

        //private readonly IMemoryCache _memoryCache;

        public RestaurantService(IGoogleService googleService, IMapper autoMapper)//, IMemoryCache memoryCache)
        {
            _googleService = googleService ?? throw new ArgumentNullException(nameof(googleService));
            _autoMapper = autoMapper ?? throw new ArgumentNullException(nameof(autoMapper));

            //use memory to cache result from apis 
            //_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
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
        public async Task<IEnumerable<Restaurant>> ListAsync(string keyword)
        {

            var places = await _googleService.ListPlaceByTextSeachAsync(keyword, PlaceType.RESTAURANT.GetDescription());
            if (!places.Any())
            {
                throw new RestaurantNotFoundException();
            }

            //mapping from  apis model to restaurant entity
            var entities = _autoMapper.Map<List<Restaurant>>(places);
            return entities;

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
        /*public async Task<IEnumerable<Restaurant>> ListAsync(string keyword)
        {
            var trimLowerKey = keyword.ToLower().Trim();
            //use memory to cache result from apis             var restaturantCache;
            List<Restaurant> restaturantCache;

            // Look for cache key.
            if (_memoryCache.TryGetValue(trimLowerKey, out restaturantCache))
            {
                return restaturantCache;
            }

            var places = await _googleService.ListPlaceByTextSeachAsync(keyword, PlaceType.RESTAURANT.GetDescription());
            if (!places.Any())
            {
                throw new RestaurantNotFoundException();
            }

            //mapping from  apis model to restaurant entity
            var entities = _autoMapper.Map<List<Restaurant>>(places);

            //store data
            // Key not in cache, so get data.
            restaturantCache = entities;

            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromHours(1));

            // Save data in cache.
            _memoryCache.Set(trimLowerKey, restaturantCache, cacheEntryOptions);

            return entities;

        }*/

    }
}