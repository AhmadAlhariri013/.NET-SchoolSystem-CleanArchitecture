using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Data.Entities
{
    public class InstructorSubject
    {
        [Key]
        public int InstId { get; set; }
        [Key]
        public int SubId { get; set; }

        [ForeignKey("InstId")]
        [InverseProperty("InstructorSubjects")]
        public virtual Instructor? Instructor { get; set; }

        [ForeignKey("SubId")]
        [InverseProperty("InstructorSubjects")]
        public virtual Subject? Subject { get; set; }

    }
}
