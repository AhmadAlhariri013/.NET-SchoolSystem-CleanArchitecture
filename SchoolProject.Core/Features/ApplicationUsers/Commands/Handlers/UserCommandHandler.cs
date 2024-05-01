using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.ApplicationUsers.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identities;

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

        #endregion

        #region Constructors
        public UserCommandHandler(IStringLocalizer<SharedResources> localizer,
                              IMapper mapper,
                              UserManager<User> userManager) : base(localizer)
        {
            _localizer = localizer;
            _mapper = mapper;
            _userManager = userManager;
        }
        #endregion

        #region Handle Functions

        // Add User Function
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // Check if user is exist by finding it by eamil
            var userToMap = await _userManager.FindByEmailAsync(request.Email);
            if (userToMap != null) return BadRequest<string>(_localizer[SharedResourcesKeys.EmailIsExist]);

            // Check if user is exist by finding it by username
            var userByUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userByUserName != null) return BadRequest<string>(_localizer[SharedResourcesKeys.EmailIsExist]);

            // Mapping
            var userMapper = _mapper.Map<User>(request);

            // Create User
            var createdUser = await _userManager.CreateAsync(userMapper, request.Password);
            //Faild
            if (!createdUser.Succeeded)
            {
                return BadRequest<string>(createdUser.Errors.FirstOrDefault().Description);
            }
            //Succeeded
            return Created("");

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
