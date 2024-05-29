using SchoolProject.Data.Entities.Identities;

namespace SchoolProject.Service.Interfaces
{
    public interface IApplicationUserService
    {
        public Task<string> CreateUserAsync(User user, string password);
    }
}
