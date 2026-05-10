using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.SchoolSystemConfiguration
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.CourseId);

            builder.Property(c => c.CourseName)
                   .IsRequired();
            builder.Property(c => c.IsActive)
                .IsRequired();
            builder.Property(c => c.ClassCount)
                .HasDefaultValue(0);
                
            

            builder.HasOne(c => c.DepartmentLink)
                   .WithMany(d => d.CourseList)
                   .HasForeignKey(c => c.DepartmentId);
        }
    }
}
