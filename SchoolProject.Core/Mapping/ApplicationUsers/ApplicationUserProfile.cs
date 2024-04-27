using AutoMapper;

namespace SchoolProject.Core.Mapping.ApplicationUsers
{
    public partial class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            AddUserMapping();
            GetPaginatedUserListMapping();
            GetUserByIDMapping();
        }
    }
}
