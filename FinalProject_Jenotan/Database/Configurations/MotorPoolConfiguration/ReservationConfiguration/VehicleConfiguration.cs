using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.ReservationConfiguration
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(v => v.VehicleId);

            builder.Property(v => v.Brand).IsRequired();
            builder.Property(v => v.Type).IsRequired();

            builder.Property(v => v.IsAvailable)
                   .HasDefaultValue(true);

            builder.Property(v => v.OdoReading)
                   .HasDefaultValue(0);

            // ReservationForms
            builder.HasMany(v => v.ReservationFormList)
                   .WithOne(r => r.VehicleLink)
                   .HasForeignKey(r => r.VehicleId);

            // CompletionForms
            builder.HasMany(v => v.CompletionFormList)
                   .WithOne(c => c.VehicleLink)
                   .HasForeignKey(c => c.VehicleId);

            // MaintenanceLogs (assuming exists)
            builder.HasMany(v => v.MaintenanceLogList)
                   .WithOne()
                   .HasForeignKey(c => c.VehicleId);
            //VehicleReportList
            builder.HasMany(v => v.VehicleReportList)
                .WithOne(vr => vr.VehicleLink)   // ✅ IMPORTANT FIX
                .HasForeignKey(vr => vr.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
