using MediatR;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.Authorization.Queries.Responses;

namespace SchoolProject.Core.Features.Authorization.Queries.Models
{
    public class GetRolesListQuery : IRequest<Response<List<GetRolesListResponse>>>
    {

    }
}
