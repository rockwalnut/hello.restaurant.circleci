
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace hello.restaurant.api.APIs.Model.Gateway
{
    public class PlaceAsync
    {
        [JsonProperty("place_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("business_status")]
        public string BusinessStatus { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]

        public GeometryAsync Geometry { get; set; }

        [JsonProperty("rating")]
        public decimal Rating { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }

        [JsonProperty("user_ratings_total")]
        public int UserRatingsTotal { get; set; }

    }

    public class GeometryAsync
    {
        [JsonProperty("location")]

        public LocationAsync Location { get; set; }

    }

    public class LocationAsync
    {

        [JsonProperty("lat")]
        public decimal Lat { get; set; }


        [JsonProperty("lng")]
        public decimal Lng { get; set; }
    }

    public class PlaceResponseAsync
    {

        [JsonProperty("next_page_token")]
        public string NextPageToken { get; set; }


        [JsonProperty("results")]
        public IEnumerable<PlaceAsync> Results { get; set; }
    }



}