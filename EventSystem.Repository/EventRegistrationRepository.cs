using EventSystem.Domain;
using EventSystem.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EventSystem.Repository
{
    public class EventRegistrationRepository : GenericRepository<EventRegistration>, IEventRegistrationRepository
    {
        public EventRegistrationRepository(DataContext context) : base(context) { }

        public async Task<bool> IsUserRegisteredForEvent(long eventId, string userId)
        {
            return await table.AsNoTracking()
                .AnyAsync(e => e.EventId == eventId && e.UserId == userId);
        }
    }
}
