using MediatR;
using SchoolProject.Core.Basies;

namespace SchoolProject.Core.Features.Authentications.Commands.Models
{
    public class ResetPasswordCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
