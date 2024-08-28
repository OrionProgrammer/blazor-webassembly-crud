using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using EventSystem.Model;
using Newtonsoft.Json;

namespace EventSystem.Services
{
    public class EventService : IEventService
    {
        private readonly HttpClient _httpClient;
        private const string ApiVersion = "v1"; // Adjust the version as needed

        public EventService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("x-api-version", ApiVersion);
        }

        public async Task<IEnumerable<EventModel>> GetEventsAsync()
        {
            var response = await _httpClient.GetAsync($"{ApiVersion}/events");
            return await response.Content.ReadFromJsonAsync<IEnumerable<EventModel>>();
        }

        public async Task<EventModel> GetEventByIdAsync(long id)
        {
            var response = await _httpClient.GetAsync($"{ApiVersion}/events/{id}");
            return await response.Content.ReadFromJsonAsync<EventModel>();
        }

        public async Task<EventModel> CreateEventAsync(EventModel eventModel, string jwtToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{ApiVersion}/events");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(eventModel), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<EventModel>();
        }

        public async Task<EventModel> UpdateEventAsync(EventModel eventModel, string jwtToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{ApiVersion}/events/{eventModel.Id}");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            request.Content = new StringContent(JsonConvert.SerializeObject(eventModel), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<EventModel>();
        }

        public async Task DeleteEventAsync(long id, string jwtToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{ApiVersion}/events/{id}");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var response = await _httpClient.SendAsync(request);
        }
    }
}
