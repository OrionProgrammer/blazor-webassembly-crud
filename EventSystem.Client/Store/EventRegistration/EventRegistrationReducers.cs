using Fluxor;

namespace EventSystem.Client.Store.EventRegistration
{
    public static class EventRegistrationReducers
    {
        [ReducerMethod]
        public static EventRegistrationState ReduceGetUserRegisteredAction(EventRegistrationState state, GetUserRegisteredAction action) =>
            new EventRegistrationState(isLoading: true, isUserRegistered: false, eventRegistration: state.EventRegistration, errorMessage: null, false);

        [ReducerMethod]
        public static EventRegistrationState ReduceGetUserRegisteredSuccessAction(EventRegistrationState state, GetUserRegisteredSuccessAction action) =>
            new EventRegistrationState(isLoading: false, isUserRegistered: action.IsRegistered, eventRegistration: state.EventRegistration, errorMessage: null, true);

        [ReducerMethod]
        public static EventRegistrationState ReduceGetUserRegisteredFailureAction(EventRegistrationState state, GetUserRegisteredFailureAction action) =>
            new EventRegistrationState(isLoading: false, isUserRegistered: false, eventRegistration: state.EventRegistration, errorMessage: action.ErrorMessage, true);

        [ReducerMethod]
        public static EventRegistrationState ReduceCreateEventRegistrationAction(EventRegistrationState state, CreateEventRegistrationAction action) =>
            new EventRegistrationState(isLoading: true, isUserRegistered: state.IsUserRegistered, eventRegistration: null, errorMessage: null, false);

        [ReducerMethod]
        public static EventRegistrationState ReduceCreateEventRegistrationSuccessAction(EventRegistrationState state, CreateEventRegistrationSuccessAction action) =>
            new EventRegistrationState(isLoading: false, isUserRegistered: true, eventRegistration: action.EventRegistrationModel, errorMessage: null, true);

        [ReducerMethod]
        public static EventRegistrationState ReduceCreateEventRegistrationFailureAction(EventRegistrationState state, CreateEventRegistrationFailureAction action) =>
            new EventRegistrationState(isLoading: false, isUserRegistered: state.IsUserRegistered, eventRegistration: null, errorMessage: action.ErrorMessage, true);
    }

}
