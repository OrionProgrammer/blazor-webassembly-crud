using EventSystem.Model;

namespace EventSystem.Client.Store.Account
{
    public record AccountActions();

    // Login Actions
    public record LoginAction(LoginModel LoginModel);

    public record LoginSuccessAction(LoginResponseModel LoginResponseModel);
    
    public record LoginFailedAction(string ErrorMessage);
    
    // Register Actions
    public record RegisterAction(UserModel UserModel);
    
    public record RegisterSuccessAction(string Message);
    
    public record RegisterFailedAction(string Message);
    
    // Logout Action
    public record LogoutAction { }

}
