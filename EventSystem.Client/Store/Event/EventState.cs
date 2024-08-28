using EventSystem.Model;
using Fluxor;

namespace EventSystem.Client.Store.Event
{
    [FeatureState]
    public class EventState
    {
        public IEnumerable<EventModel> Events { get; }
        public EventModel SelectedEvent { get; }
        public bool IsLoading { get; }
        public string ErrorMessage { get; }

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

        private EventState()
        {
            Events = Enumerable.Empty<EventModel>();
            SelectedEvent = null;
            IsLoading = false;
            ErrorMessage = "";
            isApiTaskCompleted = false;
        }

        public EventState(IEnumerable<EventModel> events, EventModel selectedEvent, bool isLoading, string errorMessage, bool isApiTaskCompleted)
        {
            Events = events;
            SelectedEvent = selectedEvent;
            IsLoading = isLoading;
            ErrorMessage = errorMessage;
            IsApiTaskCompleted = isApiTaskCompleted;
        }

        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
