using SchoolProject.Data.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class Instructor : GeneralLocalizableEntity
    {
        public Instructor()
        {
            Instructors = new HashSet<Instructor>();
            InstructorSubjects = new HashSet<InstructorSubject>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InsId { get; set; }
        public string? ENameAr { get; set; }
        public string? ENameEn { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public int? SupervisorId { get; set; }
        public decimal? Salary { get; set; }
        public string? Image { get; set; }
        public int DID { get; set; }

        [ForeignKey("DID")]
        [InverseProperty(nameof(Department.Instructors))]
        public virtual Department? Department { get; set; }

        [InverseProperty("Instructor")]
        public virtual Department? ManagedDepartment { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        [InverseProperty("Instructors")]
        public virtual Instructor? Supervisor { get; set; }

        [InverseProperty("Supervisor")]
        public virtual ICollection<Instructor> Instructors { get; set; }

        [InverseProperty("Instructor")]
        public virtual ICollection<InstructorSubject> InstructorSubjects { get; set; }
    }
}
