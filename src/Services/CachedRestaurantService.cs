using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using hello.restaurant.api.Models;
using hello.restaurant.api.Providers;
using hello.restaurant.api.Services;

namespace hello.restaurant.api.Services
{
    public class CachedRestaurantService : IRestaurantService
    {

        private readonly RestaurantService _restaurantService;
        private readonly ICacheProvider _cacheProvider;

        private static readonly SemaphoreSlim GetUsersSemaphore = new SemaphoreSlim(1, 1);

        public CachedRestaurantService(RestaurantService restaurantService, ICacheProvider cacheProvider)
        {
            _restaurantService = restaurantService;
            _cacheProvider = cacheProvider;
        }

        public async Task<IEnumerable<Restaurant>> ListAsync(string keyword)
        {
            return await GetCachedResponse(CacheKeys.Restaurants, () => _restaurantService.ListAsync(keyword));
        }

        private async Task<IEnumerable<Restaurant>> GetCachedResponse(string cacheKey, Func<Task<IEnumerable<Restaurant>>> func)
        {
            var restaurants = _cacheProvider.GetFromCache<IEnumerable<Restaurant>>(cacheKey);
            if (restaurants != null) return restaurants;
            restaurants = await func();
            _cacheProvider.SetCache(cacheKey, restaurants, DateTimeOffset.Now.AddDays(1));

            return restaurants;
        }

        private async Task<IEnumerable<Restaurant>> GetCachedResponse(string cacheKey, SemaphoreSlim semaphore, Func<Task<IEnumerable<Restaurant>>> func)
        {
            var restaurants = _cacheProvider.GetFromCache<IEnumerable<Restaurant>>(cacheKey);

            if (restaurants != null) return restaurants;
            try
            {
                await semaphore.WaitAsync();
                restaurants = _cacheProvider.GetFromCache<IEnumerable<Restaurant>>(cacheKey); // Recheck to make sure it didn't populate before entering semaphore
                if (restaurants != null)
                {
                    return restaurants;
                }
                restaurants = await func();
                _cacheProvider.SetCache(cacheKey, restaurants, DateTimeOffset.Now.AddDays(1));
            }
            finally
            {
                semaphore.Release();
            }

            return restaurants;
        }
    }
}