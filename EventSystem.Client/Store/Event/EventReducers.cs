using Fluxor;

namespace EventSystem.Client.Store.Event
{
    public static class EventReducers
    {
        // Load Events
        [ReducerMethod]
        public static EventState ReduceLoadEventsAction(EventState state, LoadEventsAction action) =>
            new EventState(state.Events, state.SelectedEvent, true, null, false);

        [ReducerMethod]
        public static EventState ReduceLoadEventsSuccessAction(EventState state, LoadEventsSuccessAction action) =>
            new EventState(action.Events, state.SelectedEvent, false, null, true);

        [ReducerMethod]
        public static EventState ReduceLoadEventsFailedAction(EventState state, LoadEventsFailedAction action) =>
            new EventState(state.Events, state.SelectedEvent, false, action.ErrorMessage, true);

        // Select Event
        [ReducerMethod]
        public static EventState ReduceSelectEventAction(EventState state, SelectEventAction action) =>
            new EventState(state.Events, 
                state?.Events?.FirstOrDefault(e => e.Id == action.EventId), 
                state.IsLoading, 
                state.ErrorMessage,
                true);

        // Add Event
        [ReducerMethod]
        public static EventState ReduceAddEventAction(EventState state, AddEventAction action) =>
            new EventState(state.Events, state.SelectedEvent, true, null, false);

        [ReducerMethod]
        public static EventState ReduceAddEventSuccessAction(EventState state, AddEventSuccessAction action) =>
            new EventState(state.Events, action.Event, state.IsLoading, state.ErrorMessage, true);

        [ReducerMethod]
        public static EventState ReduceAddEventFailedAction(EventState state, AddEventFailedAction action) =>
            new EventState(state.Events, state.SelectedEvent, state.IsLoading, action.ErrorMessage, true);

        // Update Event
        [ReducerMethod]
        public static EventState ReduceUpdateEventAction(EventState state, UpdateEventAction action) =>
            new EventState(state.Events.Append(action.Event), action.Event, state.IsLoading, state.ErrorMessage, false);

        [ReducerMethod]
        public static EventState ReduceUpdateEventSuccessAction(EventState state, UpdateEventSuccessAction action) =>
            new EventState(state.Events.Select(e => e.Id == state.SelectedEvent.Id ? state.SelectedEvent : e),
                action.Event, 
                state.IsLoading,
                state.ErrorMessage,
                true);

        [ReducerMethod]
        public static EventState ReduceUpdateEventFailedAction(EventState state, UpdateEventFailedAction action) =>
            new EventState(state.Events, state.SelectedEvent, state.IsLoading, action.ErrorMessage, true);

        // Delete Event
        [ReducerMethod]
        public static EventState ReduceDeleteEventAction(EventState state, DeleteEventAction action) =>
            new EventState(state.Events.Where(e => e.Id != action.EventId),
                state.SelectedEvent, state.IsLoading, state.ErrorMessage, false);

        [ReducerMethod]
        public static EventState ReduceDeleteEventSuccessAction(EventState state, DeleteEventSuccessAction action) =>
            new EventState(state.Events.Where(e => e.Id != action.EventId), 
                state.SelectedEvent, state.IsLoading, state.ErrorMessage, true);

        [ReducerMethod]
        public static EventState ReduceDeleteEventFailedAction(EventState state, DeleteEventFailedAction action) =>
            new EventState(state.Events, state.SelectedEvent, state.IsLoading, action.ErrorMessage, true);
    }

}
