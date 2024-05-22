using MediatR;
using SchoolProject.Core.Basies;
using SchoolProject.Data.Responses;

namespace SchoolProject.Core.Features.Authorization.Queries.Models
{
    public class GetUserClaimsQuery : IRequest<Response<GetUserClaimsResponse>>
    {
        public int UserId { get; set; }
    }
}
