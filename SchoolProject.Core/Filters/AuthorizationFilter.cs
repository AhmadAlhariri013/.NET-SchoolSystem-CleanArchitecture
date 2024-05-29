using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SchoolProject.Service.AuthServices.Interfaces;

namespace SchoolProject.Core.Filters
{
    public class AuthorizationFilter : IAsyncActionFilter
    {
        #region Fields
        private readonly ICurrentUserService _currentUserService;

        #endregion

        #region Constructors
        public AuthorizationFilter(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }
        #endregion

        #region Functions
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get The Roles of the user
            var roles = await _currentUserService.GetCurrentUserRoles();

            if (roles.All(role => !role.Equals("User")))
            {
                context.Result = new ObjectResult("Forbidden")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            else
            {
                await next();
            }

        }
        #endregion

    }
}
