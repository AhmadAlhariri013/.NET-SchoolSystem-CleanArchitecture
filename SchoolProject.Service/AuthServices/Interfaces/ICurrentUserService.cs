using SchoolProject.Data.Entities.Identities;

namespace SchoolProject.Service.AuthServices.Interfaces
{
    public interface ICurrentUserService
    {
        public int GetCurrentUserId();
        public Task<User> GetCurrentUser();
        public Task<List<string>> GetCurrentUserRoles();

    }
}
