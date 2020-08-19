using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using hello.restaurant.api.Models;

namespace hello.restaurant.api.Exceptions
{
    public class RestaurantNotFoundException : Exception
    {

        private const string DEFAULT_MESSAGE = "Cargo not found";
        public string rev { get; }
        public string value { get; }

        public RestaurantNotFoundException()
           : base(DEFAULT_MESSAGE)
        {
        }

        public RestaurantNotFoundException(Guid id)
            : base(string.Format("Cargo with id = {0} not found", id.ToString()))
        {
        }

        public RestaurantNotFoundException(string message, Restaurant restaurant)
            : base(message)
        {
        }

        public RestaurantNotFoundException(string message, Exception inner)
       : base(message, inner)
        {
        }

    }
}