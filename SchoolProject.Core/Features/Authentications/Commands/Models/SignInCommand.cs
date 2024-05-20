using MediatR;
using SchoolProject.Core.Basies;
using SchoolProject.Data.Helpers;

namespace SchoolProject.Core.Features.Authentications.Commands.Models
{
    public class SignInCommand : IRequest<Response<JwtAuthResponse>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
