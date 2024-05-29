using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Data.Entities.Identities;
using SchoolProject.Infrustructure.Data;
using SchoolProject.Service.Interfaces;

namespace SchoolProject.Service.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailsService _emailsService;
        private readonly AppDbContext _dbContext;
        private readonly IUrlHelper _urlHelper;
        #endregion

        #region Constructors
        public ApplicationUserService(UserManager<User> userManager,
                                      IHttpContextAccessor httpContextAccessor,
                                      IEmailsService emailsService,
                                      AppDbContext dbContext,
                                      IUrlHelper urlHelper)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailsService = emailsService;
            _dbContext = dbContext;
            _urlHelper = urlHelper;
        }


        #endregion

        #region Handle Functions
        public async Task<string> CreateUserAsync(User user, string password)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Check if user's email is exist 
                var userToMap = await _userManager.FindByEmailAsync(user.Email);
                if (userToMap != null) return "EmailIsExist";

                // Check if username is exist 
                var userByUserName = await _userManager.FindByNameAsync(user.UserName);
                if (userByUserName != null) return "UserNameIsExist";

                // Create User
                var createdUser = await _userManager.CreateAsync(user, password);

                // Faild To Create the user
                if (!createdUser.Succeeded) return string.Join(",", createdUser.Errors.Select(x => x.Description).ToList());


                // Send Confirmation Email 
                var code = _userManager.GenerateEmailConfirmationTokenAsync(user);

                var requestAccessor = _httpContextAccessor.HttpContext.Request;

                var returnUrl = requestAccessor.Scheme + "" + requestAccessor.Host + _urlHelper.Action("ConfirmEmail", "Authentication", new { userId = user.Id, code = code });
                //$"/Api/V1/Authentication/ConfirmEmail?userId={user.Id}&code={code}";

                var message = $"To Confirm Email Click Link: <a href='{returnUrl}'></a>";

                await _emailsService.SendEmail(user.Email, message, "Email Confrimation");


                await transaction.CommitAsync();

                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return "Failed";
            }

        }
        #endregion
    }
}
