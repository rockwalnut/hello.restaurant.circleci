using System;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;
using Newtonsoft.Json;

//model
using hello.restaurant.api;
using hello.restaurant.api.Controllers;
using hello.restaurant.api.Models;
using hello.restaurant.api.Services;
using hello.restaurant.api.test.Utilities;
using hello.restaurant.api.APIs.Services;
using hello.restaurant.api.APIs.Model.Gateway;
using System.Collections.ObjectModel;
using hello.restaurant.api.Extensions;

namespace hello.restaurant.api.test.Controllers
{
    //integration test for restaurant controllers
    public class RestaurantControllerITest : BaseHttpTest
    {
        protected Mock<IGoogleService> mockGoogleService { get; set; }

        protected ReadOnlyCollection<Restaurant> expectedRestaurants { get; }

        public RestaurantControllerITest()
        {
            mockGoogleService = new Mock<IGoogleService>();

            //serilog
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

        public class ListAsync : RestaurantControllerITest
        {
            //arrange

            protected override void ConfigureServices(IServiceCollection services)
            {
                //IoC
                services
               .AddSingleton(x => mockGoogleService.Object);
            }

            [Fact]
            public async Task Should_return_the_list_of_restaurant_without_cahce()
            {
                string keyword = "hello";
                string type = PlaceType.RESTAURANT.GetDescription();

                var serviceResult = new List<PlaceAsync>
                {
                    new PlaceAsync() {
                                Id = "b1357cd1-2901-3e8c-9852-1e659bceae98",
                                Name = "JK. Rowling",
                                Rating = 4
                            },

                    new PlaceAsync() {
                                Id = "a1357cd1-2901-3e8c-9852-1e659bceae51",
                                Name = "Microsoft",
                                Rating = 5
                            }
                };

                // Arrange

                var expectedNumberOfRestaurants = expectedRestaurants.Count();

                mockGoogleService
                    .Setup(x => x.ListPlaceByTextSeachAsync(keyword, type))
                        .ReturnsAsync(serviceResult);

                // Act
                var httpResponse = await Client.GetAsync("api/1.0/restaurant/list?keyword=" + keyword);

                // Assert
                httpResponse.EnsureSuccessStatusCode();
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();
                var restaurants = JsonConvert.DeserializeObject<Restaurant[]>(stringResponse);

                Assert.NotNull(restaurants);
                Assert.Equal(expectedNumberOfRestaurants, restaurants.Length);
                Assert.Collection(restaurants,
                    restaurant => Assert.Equal(expectedRestaurants.ElementAt(0).Id, restaurants[0].Id),
                    restaurant => Assert.Equal(expectedRestaurants.ElementAt(1).Id, restaurants[1].Id)
                );
            }
        }

    }
}