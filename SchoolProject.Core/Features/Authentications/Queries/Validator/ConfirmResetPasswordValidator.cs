using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authentications.Queries.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.Authentications.Queries.Validator
{
    public class ConfirmResetPasswordValidator : AbstractValidator<ConfirmResetPasswordQuery>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public ConfirmResetPasswordValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Code)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);
            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKeys.NotEmpty])
                 .NotNull().WithMessage(_localizer[SharedResourcesKeys.Required]);

        }

        public void ApplyCustomValidationsRules()
        {
        }

        #endregion

    }
}
