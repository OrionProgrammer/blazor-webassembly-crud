using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using EventSystem.Domain;
using Microsoft.EntityFrameworkCore;

namespace EventSystem.Helpers
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        protected readonly DataContext _context = null;
        protected readonly DbSet<T> table = null;

        protected GenericRepository(DataContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync() => await table.AsNoTracking().ToListAsync();

        public async Task<T> GetByIdLongAsync(long id) => await table.FindAsync(id);

        public async Task<T> GetByIdGUIDAsync(Guid id) => await table.FindAsync(id);

        public async Task InsertAsync(T entity) => await table.AddAsync(entity);

        public async Task UpdateGUIDAsync(T entity, Guid id)
        {
            var obj = await table.FindAsync(id);

            _context.Entry(obj).CurrentValues.SetValues(entity);
        }

        public async Task UpdateLongAsync(T entity, long id)
        {
            var obj = await table.FindAsync(id);

            _context.Entry(obj).CurrentValues.SetValues(entity);
        }

        public void DeleteLong(long id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public void DeleteGuid(Guid id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
    }
}