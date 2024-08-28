using EventSystem.Model;

namespace EventSystem.Services
{
    public interface IEventRegistrationService
    {
        Task<bool> GetUserRegisteredAsync(long eventId, string userId);
        Task<EventRegistrationModel> CreateEventAsync(EventRegistrationModel eventRegistrationModel, string jwtToken);
    }
}