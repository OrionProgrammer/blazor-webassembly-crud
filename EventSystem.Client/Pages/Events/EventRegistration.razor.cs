using EventSystem.Client.Helpers;
using EventSystem.Client.Store.Account;
using EventSystem.Client.Store.Event;
using EventSystem.Client.Store.EventRegistration;
using EventSystem.Model;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EventSystem.Client.Pages.Events
{
    public partial class EventRegistration : ComponentBase, IDisposable
    {
        [Inject] IDispatcher dispatcher { get; set; }
        [Inject] IState<EventState> eventState { get; set; }
        [Inject] IState<EventRegistrationState> eventRegistrationState { get; set; }
        [Inject] NavigationManager navigation { get; set; }
        [Inject] IState<AccountState> accountState { get; set; }
        [Inject] SessionHelper sessionHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }
        public UserModel userModel { get; set; } = new UserModel();
        private AuthUserModel authUserModel { get; set; }

        [Parameter] public int? eventId { get; set; }

        private EventModel eventModel = new();
        public string submitMessage { get; set; } = string.Empty;
        private bool isUpdateMode => eventId.HasValue;
        private string submitButtonText => isUpdateMode ? "Update Event" : "Create Event";
        private bool isRegistered { get; set; }
        private string message { get; set; }

        private async void OnEventStateChanged(object sender, EventArgs e)
        {
            if ((bool)(eventState?.Value?.IsApiTaskCompleted))
            {
                var @event = eventState?.Value?.SelectedEvent;

                if (@event is not null)
                {
                    eventModel.Id = @event.Id;
                    eventModel.AttendanceCount = @event.AttendanceCount;
                    eventModel.Date = @event.Date;
                    eventModel.Description = @event.Description;
                    eventModel.Location = @event.Location;
                    eventModel.SeatCount = @event.SeatCount;
                    eventModel.Name = @event.Name;

                    StateHasChanged();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            // Subscribe to state changes
            eventState.StateChanged += OnEventStateChanged;
            eventRegistrationState.StateChanged += OnEventRegisterStateChanged;

            dispatcher.Dispatch(new SelectEventAction(eventId.Value));

            try
            {
                authUserModel = await sessionHelper.GetUserSessionModel();
            }
            catch
            {
                isRegistered = false;
            }

            if (authUserModel is { })
            {
                dispatcher.Dispatch(new GetUserRegisteredAction(eventModel.Id, authUserModel.UserId));
            }
        }

        private async void OnEventRegisterStateChanged(object sender, EventArgs e)
        {
            if ((bool)(eventRegistrationState?.Value?.IsApiTaskCompleted))
            {
                isRegistered = eventRegistrationState.Value.IsUserRegistered;

                message = "You are registered for this event";

                if (!string.IsNullOrWhiteSpace(eventRegistrationState?.Value?.EventRegistration?.ReferenceNumber))
                {
                    message = $"Registered!. Reference Number: {eventRegistrationState.Value.EventRegistration.ReferenceNumber}";
                    
                    dispatcher.Dispatch(new SelectEventAction(eventId.Value));
                }

                StateHasChanged();
            }
        }

        public async Task RegisterForEvent()
        {
            bool confirmed = await js.InvokeAsync<bool>("confirm", "Are you sure you want to register for this event?");

            if (confirmed)
            {
                if (authUserModel is { })
                {
                    EventRegistrationModel eventRegistrationModel = new()
                    {
                        UserId = authUserModel.UserId,
                        EventId = eventModel.Id,
                        ReferenceNumber = ""
                    };

                    dispatcher.Dispatch(new CreateEventRegistrationAction(eventRegistrationModel, authUserModel.Token));
                }
            }
        }


        public void Dispose()
        {
            eventState.StateChanged -= OnEventStateChanged;
            eventRegistrationState.StateChanged -= OnEventRegisterStateChanged;
        }
    }
}
