using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.Interfaces;

namespace SchoolProject.Infrustructure.Repositories
{
    public class SubjectRepository : GenericRepository<Student>, ISubjectRepository
    {
        #region Fields
        private DbSet<Subject> Subjects;
        #endregion

        #region Constructor
        public SubjectRepository(AppDbContext dbContext) : base(dbContext)
        {
            Subjects = dbContext.Set<Subject>();
        }
        #endregion

        #region Handle Functions
        public Task<Subject> AddAsync(Subject entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(ICollection<Subject> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Subject entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(ICollection<Subject> entities)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Subject entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(ICollection<Subject> entities)
        {
            throw new NotImplementedException();
        }

        Task<Subject> IGenericRepository<Subject>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        IQueryable<Subject> IGenericRepository<Subject>.GetTableAsTracking()
        {
            throw new NotImplementedException();
        }

        IQueryable<Subject> IGenericRepository<Subject>.GetTableNoTracking()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
