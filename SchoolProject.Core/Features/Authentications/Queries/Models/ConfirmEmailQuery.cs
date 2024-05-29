using MediatR;
using SchoolProject.Core.Basies;

namespace SchoolProject.Core.Features.Authentications.Queries.Models
{
    public class ConfirmEmailQuery : IRequest<Response<string>>
    {
        public int UserId { get; set; }
        public string Code { get; set; }
    }
}
