
using System.Linq.Expressions;

namespace CoreWebApiSuperHero.Data.Repository
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        private readonly CollegeDBContext _collegeDBContext;
        private readonly DbSet<T> _dbSet; // DbSet to perform CRUD operations on the entity type T

        public CollegeRepository(CollegeDBContext collegeDBContext)
        {
            _collegeDBContext = collegeDBContext;
            _dbSet = _collegeDBContext.Set<T>(); // Initialize the DbSet for the entity type T
        }

        public async Task<T> CreateAsync(T tobj)
        {
            await  _dbSet.AddAsync(tobj);
            await _collegeDBContext.SaveChangesAsync();
            return tobj;
        }
        public async Task<T> UpdateAsync(T tobj)
        {
            _dbSet.Update(tobj);
            await _collegeDBContext.SaveChangesAsync();
            return tobj;
        }

        public async Task<bool> DeleteAsync(T tobj)
        {
            _dbSet.Remove(tobj);
            await _collegeDBContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, bool useNoTracking)
        {
            if (useNoTracking)
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        /*public async Task<T> GetByIdAsync(Expression<Func<T,bool>> filter, bool useNoTracking)
        {
            if (useNoTracking)
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }*/
               
    }
}
