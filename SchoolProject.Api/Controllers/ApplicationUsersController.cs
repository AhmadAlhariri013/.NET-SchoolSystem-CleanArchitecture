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


        // Create User Endpoint
        [HttpPost(Router.ApplicationUserRouting.Create)]
        public async Task<IActionResult> CreateUser([FromBody] AddUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        // Get Users List Endpoint
        [HttpGet(Router.ApplicationUserRouting.List)]
        public async Task<IActionResult> GetPaginatedUsersList([FromQuery] GetPaginatedUserListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        // Get User By Id Endpoint
        [HttpGet(Router.ApplicationUserRouting.GetById)]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            var response = await Mediator.Send(new GetUserByIDQuery(id));
            return NewResult(response);
        }


        // Edit User Endpoint
        [HttpPut(Router.ApplicationUserRouting.Edit)]
        public async Task<IActionResult> GetUserById([FromBody] EditUserCommand user)
        {
            var response = await Mediator.Send(user);
            return NewResult(response);
        }

        // Delete User Endpoint
        [HttpDelete(Router.ApplicationUserRouting.Delete)]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteUserCommand(id));
            return NewResult(response);
        }

        // Change User Password Endpoint
        [HttpPut(Router.ApplicationUserRouting.ChangePassword)]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

    }
}
