using EventSystem.Services;
using Fluxor;

namespace EventSystem.Client.Store.Account
{
    public class AccountEffects
    {
        private readonly IAccountService _accountService;

        public AccountEffects(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Login Effect
        [EffectMethod]
        public async Task HandleLoginAction(LoginAction action, IDispatcher dispatcher)
        {
            try
            {
                var loginResponse = await _accountService.LoginAsync(action.LoginModel);
                if (loginResponse.IsSuccess)
                {
                    dispatcher.Dispatch(new LoginSuccessAction(loginResponse));
                }
                else
                {
                    dispatcher.Dispatch(new LoginFailedAction(loginResponse.Message));
                }
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new LoginFailedAction(ex.Message));
            }
        }

        // Register Effect
        [EffectMethod]
        public async Task HandleRegisterAction(RegisterAction action, IDispatcher dispatcher)
        {
            try
            {
                var registerResponse = await _accountService.RegisterAsync(action.UserModel);
                if (registerResponse.IsSuccess)
                {
                    dispatcher.Dispatch(new RegisterSuccessAction(registerResponse.Message));
                }
                else
                {
                    dispatcher.Dispatch(new RegisterFailedAction(registerResponse.Message));
                }
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new RegisterFailedAction(ex.Message));
            }
        }
    }
}
