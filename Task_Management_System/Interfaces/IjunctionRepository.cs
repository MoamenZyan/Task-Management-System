namespace Project.API.Interfaces
{
    public interface IJunctionRepository<T>
    {
        Task<T?> AddAsync(T entity);
        Task<T?> GetById(int userId, int entityId);
        IEnumerable<T?> Filter(Func<T, bool> func);
        Task<bool> DeleteAsync(int userId, int entityId);
        Task<bool> UpdateAsync(T entity);
    }
}
