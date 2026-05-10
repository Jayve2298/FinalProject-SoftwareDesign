using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.SchoolSystemConfiguration
{
    public class SchoolConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.HasKey(s => s.SchooldId);

            builder.Property(s => s.SchoolName)
                   .HasConversion<string>()
                  .IsRequired();

            // Dean (1:1)
            builder.HasOne(s => s.FacultyLink)
                   .WithMany()
                   .HasForeignKey(s => s.FacultyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.DepartmentList)
                   .WithOne(d => d.SchoolLink)
                   .HasForeignKey(d => d.SchoolId);
        }
    }
}
