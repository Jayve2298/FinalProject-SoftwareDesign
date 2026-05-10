using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.SchoolSystemConfiguration
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.DepartmentId);

            builder.Property(d => d.DepartmentName)
                   .IsRequired();
            builder.Property(d => d.IsActive)
                .HasDefaultValue(true);
                
                

            // School
            builder.HasOne(d => d.SchoolLink)
                   .WithMany(s => s.DepartmentList)
                   .HasForeignKey(d => d.SchoolId);

            // Faculty (Members)
            builder.HasMany(d => d.FacultyList)
                   .WithOne(f => f.DepartmentLink)
                   .HasForeignKey(f => f.DepartmentId);

            // Chair (1:1)
            builder.HasOne(d => d.FacultyLink)
                   .WithMany()
                   .HasForeignKey(d => d.FacultyId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
