using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.SchoolSystemConfiguration
{
    public class MajorConfiguration : IEntityTypeConfiguration<Major>
    {
        public void Configure(EntityTypeBuilder<Major> builder)
        {
            builder.HasKey(m => m.MajorId);

            builder.Property(m => m.MajorName)
                   .IsRequired();
            builder.Property(m => m.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            builder.Property(m => m.StudentCount)
                .IsRequired(true)
                .HasDefaultValue(0);

            builder.HasOne(m => m.DepartmentLink)
                   .WithMany(d => d.MajorList)
                   .HasForeignKey(m => m.DepartmentId);
        }
    }
}
