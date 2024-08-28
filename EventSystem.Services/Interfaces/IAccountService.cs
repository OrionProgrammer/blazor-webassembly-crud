using EventSystem.Model;

namespace EventSystem.Services
{
    public interface IAccountService
    {
        Task<LoginResponseModel> LoginAsync(LoginModel loginModel);
        Task<RegisterResponseModel> RegisterAsync(UserModel userModel);
    }
}