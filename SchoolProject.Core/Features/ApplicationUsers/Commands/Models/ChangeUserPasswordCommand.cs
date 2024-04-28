using MediatR;
using SchoolProject.Core.Basies;

namespace SchoolProject.Core.Features.ApplicationUsers.Commands.Models
{
    public class ChangeUserPasswordCommand : IRequest<Response<string>>
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
