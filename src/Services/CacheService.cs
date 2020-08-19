
using System;
using System.Collections.Generic;

//model
using hello.restaurant.api.Models;
using hello.restaurant.api.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace hello.restaurant.api.Services
{
    public interface ICacheService
    {
        IEnumerable<Restaurant> GetCachedRestaurant();
        void ClearCache();
    }
    public class CacheService : ICacheService
    {
        private readonly ICacheProvider _cacheProvider;

        public CacheService(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public IEnumerable<Restaurant> GetCachedRestaurant()
        {
            return _cacheProvider.GetFromCache<IEnumerable<Restaurant>>(CacheKeys.Restaurants);
        }

        public void ClearCache()
        {
            _cacheProvider.ClearCache(CacheKeys.Restaurants);
        }
    }
}