﻿using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Features.Authentications.Commands.Models;
using SchoolProject.Core.Resources;

namespace SchoolProject.Core.Features.Authentications.Commands.Validators
{
    public class SendResetPasswordCommandValidator : AbstractValidator<SendResetPasswordCodeCommand>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public SendResetPasswordCommandValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
            ApplyCustomValidationsRules();
        }
        #endregion

        #region Actions
        public void ApplyValidationsRules()
        {
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
