using SchoolProject.Data.Entities.Identities;
using SchoolProject.Data.Requestes;
using SchoolProject.Data.Responses;

namespace SchoolProject.Service.Interfaces
{
    public interface IAuthorizationService
    {
        public Task<bool> IsRoleExistByName(string roleName);
        public Task<bool> IsRoleExistById(int roleId);
        public Task<List<Role>> GetRolesList();
        public Task<Role> GetRoleById(int id);
        public Task<string> AddRoleAsync(string roleName);
        public Task<string> EditRoleAsync(int id, string name);
        public Task<string> DeleteRoleAsync(int id);
        public Task<GetUserRoleResponse> GetUserRoles(User user);
        public Task<string> UpdateUserRoles(UpdateUserRolesRequest request);
        public Task<GetUserClaimsResponse> GetUserClaims(User user);
        public Task<string> UpdateUserClaims(UpdateUserClaimsRequest request);



    }
}
