using System.Linq.Expressions;
using Microsoft.AspNetCore.JsonPatch;

namespace CoreWebApiSuperHero.Data.Repository
{
    public interface ICollegeRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);       
        Task<T> CreateAsync(T tobj);
        Task<T> UpdateAsync(T tobj);            
        Task<bool> DeleteAsync(T tobj);        
    }
}
