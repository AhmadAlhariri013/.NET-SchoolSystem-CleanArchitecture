using MediatR;
using SchoolProject.Core.Basies;
using SchoolProject.Data.Requestes;

namespace SchoolProject.Core.Features.Authorization.Commands.Models
{
    public class UpdateUserClaimsCommand : UpdateUserClaimsRequest, IRequest<Response<string>>
    {
    }
}
