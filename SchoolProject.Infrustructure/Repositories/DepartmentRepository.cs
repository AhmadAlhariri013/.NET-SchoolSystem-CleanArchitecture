using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Infrustructure.InfrustructureBases;
using SchoolProject.Infrustructure.Interfaces;

namespace SchoolProject.Infrustructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        #region Fields
        private DbSet<Department> _Departments;
        #endregion

        #region Constructor
        public DepartmentRepository(AppDbContext dbContext) : base(dbContext)
        {
            _Departments = dbContext.Set<Department>();
        }
        #endregion


        #region Handle Functions

        #endregion

    }
}
