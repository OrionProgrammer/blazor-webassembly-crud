using EventSystem.Repository;
using EventSystem.Repository.Interfaces;

namespace EventSystem.Helpers
{
    public interface IUnitOfWork
    {
        IEventRepository EventRepository { get; }
        IEventRegistrationRepository EventRegistrationRepository { get; }

        Task Complete();
    }
}