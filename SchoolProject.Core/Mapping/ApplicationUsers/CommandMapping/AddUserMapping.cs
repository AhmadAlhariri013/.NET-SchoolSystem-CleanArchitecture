using SchoolProject.Core.Features.ApplicationUsers.Commands.Models;
using SchoolProject.Data.Entities.Identities;

namespace SchoolProject.Core.Mapping.ApplicationUsers
{
    public partial class ApplicationUserProfile
    {
        public void AddUserMapping()
        {
            CreateMap<AddUserCommand, User>();
        }
    }
}
