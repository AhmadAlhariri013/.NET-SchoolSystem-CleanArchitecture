

using SchoolProject.Data.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public partial class Department : GeneralLocalizableEntity
    {
        public Department()
        {
            Students = new HashSet<Student>();
            DepartmentSubjects = new HashSet<DepartmetSubject>();
            Instructors = new HashSet<Instructor>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DID { get; set; }

        public string? DNameAr { get; set; }
        [StringLength(200)]
        public string? DNameEn { get; set; }

        public int? ManagerId { get; set; }

        [ForeignKey(nameof(ManagerId))]
        [InverseProperty("ManagedDepartment")]
        public virtual Instructor? Instructor { get; set; }


        [InverseProperty("Department")]
        public virtual ICollection<Instructor> Instructors { get; set; }


        [InverseProperty("Department")]
        public virtual ICollection<Student> Students { get; set; }

        [InverseProperty("Department")]
        public virtual ICollection<DepartmetSubject> DepartmentSubjects { get; set; }

    }

}
