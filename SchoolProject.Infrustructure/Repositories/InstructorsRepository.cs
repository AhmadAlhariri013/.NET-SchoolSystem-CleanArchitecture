using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.Interfaces;

namespace SchoolProject.Infrustructure.Repositories
{
    public class InstructorsRepository : GenericRepository<Student>, IInstructorsRepository
    {
        #region Fields
        private DbSet<Instructor> Instructors;
        #endregion

        #region Constructor
        public InstructorsRepository(AppDbContext dbContext) : base(dbContext)
        {
            Instructors = dbContext.Set<Instructor>();
        }
        #endregion

        #region Handle Functions
        public Task<Instructor> AddAsync(Instructor entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(ICollection<Instructor> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Instructor entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(ICollection<Instructor> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Instructor entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(ICollection<Instructor> entities)
        {
            throw new NotImplementedException();
        }

        Task<Instructor> IGenericRepository<Instructor>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        IQueryable<Instructor> IGenericRepository<Instructor>.GetTableAsTracking()
        {
            throw new NotImplementedException();
        }

        IQueryable<Instructor> IGenericRepository<Instructor>.GetTableNoTracking()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
