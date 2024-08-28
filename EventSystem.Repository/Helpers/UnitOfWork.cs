
using System;
using System.Threading.Tasks;
using EventSystem.Domain;
using EventSystem.Repository;
using EventSystem.Repository.Interfaces;

namespace EventSystem.Helpers
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public IEventRepository EventRepository { get; }
        public IEventRegistrationRepository EventRegistrationRepository { get; }

        public UnitOfWork(DataContext dataContext,
            IEventRepository eventRepository,
            IEventRegistrationRepository eventRegistrationRepository)
        {
            this._context = dataContext;
            this.EventRepository = eventRepository;
            this.EventRegistrationRepository = eventRegistrationRepository;
        }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}