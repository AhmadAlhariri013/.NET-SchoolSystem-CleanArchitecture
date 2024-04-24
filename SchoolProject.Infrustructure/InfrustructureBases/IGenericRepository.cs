using Microsoft.EntityFrameworkCore.Storage;

namespace SchoolProject.Infrustructure.InfrustructureBases
{
    public interface IGenericRepository<T> where T : class
    {

        // Commands and Queries
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(ICollection<T> entities);
        Task SaveChangesAsync();

        //Transactions and Tracking
        IDbContextTransaction BeginTransaction();
        void Commit();
        void RollBack();
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();

    }

}
