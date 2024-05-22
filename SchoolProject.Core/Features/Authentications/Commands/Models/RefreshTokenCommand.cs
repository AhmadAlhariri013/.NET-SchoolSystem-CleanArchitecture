using MediatR;
using SchoolProject.Core.Basies;
using SchoolProject.Data.Responses;

namespace SchoolProject.Core.Features.Authentications.Commands.Models
{
    public class RefreshTokenCommand : IRequest<Response<JwtAuthResponse>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
