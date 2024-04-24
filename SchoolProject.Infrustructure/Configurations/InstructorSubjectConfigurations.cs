using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolProject.Data.Entities;

namespace SchoolProject.Infrustructure.Configurations
{
    public class InstructorSubjectConfigurations : IEntityTypeConfiguration<InstructorSubject>
    {
        public void Configure(EntityTypeBuilder<InstructorSubject> builder)
        {
            builder
                .HasKey(x => new { x.SubId, x.InstId });


            builder.HasOne(ds => ds.Instructor)
                     .WithMany(d => d.InstructorSubjects)
                     .HasForeignKey(ds => ds.InstId);

            builder.HasOne(ds => ds.Subject)
                 .WithMany(d => d.InstructorSubjects)
                 .HasForeignKey(ds => ds.SubId);
        }
    }
}
