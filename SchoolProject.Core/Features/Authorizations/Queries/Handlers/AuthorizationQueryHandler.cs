using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.Authorization.Queries.Models;
using SchoolProject.Core.Features.Authorization.Queries.Responses;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identities;
using SchoolProject.Data.Responses;
using SchoolProject.Service.Interfaces;

namespace SchoolProject.Core.Features.Authorization.Queries.Handlers
{
    public class AuthorizationQueryHandler : ResponseHandler,
                                             IRequestHandler<GetRolesListQuery, Response<List<GetRolesListResponse>>>,
                                             IRequestHandler<GetRoleByIdQuery, Response<GetRoleByIdResponse>>,
                                             IRequestHandler<GetUserRolesQuery, Response<GetUserRoleResponse>>,
                                             IRequestHandler<GetUserClaimsQuery, Response<GetUserClaimsResponse>>



    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        #endregion

        #region Constructors
        public AuthorizationQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
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
        public async Task<Response<List<GetRolesListResponse>>> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
        {
            var roles = await _authorizationService.GetRolesList();
            var result = _mapper.Map<List<GetRolesListResponse>>(roles);
            return Success(result);
        }

        public async Task<Response<GetRoleByIdResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            // Use "GetRoleById" service to get the role
            var role = await _authorizationService.GetRoleById(request.Id);

            // Check if the role exist or not
            if (role is null) return NotFound<GetRoleByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            // Mapping
            var result = _mapper.Map<GetRoleByIdResponse>(role);

            // Success
            return Success(result);
        }

        public async Task<Response<GetUserRoleResponse>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            // Get The User That wanted to Get his "Roles"
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            // Check if the user exist or not
            if (user is null) return NotFound<GetUserRoleResponse>((string)_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);

            // Use "ManageUserRoles" Service to get and manage the roles for this user
            var result = await _authorizationService.GetUserRoles(user);

            return Success(result);
        }

        public async Task<Response<GetUserClaimsResponse>> Handle(GetUserClaimsQuery request, CancellationToken cancellationToken)
        {
            // Get The User That wanted to Get his "Roles"
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            // Check if the user exist or not
            if (user is null) return NotFound<GetUserClaimsResponse>((string)_stringLocalizer[SharedResourcesKeys.UserIsNotFound]);

            // Use "ManageUserRoles" Service to get and manage the roles for this user
            var result = await _authorizationService.GetUserClaims(user);

            return Success(result);
        }
        #endregion
    }
}
