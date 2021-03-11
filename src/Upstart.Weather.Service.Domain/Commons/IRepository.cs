using System.Collections.Generic;
using System.Threading.Tasks;

namespace Upstart.Weather.Service.Domain.Commons
{
    public interface IRepository<TEntity, in TId> where TEntity : class
    {
        Task<int> Insert(TEntity entity);
        Task<int> InsertInBulkMode(IEnumerable<TEntity> entity);
        Task<int> Update(TEntity entity);
        Task<int> UpdateInBulkMode(IEnumerable<TEntity> entity);
        Task<int> Deactivate(TId id);
        Task<int> DeactivateInBulkMode(IEnumerable<TId> id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get();
    }
}
