using EventSystem.Client.Store.Account;
using EventSystem.Model;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace EventSystem.Client.Pages.Account
{
    public partial class Register : ComponentBase, IDisposable
    {
        [Inject] IState<AccountState> accountState { get; set;}
        [Inject] IDispatcher dispatcher { get; set; }

        public UserModel userModel { get; set; } = new UserModel();

        public bool IsAdmin { get; set; } = false;

        protected override void OnInitialized()
        {
            // Subscribe to state changes
            accountState.StateChanged += OnAccountStateChanged;
        }

        private async void OnAccountStateChanged(object sender, EventArgs e)
        {
            StateHasChanged();
        }

        public void HandleSubmit()
        {
            userModel.Role = IsAdmin ? "Admin" : "User"; 

            dispatcher.Dispatch(new RegisterAction(userModel));
        }

        public void Dispose()
        {
            accountState.StateChanged -= OnAccountStateChanged;
        }
    }
}
