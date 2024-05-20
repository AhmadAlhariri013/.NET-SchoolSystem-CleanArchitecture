using MediatR;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.ApplicationUsers.Queries.Responses;

namespace SchoolProject.Core.Features.ApplicationUsers.Queries.Models
{
    public class GetUserByIDQuery : IRequest<Response<GetUserByIDResponse>>
    {
        public string Id { get; set; }

        public GetUserByIDQuery(string id)
        {
            Id = id;
        }

    }
}
