using EventSystem.Client.Helpers;
using EventSystem.Client.Store.Account;
using EventSystem.Client.Store.Event;
using EventSystem.Model;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace EventSystem.Client.Pages.Events
{
    public partial class CreateUpdate : ComponentBase, IDisposable
    {
        [Inject] IDispatcher dispatcher { get; set; }
        [Inject] IState<EventState> eventState { get; set; }
        [Inject] NavigationManager navigation { get; set; }
        [Inject] IState<AccountState> accountState { get; set; }
        [Inject] SessionHelper sessionHelper { get; set; }
        public UserModel userModel { get; set; } = new UserModel();

        [Parameter] public int? eventId { get; set; }

        private EventModel eventModel = new();
        public string submitMessage { get; set; } = string.Empty;
        private bool isUpdateMode => eventId.HasValue;
        private string submitButtonText => isUpdateMode ? "Update Event" : "Create Event";

        private async void OnEventStateChanged(object sender, EventArgs e)
        {
            // need to add a check to see if it's update or add, then clear model and state.
            // only update DOM if it's a fecth

            // update the eventModel
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
            if (isUpdateMode)
            {
                var eventToEdit = eventState?.Value?.Events?.FirstOrDefault(e => e.Id == eventId);
                if (eventToEdit == null)
                {
                    dispatcher.Dispatch(new SelectEventAction(eventId.Value));
                }
                else
                {
                    eventModel = eventToEdit;
                }
            }

            // Subscribe to state changes
            eventState.StateChanged += OnEventStateChanged;
        }

        private async Task HandleValidSubmit()
        {
            // get the accountstate from session
            var authUserModel = await sessionHelper.GetUserSessionModel();

            if (authUserModel is not null)
            {
                if (isUpdateMode)
                {
                    // Dispatch update action
                    dispatcher.Dispatch(new UpdateEventAction(eventModel, authUserModel.Token));
                }
                else
                {
                    // Dispatch create action
                    dispatcher.Dispatch(new AddEventAction(eventModel, authUserModel.Token));
                }


                submitMessage = isUpdateMode ? "Event updated successfully!" : "Event created successfully!";
                eventModel = new EventModel(); // Clear the form
            }
            else
            {
                submitMessage = "Operation was not performed. Invalid User!";
            }
        }

        public void Dispose()
        {
            eventState.StateChanged -= OnEventStateChanged;
        }
    }
}
