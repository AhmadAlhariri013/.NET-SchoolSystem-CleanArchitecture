using MediatR;
using SchoolProject.Core.Basies;

namespace SchoolProject.Core.Features.ApplicationUsers.Commands.Models
{
    public class DeleteUserCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}
