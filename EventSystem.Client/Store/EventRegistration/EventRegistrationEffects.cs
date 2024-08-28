using EventSystem.Services;
using Fluxor;

namespace EventSystem.Client.Store.EventRegistration
{
    public class EventRegistrationEffects
    {
        private readonly IEventRegistrationService _eventRegistrationService;

        public EventRegistrationEffects(IEventRegistrationService eventRegistrationService)
        {
            _eventRegistrationService = eventRegistrationService;
        }

        [EffectMethod]
        public async Task HandleGetUserRegisteredAction(GetUserRegisteredAction action, IDispatcher dispatcher)
        {
            try
            {
                var isRegistered = await _eventRegistrationService.GetUserRegisteredAsync(action.EventId, action.UserId);
                dispatcher.Dispatch(new GetUserRegisteredSuccessAction(action.EventId, action.UserId, isRegistered));
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new GetUserRegisteredFailureAction(ex.Message));
            }
        }

        [EffectMethod]
        public async Task HandleCreateEventRegistrationAction(CreateEventRegistrationAction action, IDispatcher dispatcher)
        {
            try
            {
                var eventRegistration = await _eventRegistrationService.CreateEventAsync(action.EventRegistrationModel, action.JwtToken);
                dispatcher.Dispatch(new CreateEventRegistrationSuccessAction(eventRegistration));
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new CreateEventRegistrationFailureAction(ex.Message));
            }
        }
    }

}
