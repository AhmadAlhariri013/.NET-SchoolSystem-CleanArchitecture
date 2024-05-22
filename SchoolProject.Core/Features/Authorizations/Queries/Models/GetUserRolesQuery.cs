using MediatR;
using SchoolProject.Core.Basies;
using SchoolProject.Data.Responses;

namespace SchoolProject.Core.Features.Authorization.Queries.Models
{
    public class GetUserRolesQuery : IRequest<Response<GetUserRoleResponse>>
    {
        public int UserId { get; set; }
    }
}
