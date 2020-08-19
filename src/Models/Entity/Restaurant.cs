using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hello.restaurant.api.Models
{
    public class Restaurant
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string[] Types { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }

        public decimal Rating { get; set; }
    }
}