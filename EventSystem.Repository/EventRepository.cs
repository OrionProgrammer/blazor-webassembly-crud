using EventSystem.Domain;
using EventSystem.Helpers;
using EventSystem.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventSystem.Repository
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(DataContext context) : base(context) { }

        public async Task<bool> Exists(long id)
        {
            return await table.AsNoTracking().AnyAsync(e => e.Id == id);
        }
    }
}
