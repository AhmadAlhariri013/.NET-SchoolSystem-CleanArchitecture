using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.ApplicationUsers.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identities;
using SchoolProject.Service.Interfaces;

namespace SchoolProject.Core.Features.ApplicationUsers.Commands.Handlers
{
    public class UserCommandHandler : ResponseHandler,
                                        IRequestHandler<AddUserCommand, Response<string>>,
                                        IRequestHandler<EditUserCommand, Response<string>>,
                                        IRequestHandler<DeleteUserCommand, Response<string>>,
                                        IRequestHandler<ChangeUserPasswordCommand, Response<string>>




    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailsService _emailsService;
        private readonly IApplicationUserService _applicationUserService;

        #endregion

        #region Constructors
        public UserCommandHandler(IStringLocalizer<SharedResources> localizer,
                              IMapper mapper,
                              UserManager<User> userManager,
                              IHttpContextAccessor httpContextAccessor,
                              IEmailsService emailsService,
                              IApplicationUserService applicationUserService) : base(localizer)
        {
            _localizer = localizer;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailsService = emailsService;
            _applicationUserService = applicationUserService;
        }
        #endregion

        #region Handle Functions

        // Add User Function
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // Mapping
            var userToCreate = _mapper.Map<User>(request);

            // Create User Using "CreateUserAsync" Service
            var createdUser = await _applicationUserService.CreateUserAsync(userToCreate, request.Password);

            // Check on the result of creating user operation
            switch (createdUser)
            {
                case "EmailIsExist": return BadRequest<string>(_localizer[SharedResourcesKeys.EmailIsExist]);
                case "UserNameIsExist": return BadRequest<string>(_localizer[SharedResourcesKeys.UserNameIsExist]);
                case "ErrorInCreateUser": return BadRequest<string>(_localizer[SharedResourcesKeys.FaildToAddUser]);
                case "Failed": return BadRequest<string>(_localizer[SharedResourcesKeys.TryToRegisterAgain]);
                case "Success": return Success<string>("");
                default: return BadRequest<string>(createdUser);
            }


        }

        // Edit User Function
        public async Task<Response<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            // Check if User is exist
            var userToUpdate = await _userManager.FindByIdAsync(request.Id.ToString());
            if (userToUpdate is null) return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);

            // Mapping
            var updatedUser = _mapper.Map(request, userToUpdate);

            //if username is Exist
            var userByUserName = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == updatedUser.UserName && x.Id != updatedUser.Id);
            //username is Exist
            if (userByUserName != null) return BadRequest<string>(_localizer[SharedResourcesKeys.UserNameIsExist]);

            // Updated
            var result = await _userManager.UpdateAsync(updatedUser);

            // Faild
            if (!result.Succeeded) return BadRequest<string>(_localizer[SharedResourcesKeys.UpdateFailed]);

            // Succeeded
            return Success((string)_localizer[SharedResourcesKeys.Updated]);


        }

        // Delete User Function
        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Find The User
            var userToDelete = await _userManager.FindByIdAsync(request.Id.ToString());

            // Check if the user is exist
            if (userToDelete is null) return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);

            // Delete the user
            var result = await _userManager.DeleteAsync(userToDelete);

            // Faild
            if (!result.Succeeded) return BadRequest<string>(_localizer[SharedResourcesKeys.DeletedFailed]);

            // Succeeded
            return Success((string)_localizer[SharedResourcesKeys.Deleted]);
        }


        // Change User Password Function
        public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            // Find The User To Change His Password
            var userToChangePass = await _userManager.FindByNameAsync(request.UserName);

            // Check if it's exist
            if (userToChangePass is null) return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);

            // Change the Password
            var result = await _userManager.ChangePasswordAsync(userToChangePass, request.CurrentPassword, request.NewPassword);

            // Faild
            if (!result.Succeeded) return BadRequest<string>(_localizer[SharedResourcesKeys.ChangePassFailed]);

            // Succeeded
            return Success((string)_localizer[SharedResourcesKeys.Success]);
        }


        #endregion

    }
}
