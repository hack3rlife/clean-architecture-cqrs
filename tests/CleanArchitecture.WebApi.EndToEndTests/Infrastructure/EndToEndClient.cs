using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CleanArchitecture.WebApi.EndToEndTests.Infrastructure
{
    public class EndToEndClient : IEndToEndClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public EndToEndClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var baseUri = _configuration.GetValue<string>("BlogWebAPI:BaseUrl");

            _httpClient.BaseAddress = string.IsNullOrEmpty(baseUri) ? new Uri("http://localhost/") : new Uri(baseUri);
        }
        public async Task<ApiResponse<T>> GetApiResponseAsync<T>(string url)
        {
            var httpResponseMessage = await _httpClient.GetAsync(url);

            var apiResponse = new ApiResponse<T>
            {
                StatusCode = httpResponseMessage.StatusCode,
                IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode,
            };

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                apiResponse.ProblemDetails = JsonConvert.DeserializeObject<ProblemDetails>(await httpResponseMessage.Content.ReadAsStringAsync());
            }
            else
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                apiResponse.Result = JsonConvert.DeserializeObject<T>(content);
            }

            return apiResponse;
        }
    }
}
