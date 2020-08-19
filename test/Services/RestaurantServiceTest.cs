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
using hello.restaurant.api.Exceptions;
using hello.restaurant.api.APIs.Services;
using AutoMapper;
using hello.restaurant.api.Extensions;
using hello.restaurant.api.APIs.Model.Gateway;
using System.Linq;

namespace hello.restaurant.api.test.Services
{
    public class RestaurantServiceTest
    {
        protected Mock<IGoogleService> mockGoogleService { get; set; }
        protected Mock<IMapper> mockMapper { get; set; }

        protected RestaurantService ServiceUnderTest { get; }

        protected ReadOnlyCollection<Restaurant> expectedRestaurants { get; }

        public RestaurantServiceTest()
        {
            mockGoogleService = new Mock<IGoogleService>();
            mockMapper = new Mock<IMapper>();
            ServiceUnderTest = new RestaurantService(mockGoogleService.Object, mockMapper.Object);

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

        public class ListAsync : RestaurantServiceTest
        {
            [Fact]
            [Trait("Category", "Restaurant")]
            public async Task should_return_all_restaurants_type_with_correct_mapper()
            {
                var keyword = "hello";
                var type = PlaceType.RESTAURANT.GetDescription();

                //arrange
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


                mockGoogleService
                    .Setup(x => x.ListPlaceByTextSeachAsync(keyword, type))
                    .ReturnsAsync(serviceResult);

                // Act
                var result = await ServiceUnderTest.ListAsync(keyword);
                int range = 2; //result.Count();

                // Assert
                Assert.Equal(range, expectedRestaurants.Count);

                Assert.Equal(serviceResult.FirstOrDefault().Id, expectedRestaurants.FirstOrDefault().Id);
                Assert.Equal(serviceResult.Last().Id, expectedRestaurants.Last().Id);
            }


            [Fact]
            [Trait("Category", "Restaurant")]
            public async void should_throw_RestaurantNotFoundException_when_service_return_null()
            {
                var keyword = "hello";
                var type = PlaceType.RESTAURANT.GetDescription();

                //arrange
                var serviceResult = new List<PlaceAsync>() { };

                mockGoogleService
                    .Setup(x => x.ListPlaceByTextSeachAsync(keyword, type))
                    .ReturnsAsync(serviceResult);

                // Act & Assert
                await Assert.ThrowsAsync<RestaurantNotFoundException>(() => ServiceUnderTest.ListAsync(keyword));

            }
        }
    }
}