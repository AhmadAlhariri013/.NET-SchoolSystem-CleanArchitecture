using SchoolProject.Data.Entities.Identities;
using SchoolProject.Infrustructure.InfrustructureBases;

namespace SchoolProject.Infrustructure.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<UserRefreshToken>
    {
    }
}
