using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Interfaces;
using SchoolProject.Service.Interfaces;
//using Serilog;

namespace SchoolProject.Service.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        #region Fields
        private readonly IDepartmentRepository _departmentRepository;
        #endregion

        #region Constructors

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<Department> GetDepartmentByID(int id)
        {
            var department = await _departmentRepository.GetTableNoTracking()
                .Where(x => x.DID.Equals(id))
                .Include(x => x.Instructors)
                .Include(x => x.Instructor)
                .Include(x => x.DepartmentSubjects).ThenInclude(x => x.Subject)
                .FirstOrDefaultAsync();

            return department;
        }
        #endregion
    }
}
