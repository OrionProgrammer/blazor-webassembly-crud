using EventSystem.Client.Helpers;
using EventSystem.Client.Store.Event;
using EventSystem.Model;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SortDirection = EventSystem.Client.Helpers.SortDirection;

namespace EventSystem.Client.Pages.Events
{
    public partial class List : ComponentBase, IDisposable
    {
        [Inject] IDispatcher dispatcher { get; set; }
        [Inject] IState<EventState> eventState { get; set; }
        [Inject] SessionHelper sessionHelper { get; set; }
        [Inject] IJSRuntime js { get; set; }

        public string SearchTerm { get; set; } = string.Empty;
        public int CurrentPage { get; set; } = 1;
        public string SortExpression { get; set; } = nameof(EventModel.Name);
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

        public IQueryable<EventModel> FilteredEvents;

        public List<EventModel> eventList = new List<EventModel>();

        protected override void OnInitialized()
        {
            dispatcher.Dispatch(new LoadEventsAction());

            // Subscribe to state changes
            eventState.StateChanged += OnEventStateChanged;
        }

        private async void OnEventStateChanged(object sender, EventArgs e)
        {
            if ((bool)(eventState?.Value?.IsApiTaskCompleted))
            {
                FilteredEvents = eventState.Value.Events
                .Where(e => string.IsNullOrEmpty(SearchTerm) || e.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                .OrderBy(SortExpression, SortDirection)
                .Skip((CurrentPage - 1) * 10)
                .Take(10)
                .AsQueryable();

                StateHasChanged();
            }
        }

        private void HandlePageChanged(int newPage)
        {
            CurrentPage = newPage;
        }

        public async void Delete(long eventId)
        {
            bool confirmed = await js.InvokeAsync<bool>("confirm", "Are you sure you want to delete this event?");

            if (confirmed)
            {
                var authUserModel = await sessionHelper.GetUserSessionModel();

                if (authUserModel is not null)
                {
                    dispatcher.Dispatch(new DeleteEventAction(eventId, authUserModel.Token));
                }
            }
        }

        public void Dispose()
        {
            eventState.StateChanged -= OnEventStateChanged;
        }
    }
}
