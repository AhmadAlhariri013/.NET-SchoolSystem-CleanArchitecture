using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.Emails.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Service.Interfaces;

namespace SchoolProject.Core.Features.Emails.Commands.Handlers
{
    public class EmailCommandHandler : ResponseHandler, IRequestHandler<SendEmailCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IEmailsService _emailsService;
        #endregion

        #region Constructors
        public EmailCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                   IEmailsService emailsService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _emailsService = emailsService;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = await _emailsService.SendEmail(request.Email, request.Message, request.Message);

            if (response != "Success") return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.SendEmailFailed]);

            return Success(response);

        }
        #endregion

    }
}
