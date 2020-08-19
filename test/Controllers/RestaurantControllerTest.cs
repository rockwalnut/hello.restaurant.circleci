using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using System;
using Xunit;
using Moq;

//model
using hello.restaurant.api;
using hello.restaurant.api.Controllers;
using hello.restaurant.api.Models;
using hello.restaurant.api.Services;
using System.Collections.ObjectModel;
using hello.restaurant.api.Extensions;
using System.Linq;

namespace hello.restaurant.api.test.Controllers
{
    //unit test for restaurant controllers
    public class RestaurantControllerTest
    {
        protected RestaurantController ControllerUnderTest { get; set; }

        protected Mock<IRestaurantService> mockService { get; set; }
        protected Mock<ICacheService> mockCacheService { get; set; }

        protected ReadOnlyCollection<Restaurant> expectedRestaurants { get; }


        public RestaurantControllerTest()
        {
            mockService = new Mock<IRestaurantService>();
            mockCacheService = new Mock<ICacheService>();

            ControllerUnderTest = new RestaurantController(mockService.Object, mockCacheService.Object);

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

            //mock result
            expectedRestaurants = new ReadOnlyCollection<Restaurant>(new List<Restaurant>
                {
                    new Restaurant() {
                                Id = "b1357cd1-2901-3e8c-9852-1e659bceae98",
                                Name = "JK. Rowling",
                                Rating = 4
                            },

                    new Restaurant() {
                                Id = "a1357cd1-2901-3e8c-9852-1e659bceae51",
                                Name = "Microsoft",
                                Rating = 5
                            }
                });
        }

        public class ListAsync : RestaurantControllerTest
        {

            [Fact]
            public async void should_return_OkObjectResult_with_restaurants_with_out_cahce()
            {
                var keyword = "hello";
                var type = PlaceType.RESTAURANT.GetDescription();

                // Arrange
                //should serialize from JSON
                var serviceResult = new List<Restaurant>
                {
                    new Restaurant() {
                                Id = "b1357cd1-2901-3e8c-9852-1e659bceae98",
                                Name = "JK. Rowling",
                                Rating = 4
                            },

                    new Restaurant() {
                                Id = "a1357cd1-2901-3e8c-9852-1e659bceae51",
                                Name = "Microsoft",
                                Rating = 5
                            }
                };

                //setup DI
                mockService.Setup(repo => repo.ListAsync(keyword)).ReturnsAsync(serviceResult);

                // Act
                var result = await ControllerUnderTest.CacheRestaurantAsync(keyword);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var restaurants = Assert.IsType<List<Restaurant>>(okResult.Value);

                //Log.Information(okResult.Value.GetType().ToString());       

                Assert.Equal(serviceResult.FirstOrDefault().Id, expectedRestaurants.FirstOrDefault().Id);
                Assert.Equal(serviceResult.Last().Id, expectedRestaurants.Last().Id);
            }


            [Fact]
            public async void should_return_400BadRequest_from_middleware_if_restaurant_notfound()
            {
                var keyword = "hello";
                var type = PlaceType.RESTAURANT.GetDescription();

                // Arrange
                //setup DI
                //mockService.Setup(repo => repo.ListAsync(keyword)).ReturnsAsync(new List<Restaurant>());

                // Act
                var result = await ControllerUnderTest.CacheRestaurantAsync(keyword);

                // Assert
                var badRequest = Assert.IsType<BadRequestResult>(result);
                //var exception = Assert.IsType<ErrorDetails>();

                //Assert.Same(expectedRestaurants, restaurants);
            }

        }
    }
}