using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.ApplicationUsers.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identities;

namespace SchoolProject.Core.Features.ApplicationUsers.Commands.Handlers
{
    internal class UserCommandHandler : ResponseHandler, IRequestHandler<AddUserCommand, Response<string>>
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
        #endregion

    }
}
