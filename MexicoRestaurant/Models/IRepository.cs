using System.Linq.Expressions;
namespace MexicoRestaurant.Models
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id, QueryOptions<T> options); // QueryOptions allows for filtering, sorting,...
        Task<IEnumerable<T>> GetAllByIdAsync<TKey>(TKey id , string propreryName , QueryOptions<T> options);
        Task AddAsync(T entity);
        Task UdateAsunc(T entity);
        Task DeleteAsync(int id);

    }
}
