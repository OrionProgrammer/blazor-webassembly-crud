using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using EventSystem.Model;
using Newtonsoft.Json;

namespace EventSystem.Services
{
    public class EventRegistrationService : IEventRegistrationService
    {
        private readonly HttpClient _httpClient;
        private const string ApiVersion = "v1"; // Adjust the version as needed

        public EventRegistrationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("x-api-version", ApiVersion);
        }

        public async Task<bool> GetUserRegisteredAsync(long eventId, string userId)
        {
            var response = await _httpClient.GetAsync($"{ApiVersion}/eventregistration/{eventId}/{userId}");
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<EventRegistrationModel> CreateEventAsync(EventRegistrationModel eventRegistrationModel, string jwtToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{ApiVersion}/eventregistration");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(eventRegistrationModel), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<EventRegistrationModel>();
        }
    }
}
