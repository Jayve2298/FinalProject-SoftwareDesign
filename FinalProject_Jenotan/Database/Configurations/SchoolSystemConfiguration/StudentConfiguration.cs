using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.SchoolSystemConfiguration
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {

        public void Configure(EntityTypeBuilder<Student> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(s => s.StudentId);

            // 🧾 Properties
            builder.Property(s => s.Password)
                   .IsRequired();

            builder.Property(s => s.FName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.LName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(s => s.Email)
                   .IsRequired();

            builder.Property(s => s.Year)
                   .IsRequired();
            builder.Property(s => s.IsEnrolled)
                .IsRequired();

            // Major (Many Students → One Major, OPTIONAL)
            builder.HasOne(s => s.MajorLink)
                   .WithMany(m => m.StudentList)
                   .HasForeignKey(s => s.MajorId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Advisor (Faculty) (Many Students → One Faculty, REQUIRED)
            builder.HasOne(s => s.FacultyLink)
                   .WithMany(f => f.StudentAdviseList)
                   .HasForeignKey(s => s.FacultyId)
                   .IsRequired() // 🔥 REQUIRED
                   .OnDelete(DeleteBehavior.Restrict);

            // Enrollment (One Student → Many Enrollments)
            builder.HasMany(s => s.EnrollmentList)
                   .WithOne(e => e.StudentLink)
                   .HasForeignKey(e => e.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
