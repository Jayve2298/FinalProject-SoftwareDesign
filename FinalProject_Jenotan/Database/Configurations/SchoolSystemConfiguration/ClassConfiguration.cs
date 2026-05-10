using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.SchoolSystemConfiguration
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(c => c.ClassId);

            builder.Property(c => c.ClassTime)
                   .IsRequired();
            builder.Property(c => c.DayOfWeek)
                    .HasConversion<string>()
                    .IsRequired();
            builder.Property(c => c.DurationInMinutes)
                .IsRequired();
            builder.Property(c => c.IsOnGoing)
                .IsRequired()
                .HasDefaultValue(true);


            // Course (Many Classes → One Course)
            builder.HasOne(c => c.CourseLink)
                   .WithMany(c => c.ClassList)
                   .HasForeignKey(c => c.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Faculty (Many Classes → One Faculty)
            builder.HasOne(c => c.FacultyLink)
                   .WithMany(f => f.ClassTeachingList)
                   .HasForeignKey(c => c.FacultyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Room (Many Classes → One Room)
            builder.HasOne(c => c.RoomLink)
                   .WithMany(r => r.ClassList)
                   .HasForeignKey(c => c.RoomCode)
                   .OnDelete(DeleteBehavior.Restrict);

            // Enrollment
            builder.HasMany(c => c.EnrollmentList)
                   .WithOne(e => e.ClassLink)
                   .HasForeignKey(e => e.ClassId);


            //classcount
            builder.Property(c => c.ClassCount)
             .IsRequired()
                .HasDefaultValue(0);
        }
    }

    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(r => r.RoomCode);

            builder.HasOne(r => r.BuildingLink)
                   .WithMany(b => b.RoomList)
                   .HasForeignKey(r => r.BuildingId);

            builder.HasMany(r => r.ClassList)
                   .WithOne(c => c.RoomLink)
                   .HasForeignKey(c => c.RoomCode);
        }
    }
    public class BuildingConfiguration : IEntityTypeConfiguration<Building>
    {
        public void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.HasKey(b => b.BuildingId);

            builder.Property(b => b.BuildingName)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
