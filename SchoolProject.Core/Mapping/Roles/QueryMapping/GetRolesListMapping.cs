using SchoolProject.Core.Features.Authorization.Queries.Responses;
using SchoolProject.Data.Entities.Identities;


namespace SchoolProject.Core.Mapping.Roles
{
    public partial class RoleProfile
    {
        public void GetRolesListMapping()
        {
            CreateMap<Role, GetRolesListResponse>();
        }
    }
}