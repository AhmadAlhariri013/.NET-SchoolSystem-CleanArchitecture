using MediatR;
using SchoolProject.Core.Basies;

namespace SchoolProject.Core.Features.ApplicationUsers.Commands.Models
{
    public class DeleteUserCommand : IRequest<Response<string>>
    {
        public string Id { get; set; }

        public DeleteUserCommand(string id)
        {
            Id = id;
        }
    }
}
