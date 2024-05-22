using MediatR;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.Authorization.Queries.Responses;

namespace SchoolProject.Core.Features.Authorization.Queries.Models
{
    public class GetRoleByIdQuery : IRequest<Response<GetRoleByIdResponse>>
    {
        public int Id { get; set; }

        public GetRoleByIdQuery(int id)
        {
            Id = id;
        }
    }
}
