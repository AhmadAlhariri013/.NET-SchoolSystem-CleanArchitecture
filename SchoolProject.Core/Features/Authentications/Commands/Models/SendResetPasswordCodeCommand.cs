using MediatR;
using SchoolProject.Core.Basies;

namespace SchoolProject.Core.Features.Authentications.Commands.Models
{
    public class SendResetPasswordCodeCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
    }
}
