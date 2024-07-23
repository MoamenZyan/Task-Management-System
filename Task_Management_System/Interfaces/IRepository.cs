namespace Project.API.Interfaces
{
    public interface IRepository<T>
    {
        Task<T?> AddAsync(T entity);
        Task<T?> GetById(int id);
        Task<IEnumerable<T?>> GetAllAsync();
        IEnumerable<T?> Filter(Func<T, bool> func);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(T entity);
    }
}
