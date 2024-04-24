using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Service.Implementations;
using SchoolProject.Service.Interfaces;
namespace SchoolProject.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependency(this IServiceCollection services)
        {
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IDepartmentService, DepartmentService>();

            return services;
        }
    }
}
