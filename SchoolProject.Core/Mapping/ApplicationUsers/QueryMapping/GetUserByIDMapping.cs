using SchoolProject.Core.Features.ApplicationUsers.Queries.Responses;
using SchoolProject.Data.Entities.Identities;

namespace SchoolProject.Core.Mapping.ApplicationUsers
{
    public partial class ApplicationUserProfile
    {
        public void GetUserByIDMapping()
        {
            CreateMap<User, GetUserByIDResponse>();
        }
    }
}
