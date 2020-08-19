using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

//logg
using Serilog;

using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

//gateway
using hello.restaurant.api.APIs.Model.Gateway;
using hello.restaurant.api.APIs.Exceptions;

namespace hello.restaurant.api.APIs.Services
{
    public interface IGoogleService
    {
        Task<IEnumerable<PlaceAsync>> ListPlaceByTextSeachAsync(string keyword, string type);
        Task<IEnumerable<PlaceAsync>> ListPlaceByNearbySearchAsync(decimal lat, decimal lng, string type);
        Task<IEnumerable<PlaceAsync>> ListPlaceByFindPlaceFromTextAsync(string input, string type);
    }

    public class GoogleService : IGoogleService
    {
        private readonly HttpClient _httpClient;

        private string _remoteGoogleServiceBaseUrl;
        private string _googlePlaceApiKey;

        private readonly IConfiguration _configuration;

        public GoogleService(HttpClient httpClient, IConfiguration configuration)
        {
            // _httpClient = httpClient;
            _configuration = configuration;

            // Create an HttpClientHandler object and set to use default credentials
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            // Create an HttpClient object
            _httpClient = new HttpClient(handler);

            _remoteGoogleServiceBaseUrl = _configuration["AppSettings:GoogleMap:MapUrl"];
            _googlePlaceApiKey = _configuration["AppSettings:GoogleMap:ApiKey"];
        }

        //
        // Summary:
        //      list all place type = restaurant
        //
        // Returns:
        //      list of place 
        //
        // Params:
        //      keyword: keyword from UI e.g. Bang Sue
        //      type: type of place e.g. reataurant please see /model/enum
        //
        public async Task<IEnumerable<PlaceAsync>> ListPlaceByTextSeachAsync(string keyword, string type)
        {
            var endpoint = "/place/textsearch/json";
            var queryParams = $"?query={keyword}&type={type}&key={_googlePlaceApiKey}";

            var uri = new Uri(_remoteGoogleServiceBaseUrl + endpoint + queryParams);

            // Act
            var result = new HttpResponseMessage();

            try
            {
                result = await _httpClient.GetAsync(uri);
                result.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new ClientNotSuccessException((int)result.StatusCode, "HTTPGET", uri.ToString(), ex.Message);
            }

            var httpReponse = await result.Content.ReadAsStringAsync();
            var placeResponse = JsonConvert.DeserializeObject<PlaceResponseAsync>(httpReponse);

            var entities = placeResponse.Results;
            return entities;
        }

        public async Task<IEnumerable<PlaceAsync>> ListPlaceByNearbySearchAsync(decimal lat, decimal lng, string type)
        {
            await Task.Delay(0);
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<PlaceAsync>> ListPlaceByFindPlaceFromTextAsync(string input, string type)
        {
            await Task.Delay(0);
            throw new NotImplementedException();
        }


    }
}