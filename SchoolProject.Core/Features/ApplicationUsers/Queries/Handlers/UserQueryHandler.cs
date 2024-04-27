using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.ApplicationUsers.Queries.Models;
using SchoolProject.Core.Features.ApplicationUsers.Queries.Responses;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities.Identities;

namespace SchoolProject.Core.Features.ApplicationUsers.Queries.Handlers
{
    public class UserQueryHandler : ResponseHandler,
        IRequestHandler<GetPaginatedUserListQuery, PaginatedResult<GetPaginatedUserListResponse>>,
        IRequestHandler<GetUserByIDQuery, Response<GetUserByIDResponse>>

    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _sharedResources;
        private readonly UserManager<User> _userManager;
        #endregion

        #region Constructors
        public UserQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  IMapper mapper,
                                  UserManager<User> userManager) : base(stringLocalizer)
        {
            _mapper = mapper;
            _sharedResources = stringLocalizer;
            _userManager = userManager;
        }
        #endregion

        #region Handle Functions

        public Task<PaginatedResult<GetPaginatedUserListResponse>> Handle(GetPaginatedUserListQuery request, CancellationToken cancellationToken)
        {
            // Get querable list of users
            var users = _userManager.Users.AsQueryable();

            // Mapping
            var usersPaginatedList = _mapper.ProjectTo<GetPaginatedUserListResponse>(users).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            // Return User List
            return usersPaginatedList;
        }

        public async Task<Response<GetUserByIDResponse>> Handle(GetUserByIDQuery request, CancellationToken cancellationToken)
        {
            //var user = _userManager.FindByIdAsync(request.Id);
            var user = await _userManager.Users.Where(x => x.Id.Equals(request.Id)).FirstOrDefaultAsync();

            if (user == null) return NotFound<GetUserByIDResponse>(_sharedResources[SharedResourcesKeys.NotFound]);

            var userResult = _mapper.Map<GetUserByIDResponse>(user);

            return Success(userResult);

        }
        #endregion

    }
}
