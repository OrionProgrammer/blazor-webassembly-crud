using EventSystem.Model;
using Fluxor;

namespace EventSystem.Client.Store.EventRegistration
{
    [FeatureState]
    public class EventRegistrationState
    {
        public bool IsLoading { get; }
        public bool IsUserRegistered { get; }
        public EventRegistrationModel? EventRegistration { get; }
        public string? ErrorMessage { get; }

        // Event to notify when the state changes
        public event EventHandler StateChanged;

        private bool isApiTaskCompleted;
        public bool IsApiTaskCompleted
        {
            get => isApiTaskCompleted;
            private set
            {
                isApiTaskCompleted = value;
                if (isApiTaskCompleted)
                {
                    OnStateChanged();
                }
            }
        }

        private EventRegistrationState()
        {
            IsLoading = false;
            IsUserRegistered = false;
            EventRegistration = null;
            ErrorMessage = string.Empty;
            IsApiTaskCompleted = false;
        }

        public EventRegistrationState(bool isLoading, 
            bool isUserRegistered, 
            EventRegistrationModel? eventRegistration, 
            string? errorMessage, 
            bool isApiTaskCompleted)
        {
            IsLoading = isLoading;
            IsUserRegistered = isUserRegistered;
            EventRegistration = eventRegistration;
            ErrorMessage = errorMessage;
            IsApiTaskCompleted = isApiTaskCompleted;
        }

        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
