using EventSystem.Model;

namespace EventSystem.Services
{
    public interface IEventService
    {
        Task<IEnumerable<EventModel>> GetEventsAsync();
        Task<EventModel> GetEventByIdAsync(long id);
        Task<EventModel> CreateEventAsync(EventModel eventModel, string jwtToken);
        Task<EventModel> UpdateEventAsync(EventModel eventModel, string jwtToken);
        Task DeleteEventAsync(long id, string jwtToken);
    }
}