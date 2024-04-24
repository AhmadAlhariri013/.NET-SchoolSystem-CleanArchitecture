using MediatR;
using SchoolProject.Core.Basies;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Core.Features.Students.Commands.Models
{
    public class AddStudentCommand : IRequest<Response<string>>
    {
        [Required]
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public string? Address { get; set; }

        [Required]
        public string Phone { get; set; }

        public int? DepartmentId { get; set; }
    }
}
