using EventSystem.Services;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace EventSystem.Client.Store.Event
{
    public class EventEffects
    {
        private readonly IEventService _eventService;

        public EventEffects(IEventService eventService)
        {
            _eventService = eventService;
        }

        // Load Events
        [EffectMethod]
        public async Task HandleLoadEventsAction(LoadEventsAction action, IDispatcher dispatcher)
        {
            try
            {
                var events = await _eventService.GetEventsAsync();
                dispatcher.Dispatch(new LoadEventsSuccessAction(events));
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new LoadEventsFailedAction(ex.Message));
            }
        }

        // Select Event
        [EffectMethod]
        public async Task HandleSelectEventAction(SelectEventAction action, IDispatcher dispatcher)
        {
            try
            {
                var selectedEvent = await _eventService.GetEventByIdAsync(action.EventId);
                dispatcher.Dispatch(new AddEventSuccessAction(selectedEvent));
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new AddEventFailedAction($"Failed to fetch the event: {ex.Message}"));
            }
        }

        // Add Event
        [EffectMethod]
        public async Task HandleAddEventAction(AddEventAction action, IDispatcher dispatcher)
        {
            try
            {
                var eventModel = await _eventService.CreateEventAsync(action.Event, action.JwtToken);
                dispatcher.Dispatch(new AddEventSuccessAction(eventModel));
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new AddEventFailedAction(ex.Message));
            }
        }

        // Update Event
        [EffectMethod]
        public async Task HandleUpdateEventAction(UpdateEventAction action, IDispatcher dispatcher)
        {
            try
            {
                await _eventService.UpdateEventAsync(action.Event, action.JwtToken);
                dispatcher.Dispatch(new UpdateEventSuccessAction(action.Event));
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new UpdateEventFailedAction(ex.Message));
            }
        }

        // Delete Event
        [EffectMethod]
        public async Task HandleDeleteEventAction(DeleteEventAction action, IDispatcher dispatcher)
        {
            try
            {
                await _eventService.DeleteEventAsync(action.EventId, action.JwtToken);
                dispatcher.Dispatch(new DeleteEventSuccessAction(action.EventId));
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new DeleteEventFailedAction(ex.Message));
            }
        }
    }

}
