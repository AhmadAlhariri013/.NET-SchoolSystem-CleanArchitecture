using MediatR;
using SchoolProject.Core.Basies;

namespace SchoolProject.Core.Features.Authentications.Queries.Models
{
    public class ConfirmResetPasswordQuery : IRequest<Response<string>>
    {
        public string Code { get; set; }
        public string Email { get; set; }
    }
}
