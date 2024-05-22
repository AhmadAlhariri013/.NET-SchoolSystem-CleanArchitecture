using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Identities;
using SchoolProject.Data.Helpers;
using SchoolProject.Data.Requestes;
using SchoolProject.Data.Responses;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Service.Interfaces;
using System.Security.Claims;

namespace SchoolProject.Service.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        #region Fields
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _dbContext;


        #endregion

        #region Constructors
        public AuthorizationService(RoleManager<Role> roleManager, UserManager<User> userManager, AppDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        #endregion

        #region Handle Functions
        public async Task<string> AddRoleAsync(string roleName)
        {
            // Create an object of Role Entity
            var identityRole = new Role();
            identityRole.Name = roleName;

            // Create a specified role 
            var result = await _roleManager.CreateAsync(identityRole);

            // Success
            if (result.Succeeded)
                return "Success";

            // Failed
            return "Failed";

        }

        public async Task<bool> IsRoleExistByName(string roleName)
        {
            // Return If the role exist or not
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<string> EditRoleAsync(int id, string name)
        {
            // Check if the role is exist or not
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                return "NotFound";

            // Update the Role's Name
            role.Name = name;
            var result = await _roleManager.UpdateAsync(role);

            // Success
            if (result.Succeeded) return "Success";

            // Faild
            var errors = string.Join("-", result.Errors);
            return errors;

        }

        public async Task<bool> IsRoleExistById(int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return false;
            return true;
        }

        public async Task<string> DeleteRoleAsync(int id)
        {
            // Check if the role is exist or not
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role is null) return "NotFound";

            // If Exist:

            // Check if any user has this role or not
            var users = await _userManager.GetUsersInRoleAsync(role.Name);
            // Return Exception
            if (users is not null && users.Count() > 0) return "Used";

            // Delete the role if no user has this role
            var result = await _roleManager.DeleteAsync(role);

            // Success 
            if (result.Succeeded) return "Success";

            // Faild because some errors
            var errors = string.Join("-", result.Errors);
            return errors;


        }

        public async Task<List<Role>> GetRolesList()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<GetUserRoleResponse> GetUserRoles(User user)
        {

            // Get All Roles in the project
            var allRoles = await _roleManager.Roles.ToListAsync();

            // Create a list of "UserRoles" DTO 
            var UserRoles = new List<UserRoles>();

            // Create a response object of "GetUserRoleResponse" and assing values to it
            var response = new GetUserRoleResponse();

            response.UserId = user.Id;

            // loop in allRoles and assing values to the UserRole DTO object
            foreach (var role in allRoles)
            {
                // Create an object of "UserRoles" DTO and assing values to it
                var userRole = new UserRoles();
                userRole.RoleId = role.Id;
                userRole.RoleName = role.Name;

                // Check if the user has a role matches with any role in "allRoles"
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsInRole = true;
                }
                else
                {
                    userRole.IsInRole = false;
                }

                // add the object to the list of UserRoles DTO
                UserRoles.Add(userRole);
            }

            response.UserRoles = UserRoles;

            // Return ManageUserRoleResponse
            return response;

        }

        public async Task<string> UpdateUserRoles(UpdateUserRolesRequest request)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Get the user you want to update his "Roles"
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());

                // Check if it's exist or not
                if (user is null) return "NotFound";

                // Get Old User Roles
                var oldUserRoles = await _userManager.GetRolesAsync(user);

                // Remove Old User Roles
                var removeResult = await _userManager.RemoveFromRolesAsync(user, oldUserRoles);
                // Check If The Remove operation Successed or not
                if (!removeResult.Succeeded) return "RemoveOldUserRolesFaild";

                // Add New Roles To The User (the roles that its "isInRole" propery is "true")
                var userRolesToAdd = request.UserRoles.Where(x => x.IsInRole is true).Select(x => x.RoleName);
                var addResult = await _userManager.AddToRolesAsync(user, userRolesToAdd);

                // Check If The Add operation Successed or not
                if (!addResult.Succeeded) return "AddNewUserRolesFaild";

                await transaction.CommitAsync();
                // Return Result
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return "UpdateUserRolesFaild";
            }
        }

        public async Task<GetUserClaimsResponse> GetUserClaims(User user)
        {
            // Get User Claims
            var userClaims = await _userManager.GetClaimsAsync(user);

            // Get All Claims in the project
            var allClaims = ClaimsStore.Claims;

            // Create a list of "UserClaims" DTO 
            var claims = new List<UserClaims>();

            // Create a response object of "GetUserClaimsResponse" and assing values to it
            var response = new GetUserClaimsResponse();

            response.UserId = user.Id;

            foreach (var claim in allClaims)
            {
                var userClaim = new UserClaims();

                userClaim.ClaimType = claim.Type;

                if (userClaims.Any(x => x.Type == claim.Type))
                {
                    userClaim.ClaimValue = true;
                }
                else
                {
                    userClaim.ClaimValue = false;
                }

                claims.Add(userClaim);
            }

            response.UserClaims = claims;
            return response;
        }

        public async Task<string> UpdateUserClaims(UpdateUserClaimsRequest request)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Get the user you want to update his "Claims"
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());

                // Check if it's exist or not
                if (user is null) return "NotFound";

                // Get Old User Claims
                var oldUserClaims = await _userManager.GetClaimsAsync(user);

                // Remove Old User Roles
                var removeResult = await _userManager.RemoveClaimsAsync(user, oldUserClaims);
                // Check If The Remove operation Successed or not
                if (!removeResult.Succeeded) return "RemoveOldUserRolesFaild";

                // Add New Roles To The User (the roles that its "isInRole" propery is "true")
                var userClaimsToAdd = request.UserClaims.Where(x => x.ClaimValue is true).Select(x => new Claim(x.ClaimType, x.ClaimValue.ToString()));
                var addResult = await _userManager.AddClaimsAsync(user, userClaimsToAdd);

                // Check If The Add operation Successed or not
                if (!addResult.Succeeded) return "AddNewUserRolesFaild";

                await transaction.CommitAsync();
                // Return Result
                return "Success";
            }
            catch
            {
                await transaction.RollbackAsync();
                return "UpdateUserClaimsFaild";
            }

        }

        #endregion
    }
}
