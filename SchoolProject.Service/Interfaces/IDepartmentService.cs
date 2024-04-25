using SchoolProject.Data.Entities;


namespace SchoolProject.Service.Interfaces
{
    public interface IDepartmentService
    {
        public Task<Department> GetDepartmentByID(int id);
        public Task<bool> IsDepartmentIdExist(int departmentId);
    }
}
