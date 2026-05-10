using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.SchoolSystemConfiguration
{
    public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            // 🔑 Primary Key
            builder.HasKey(f => f.FacultyId);

            // 🧾 Properties
            builder.Property(f => f.Password)
                   .IsRequired();

            builder.Property(f => f.FName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(f => f.LName)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(f => f.Email)
                   .IsRequired();

            builder.Property(f => f.ContactNumber)
                   .IsRequired(false);

            builder.Property(f => f.ClassCount)
                   .HasDefaultValue(0);

            builder.Property(f => f.Role)
                  .HasConversion<string>()
                  .IsRequired();

            builder.Property(f => f.IsActive)
                .HasDefaultValue(true);

            // Department (Many Faculty → One Department)
            builder.HasOne(f => f.DepartmentLink)
                   .WithMany(d => d.FacultyList)
                   .HasForeignKey(f => f.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // eaching (One Faculty → Many Classes)
            builder.HasMany(f => f.ClassTeachingList)
                   .WithOne(c => c.FacultyLink)
                   .HasForeignKey(c => c.FacultyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Advising (One Faculty → Many Students)
            builder.HasMany(f => f.StudentAdviseList)
                   .WithOne(s => s.FacultyLink)
                   .HasForeignKey(s => s.FacultyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Optional: Reservation / Checkout (if needed)
            builder.HasMany(f => f.ReservationFormList)
                   .WithOne(c => c.FacultyLink)
                   .HasForeignKey("FacultyId")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(f => f.CheckoutFormList)
                   .WithOne(c => c.FacultyLink)
                   .HasForeignKey("FacultyId")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(f => f.CompletionFormList)
                .WithOne(c => c.FacultyLink)
                .HasForeignKey("FacultyId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
