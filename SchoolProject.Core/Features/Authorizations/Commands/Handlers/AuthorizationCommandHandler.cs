using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.Authorization.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identities;
using SchoolProject.Service.Interfaces;

namespace SchoolProject.Core.Features.Authorizations.Commands.Handlers
{


    public class AuthorizationCommandHandler : ResponseHandler,
                                               IRequestHandler<AddRoleCommand, Response<string>>,
                                               IRequestHandler<EditRoleCommand, Response<string>>,
                                               IRequestHandler<DeleteRoleCommand, Response<string>>,
                                               IRequestHandler<UpdateUserRolesCommand, Response<string>>,
                                               IRequestHandler<UpdateUserClaimsCommand, Response<string>>





    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        #endregion

        #region Constructors
        public AuthorizationCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                         IAuthorizationService authorizationService,
                                         IMapper mapper,
                                         UserManager<User> userManager) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _authorizationService = authorizationService;
            _mapper = mapper;
            _userManager = userManager;
        }

        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            // Use "AddRoleAsync" Service To Add Role
            var result = await _authorizationService.AddRoleAsync(request.RoleName);

            //Check If the Add Succeded
            if (result == "Success") return Success("");

            // Falid To Add
            return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.AddFailed]);
        }

        public async Task<Response<string>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            // Use "AddRoleAsync" Service To Edit Role
            var result = await _authorizationService.EditRoleAsync(request.Id, request.Name);

            // If the role you wanted to edit it "Not Found"
            if (result == "NotFound") return NotFound<string>();

            // If it exist and succeded to edit it
            else if (result == "Success") return Success((string)_stringLocalizer[SharedResourcesKeys.Updated]);

            // Any other problems
            else return BadRequest<string>(result);
        }

        public async Task<Response<string>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            // Use "DeleteRoleAsync" Service To  Delete Role
            var result = await _authorizationService.DeleteRoleAsync(request.Id);

            // If the role you wanted to delete it "Not Found"
            if (result == "NotFound") return NotFound<string>();

            // If the role you wanted to delete it "Used by users"
            else if (result == "Used") return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.RoleIsUsed]);

            // If it exist, not used by other users, and succeded to Delete it
            else if (result == "Success") return Success((string)_stringLocalizer[SharedResourcesKeys.Deleted]);

            // Any other problems
            else return BadRequest<string>(result);
        }
        public async Task<Response<string>> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            // Use "DeleteRoleAsync" Service To Update User's Roles
            var result = await _authorizationService.UpdateUserRoles(request);

            // Faild To These Problems
            switch (result)
            {
                case "NotFound": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
                case "RemoveOldUserRolesFaild": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToRemoveOldRoles]);
                case "AddNewUserRolesFaild": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewRoles]);
                case "UpdateUserRolesFaild": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToUpdateUserRoles]);
            }

            // Success
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }

        public async Task<Response<string>> Handle(UpdateUserClaimsCommand request, CancellationToken cancellationToken)
        {
            // Use "DeleteRoleAsync" Service To Update User's Claims
            var result = await _authorizationService.UpdateUserClaims(request);

            // Faild To These Problems
            switch (result)
            {
                case "UserIsNull": return NotFound<string>(_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);
                case "FailedToRemoveOldClaims": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToRemoveOldClaims]);
                case "FailedToAddNewClaims": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToAddNewClaims]);
                case "FailedToUpdateClaims": return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.FailedToUpdateClaims]);
            }

            // Success
            return Success<string>(_stringLocalizer[SharedResourcesKeys.Success]);
        }
        #endregion
    }
}
