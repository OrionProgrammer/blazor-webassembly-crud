using EventSystem.Client.Store.Event;
using EventSystem.Model;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace EventSystem.Client.Pages
{
    public partial class Home : ComponentBase, IDisposable
    {
        [Inject] IDispatcher dispatcher { get; set; }
        [Inject] IState<EventState> eventState { get; set; }

        public IEnumerable<EventModel> events = Enumerable.Empty<EventModel>();

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
                events = eventState.Value.Events;
                StateHasChanged();
            }
        }
        
        public void Dispose()
        {
            eventState.StateChanged -= OnEventStateChanged;
        }
    }
}
