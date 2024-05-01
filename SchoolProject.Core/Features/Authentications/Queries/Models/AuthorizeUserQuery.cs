using MediatR;
using SchoolProject.Core.Basies;

namespace SchoolProject.Core.Features.Authentications.Queries.Models
{
    public class AuthorizeUserQuery : IRequest<Response<string>>
    {
        public string AccessToken { get; set; }
    }
}
