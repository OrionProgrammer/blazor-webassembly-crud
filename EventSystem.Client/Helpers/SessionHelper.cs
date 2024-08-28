using Blazored.SessionStorage;
using EventSystem.Model;

namespace EventSystem.Client.Helpers
{
    public class SessionHelper
    {
        private readonly ISessionStorageService _sessionStorage;

        public SessionHelper(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task<AuthUserModel> GetUserSessionModel()
        {
            return await _sessionStorage.ReadEncryptedItemAsync<AuthUserModel>("UserSession");
        }
    }
}
