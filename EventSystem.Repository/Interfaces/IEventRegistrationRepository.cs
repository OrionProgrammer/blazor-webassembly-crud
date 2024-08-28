using EventSystem.Domain;
using EventSystem.Helpers;

namespace EventSystem.Repository
{
    public interface IEventRegistrationRepository : IGenericRepository<EventRegistration>
    {
        Task<bool> IsUserRegisteredForEvent(long eventId, string userId);
    }
}