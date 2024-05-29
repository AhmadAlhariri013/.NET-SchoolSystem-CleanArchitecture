using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SchoolProject.Data.Entities.Identities;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.AuthServices.Interfaces;

namespace SchoolProject.Service.AuthServices.Implementations
{
    public class CurrentUserService : ICurrentUserService
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        #endregion
        #region Constructors
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        #endregion
        #region Functions
        public async Task<User> GetCurrentUser()
        {
            var userId = GetCurrentUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new UnauthorizedAccessException();
            return user;
        }

        public int GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(claim => claim.Type == nameof(UserClaimModel.Id)).Value;
            if (userId == null) throw new UnauthorizedAccessException();
            return int.Parse(userId);
        }

        public async Task<List<string>> GetCurrentUserRoles()
        {

            var user = await GetCurrentUser();

            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToList();
        }
        #endregion
    }
}
