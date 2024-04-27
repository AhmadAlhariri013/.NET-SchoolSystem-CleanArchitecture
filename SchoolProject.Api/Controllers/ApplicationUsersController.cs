using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.ApplicationUsers.Commands.Models;
using SchoolProject.Core.Features.ApplicationUsers.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : AppControllerBase
    {


        [HttpPost(Router.ApplicationUserRouting.Create)]
        public async Task<IActionResult> CreateUser([FromBody] AddUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet(Router.ApplicationUserRouting.List)]
        public async Task<IActionResult> GetPaginatedUsersList([FromQuery] GetPaginatedUserListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [HttpGet(Router.ApplicationUserRouting.GetById)]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var response = await Mediator.Send(new GetUserByIDQuery(id));
            return NewResult(response);
        }
    }
}
