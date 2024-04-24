using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.Departments.Queries.Models;
using SchoolProject.Core.Features.Departments.Queries.Responses;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Interfaces;
using System.Linq.Expressions;

namespace SchoolProject.Core.Features.Departments.Queries.Handlers
{
    public class DepartmentQueryHandler : ResponseHandler, IRequestHandler<GetDepartmentByIDQuery, Response<GetDepartmentByIDReponse>>
    {

        #region Fields
        private readonly IDepartmentService _departmentService;
        private readonly IStudentService _studentService;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        private readonly IMapper _mapper;
        #endregion

        #region Constructors
        public DepartmentQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                      IDepartmentService departmentService,
                                      IMapper mapper,
                                      IStudentService studentService) : base(stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _mapper = mapper;
            _studentService = studentService;
            _departmentService = departmentService;
        }

        #endregion

        #region Handle Functions
        #endregion
        public async Task<Response<GetDepartmentByIDReponse>> Handle(GetDepartmentByIDQuery request, CancellationToken cancellationToken)
        {
            // 
            var departmentToMapping = await _departmentService.GetDepartmentByID(request.Id);

            if (departmentToMapping is null) return NotFound<GetDepartmentByIDReponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            //mapping
            var mapper = _mapper.Map<GetDepartmentByIDReponse>(departmentToMapping);

            // Pagination
            Expression<Func<Student, StudentResponse>> expression = e => new StudentResponse(e.StudID, e.Localize(e.NameAr, e.NameEn));
            var studentQuerable = _studentService.GetStudentsByDepartmentIDQuerable(request.Id);
            var PaginatedList = await studentQuerable.Select(expression).ToPaginatedListAsync(request.StudentPageNumber, request.StudentPageSize);
            mapper.StudentList = PaginatedList;


            return Success(mapper);
        }
    }
}
