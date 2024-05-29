using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.Authentications.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identities;
using SchoolProject.Data.Responses;
using SchoolProject.Service.Interfaces;

namespace SchoolProject.Core.Features.Authentications.Commands.Handlers
{
    public class AuthenticationCommandHandler : ResponseHandler,
                                                IRequestHandler<SignInCommand, Response<JwtAuthResponse>>,
                                                IRequestHandler<RefreshTokenCommand, Response<JwtAuthResponse>>,
                                                IRequestHandler<SendResetPasswordCodeCommand, Response<string>>,
                                                IRequestHandler<ResetPasswordCommand, Response<string>>



    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthenticationService _authenticationService;

        #endregion

        #region Constructors
        public AuthenticationCommandHandler(IStringLocalizer<SharedResources> localizer,
                                            UserManager<User> userManager,
                                            SignInManager<User> signInManager,
                                            IAuthenticationService authenticationService) : base(localizer)
        {
            _localizer = localizer;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationService = authenticationService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<JwtAuthResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {

            //Find The User By UserName
            var user = await _userManager.FindByNameAsync(request.UserName);

            // Check The User If Exist
            if (user is null) return BadRequest<JwtAuthResponse>(_localizer[SharedResourcesKeys.UserNameIsNotExist]);


            // Check The Password If Correct
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!signInResult.Succeeded) return BadRequest<JwtAuthResponse>(_localizer[SharedResourcesKeys.PasswordNotCorrect]);

            // Check The Email If Confirmed
            if (!user.EmailConfirmed) return BadRequest<JwtAuthResponse>(_localizer[SharedResourcesKeys.EmailNotConfirmed]);

            // Generate Token
            var tokenResult = await _authenticationService.GetJWTToken(user);

            // Return Token
            return Success(tokenResult);
        }

        public async Task<Response<JwtAuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Read the Access Token from the request
            var jwtToken = _authenticationService.ReadJWTToken(request.AccessToken);

            // Validate Token and Refresh Token By performing various validations on the provided JWT token, access token, and refresh token 
            var userIdAndExpireDate = await _authenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);

            // Handle different validation outcomes based on the value returned by ValidateDetails
            switch (userIdAndExpireDate)
            {
                case ("AlgorithmIsWrong", null): return Unauthorized<JwtAuthResponse>(_localizer[SharedResourcesKeys.AlgorithmIsWrong]);
                case ("TokenIsNotExpired", null): return Unauthorized<JwtAuthResponse>(_localizer[SharedResourcesKeys.TokenIsNotExpired]);
                case ("RefreshTokenIsNotFound", null): return Unauthorized<JwtAuthResponse>(_localizer[SharedResourcesKeys.RefreshTokenIsNotFound]);
                case ("RefreshTokenIsExpired", null): return Unauthorized<JwtAuthResponse>(_localizer[SharedResourcesKeys.RefreshTokenIsExpired]);
            }

            // Unpacks the tuple returned by ValidateDetails
            var (userId, expiryDate) = userIdAndExpireDate;

            // Find the user with the retrieved userId
            var user = await _userManager.FindByIdAsync(userId);
            // Chek If it's Exist
            if (user == null)
            {
                return NotFound<JwtAuthResponse>();
            }

            // Generates a new refresh token string
            var result = await _authenticationService.GetRefreshToken(user, jwtToken, expiryDate, request.RefreshToken);

            // Returns a JwtAuthResult object containing the new access token and refresh token details.
            return Success(result);
        }

        public async Task<Response<string>> Handle(SendResetPasswordCodeCommand request, CancellationToken cancellationToken)
        {
            // Use "SendResetPassword" Service to send an email that contains a code to reset the password
            var result = await _authenticationService.SendResetPasswordCode(request.Email);

            // Check on the result of sending reset password code to the user 
            switch (result)
            {
                case "NotFound": return BadRequest<string>(_localizer[SharedResourcesKeys.UserIsNotFound]);
                case "FaildToUpdateTheUser": return BadRequest<string>(_localizer[SharedResourcesKeys.TryAgainInAnotherTime]);
                case "Failed": return BadRequest<string>(_localizer[SharedResourcesKeys.TryAgainInAnotherTime]);
                case "Success": return Success<string>("");
                default: return BadRequest<string>(_localizer[SharedResourcesKeys.TryAgainInAnotherTime]);
            }


        }

        public async Task<Response<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Use "ResetPassword" Service to make the user reset his password after he confirmed the proccess by the code that receved on the email
            var result = await _authenticationService.ResetPassword(request.Email, request.Password);

            // Check on the result of confirming the reset password proccess
            switch (result)
            {
                case "NotFound": return BadRequest<string>(_localizer[SharedResourcesKeys.UserIsNotFound]);
                case "Failed": return BadRequest<string>(_localizer[SharedResourcesKeys.InvaildCode]);
                case "Success": return Success<string>("");
                default: return BadRequest<string>(_localizer[SharedResourcesKeys.InvaildCode]);
            }
        }

        #endregion

    }
}
