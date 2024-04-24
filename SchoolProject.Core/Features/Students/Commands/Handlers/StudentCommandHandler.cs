using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Interfaces;

namespace SchoolProject.Core.Features.Students.Commands.Handlers
{
    public class StudentCommandHandler : ResponseHandler, IRequestHandler<AddStudentCommand, Response<string>>,
                                                          IRequestHandler<EditStudentCommand, Response<string>>,
                                                          IRequestHandler<DeleteStudentCommand, Response<string>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public StudentCommandHandler(IStudentService studentService,
                                     IMapper mapper,
                                     IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion
        #region Handle Functions
        public async Task<Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            // Mapping Between request and student
            var studentMapper = _mapper.Map<Student>(request);
            // Use The Student Services to Add the student By "AddAsync" Service
            var studentResulte = await _studentService.AddAsync(studentMapper);
            // Check if the response successed 
            if (studentResulte == "Success") return Created("");
            else return BadRequest<string>();

        }

        public async Task<Response<string>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            //Check if the Id is Exist Or not
            var student = await _studentService.GetByIDAsync(request.Id);
            //return NotFound
            if (student == null) return NotFound<string>();
            //mapping Between request and student
            var studentMapper = _mapper.Map(request, student);
            //Call service that make Edit
            var result = await _studentService.EditAsync(studentMapper);
            //return response
            if (result == "Success") return Success((string)_stringLocalizer[SharedResourcesKeys.Updated]);
            else return BadRequest<string>();


        }

        public async Task<Response<string>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            //Check if the Id is Exist Or not
            var student = await _studentService.GetByIDAsync(request.Id);
            //return NotFound
            if (student == null) return NotFound<string>();
            //Call service that make Delete
            var result = await _studentService.DeleteAsync(student);
            if (result == "Success") return Deleted<string>();
            else return BadRequest<string>();
        }
        #endregion
    }
}
