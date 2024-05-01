using SchoolProject.Data.Entities.Identities;
using SchoolProject.Data.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace SchoolProject.Service.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<JwtAuthResponse> GetJWTToken(User user);
        public JwtSecurityToken ReadJWTToken(string accessToken);
        public Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshToken);
        public JwtAuthResponse GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expireDate, string refreshToken);
        public Task<string> ValidateToken(string accessToken);
    }
}
