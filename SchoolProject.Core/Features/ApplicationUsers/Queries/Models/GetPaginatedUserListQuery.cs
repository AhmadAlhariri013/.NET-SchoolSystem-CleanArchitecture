using MediatR;
using SchoolProject.Core.Features.ApplicationUsers.Queries.Responses;
using SchoolProject.Core.Wrappers;

namespace SchoolProject.Core.Features.ApplicationUsers.Queries.Models
{
    public class GetPaginatedUserListQuery : IRequest<PaginatedResult<GetPaginatedUserListResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
