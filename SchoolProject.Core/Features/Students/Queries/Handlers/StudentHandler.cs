using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Basies;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Responses;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Service.Interfaces;

namespace SchoolProject.Core.Features.Students.Queries.Handlers
{
    public class StudentHandler : ResponseHandler, IRequestHandler<GetAllStudentsQuery, Response<List<GetAllStudentsResponse>>>,
                                                   IRequestHandler<GetStudentByIdQuery, Response<GetSingleStudentResponse>>
                                                   , IRequestHandler<GetStudentPaginatedListQuery, PaginatedResult<GetStudentPaginatedListResponse>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructors
        public StudentHandler(IStudentService studentService,
                                   IMapper mapper,
                                   IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        public async Task<Response<List<GetAllStudentsResponse>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            var listStudentToMap = await _studentService.GetStudentsListAsync();
            var studentsMapper = _mapper.Map<List<GetAllStudentsResponse>>(listStudentToMap);
            var response = Success(studentsMapper);
            response.Meta = new { Count = studentsMapper.Count() };
            return response;
        }


        async Task<Response<GetSingleStudentResponse>> IRequestHandler<GetStudentByIdQuery, Response<GetSingleStudentResponse>>.Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var studentToMap = await _studentService.GetStudentByIDWithIncludeAsync(request.Id);
            if (studentToMap is null)
                return NotFound<GetSingleStudentResponse>($"Student With Id: {request.Id} Not Found! ");

            var student = _mapper.Map<GetSingleStudentResponse>(studentToMap);

            return Success(student);

        }

        public async Task<PaginatedResult<GetStudentPaginatedListResponse>> Handle(GetStudentPaginatedListQuery request, CancellationToken cancellationToken)
        {
            //Expression<Func<Student, GetStudentPaginatedListResponse>> expression = e => new GetStudentPaginatedListResponse(e.StudID, e.Localize(e.NameAr, e.NameEn), e.Address, e.Department.Localize(e.Department.DNameAr, e.Department.DNameEn));
            var FilterQuery = _studentService.FilterStudentPaginatedQuerable(request.OrderBy, request.Search);
            var PaginatedList = await _mapper.ProjectTo<GetStudentPaginatedListResponse>(FilterQuery).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            PaginatedList.Meta = new { Count = PaginatedList.Data.Count() };
            return PaginatedList;
        }
    }
}
