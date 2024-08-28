using EventSystem.Model;
using System.Net.Http.Json;

namespace EventSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;
        private const string ApiVersion = "v1"; // Adjust the version as needed

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("x-api-version", ApiVersion);
        }

        public async Task<LoginResponseModel> LoginAsync(LoginModel loginModel)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiVersion}/account/login", loginModel);

            var isSuccess = response.IsSuccessStatusCode;
            return await response.Content.ReadFromJsonAsync<LoginResponseModel>();
        }

        public async Task<RegisterResponseModel> RegisterAsync(UserModel userModel)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiVersion}/account/register", userModel);
            return await response.Content.ReadFromJsonAsync<RegisterResponseModel>();
        }
    }

}
