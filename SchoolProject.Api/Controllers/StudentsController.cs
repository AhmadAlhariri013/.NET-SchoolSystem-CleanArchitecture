using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Filters;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{

    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StudentsController : AppControllerBase
    {
        [Authorize(Policy = "GetStudent")]
        [HttpGet(Router.StudentRouting.List)]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<IActionResult> GetAllStudents()
        {
            var response = await Mediator.Send(new GetAllStudentsQuery());
            return NewResult(response);
        }

        [Authorize(Policy = "GetStudent")]
        [HttpGet(Router.StudentRouting.GetById)]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetStudentByIdQuery(id));
            return NewResult(response);
        }

        [Authorize(Policy = "GetStudent")]
        [HttpGet(Router.StudentRouting.Paginated)]
        public async Task<IActionResult> Paginated([FromQuery] GetStudentPaginatedListQuery query)
        {
            var response = await Mediator.Send(query);
            return Ok(response);
        }

        [Authorize(Policy = "CreateStudent")]
        [HttpPost(Router.StudentRouting.Create)]
        public async Task<IActionResult> CreateStudent([FromBody] AddStudentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = "EditStudent")]
        [HttpPut(Router.StudentRouting.Edit)]
        public async Task<IActionResult> UpdateStudent([FromForm] EditStudentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [Authorize(Policy = "DeleteStudent")]
        [HttpDelete(Router.StudentRouting.Delete)]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteStudentCommand(id));
            return NewResult(response);
        }
    }
}
