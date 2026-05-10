using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.SchoolSystemConfiguration
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => e.EnrollmentId);

            builder.Property(e => e.Semester)
                   .IsRequired();
            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);


            //class
            builder.HasOne(e => e.ClassLink)
                   .WithMany(c => c.EnrollmentList)
                   .HasForeignKey(e => e.ClassId);
            

            //Student
            builder.HasOne(e => e.StudentLink)
                   .WithMany()
                   .HasForeignKey(e => e.StudentId);
        }
    }
}
