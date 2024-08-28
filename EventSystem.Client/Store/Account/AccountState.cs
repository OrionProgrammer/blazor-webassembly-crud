using Fluxor;

namespace EventSystem.Client.Store.Account
{
    [FeatureState]
    public class AccountState
    {
        public string UserId { get; }
        public string FullName { get; }
        public string Token { get; }
        public bool IsAuthenticated { get; }
        public bool IsLoading { get; }
        public bool IsSuccess { get; }
        public string Message { get; }
        public string Role { get; }
        public int ExpiresIn { get; set; }

        // Event to notify when the state changes
        public event EventHandler StateChanged;

        private bool isApiTaskCompleted;
        public bool IsApiTaskCompleted
        {
            get => isApiTaskCompleted;
            private set
            {
                if (isApiTaskCompleted != value)
                {
                    isApiTaskCompleted = value;
                    if (isApiTaskCompleted)
                    {
                        OnStateChanged();
                    }
                }
            }
        }

        private AccountState()
        {
            UserId = "";
            FullName = "";
            Token = "";
            IsAuthenticated = false;
            IsLoading = false;
            IsSuccess = false;
            Role = "";
            Message = "";
            ExpiresIn = 0;
            isApiTaskCompleted = false;
        }

        public AccountState(string userId, 
            string fullName, 
            string token, 
            bool isAuthenticated, 
            bool isLoading, 
            bool isSuccess,
            string role,
            string message,
            int expiresIn,
            bool isApiTaskCompleted)
        {
            UserId = userId;
            FullName = fullName;
            Token = token;
            IsAuthenticated = isAuthenticated;
            IsLoading = isLoading;
            IsSuccess = isSuccess;
            Message = message;
            Role = role;
            ExpiresIn = expiresIn;
            IsApiTaskCompleted = isApiTaskCompleted;
        }

        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
