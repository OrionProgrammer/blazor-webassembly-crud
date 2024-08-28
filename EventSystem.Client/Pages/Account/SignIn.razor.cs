using EventSystem.Client.Helpers;
using EventSystem.Client.Store.Account;
using EventSystem.Model;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace EventSystem.Client.Pages.Account
{
    public partial class SignIn : ComponentBase, IDisposable
    {
        [Inject] IState<AccountState> accountState { get; set; }
        [Inject] IDispatcher dispatcher { get; set; }
        [Inject] public NavigationManager navManager { get; set; }
        [Inject] AuthenticationStateProvider? authStateProvider { get; set; }

        public LoginModel loginModel { get; set; } = new LoginModel();

        protected override void OnInitialized()
        {
            // Subscribe to state changes
            accountState.StateChanged += OnAccountStateChanged;
        }

        private async void OnAccountStateChanged(object sender, EventArgs e)
        {
            // Check if the IsApiTaskCompleted is true
            if (accountState.Value.IsApiTaskCompleted)
            {
                if (accountState.Value.IsAuthenticated)
                {
                    //create user session variale and redirect to home
                    AuthUserModel authUserModel = new()
                    {
                        UserId = accountState.Value.UserId,
                        FullName = accountState.Value.FullName,
                        Token = accountState.Value.Token,
                        Role = accountState.Value.Role ?? "User",
                        ExpiresIn = accountState.Value.ExpiresIn
                    };

                    var customAuthStateProvider = (CustomAuthenticationStateProvider)authStateProvider;

                    await customAuthStateProvider.UpdateAuthenticationState(authUserModel);

                    // id user is admin, then redirect to admin home
                    if (authUserModel.Role == "Admin")
                    {
                        navManager.NavigateTo("/events/alL", true);
                    }

                    navManager.NavigateTo("/", true);
                }
            }

            StateHasChanged();
        }

        public void HandleSubmit()
        {
            dispatcher.Dispatch(new LoginAction(loginModel));
        }

        public void Dispose()
        {
            accountState.StateChanged -= OnAccountStateChanged;
        }
    }
}
