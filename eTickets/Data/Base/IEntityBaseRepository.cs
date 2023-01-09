using System.Collections.Generic;
using System.Threading.Tasks;

namespace eTickets.Data.Base
{
    // Generic interface where T type is a class
    // Other services or repositories to inherit from this repository will need to inherit from IEntityBase
    public interface IEntityBaseRepository<T> where T : class, IEntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task<T> UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
    }
}
