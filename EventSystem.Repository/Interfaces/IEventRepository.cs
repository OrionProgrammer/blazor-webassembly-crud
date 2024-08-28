using EventSystem.Domain;
using EventSystem.Helpers;

namespace EventSystem.Repository.Interfaces
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<bool> Exists(long id);
    }
}
