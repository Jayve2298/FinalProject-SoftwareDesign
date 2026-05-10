using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.ReservationConfiguration
{
    public class ReservationFormConfiguration : IEntityTypeConfiguration<ReservationForm>
    {
        public void Configure(EntityTypeBuilder<ReservationForm> builder)
        {
            builder.HasKey(r => r.RFormId);

            builder.Property(r => r.DepartureDateTime).IsRequired();
            builder.Property(r => r.Destination).IsRequired();

            // Faculty
            builder.HasOne(r => r.FacultyLink)
                   .WithMany()
                   .HasForeignKey(r => r.FacultyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Vehicle
            builder.HasOne(r => r.VehicleLink)
                   .WithMany(v => v.ReservationFormList)
                   .HasForeignKey(r => r.VehicleId)
                   .OnDelete(DeleteBehavior.Restrict);

            // CheckoutForms
            builder.HasMany(r => r.CheckoutFormList)
                   .WithOne(c => c.ReservationFormLink)
                   .HasForeignKey(c => c.RFormId)
                   .OnDelete(DeleteBehavior.Cascade);

            //ResrvationForms
            builder.HasOne(r => r.EmployeeLink)
                .WithMany()
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
