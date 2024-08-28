namespace EventSystem.Helpers
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdLongAsync(long id);
        Task<T> GetByIdGUIDAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task InsertAsync(T entity);
        void DeleteLong(long id);
        void DeleteGuid(Guid id);
        Task UpdateGUIDAsync(T entity, Guid id);
        Task UpdateLongAsync(T entity, long id);
    }
}
