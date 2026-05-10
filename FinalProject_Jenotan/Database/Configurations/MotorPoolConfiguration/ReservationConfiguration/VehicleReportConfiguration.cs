using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.ReservationConfiguration
{
    public class VehicleReportConfiguration : IEntityTypeConfiguration<VehicleReport>
    {
        public void Configure(EntityTypeBuilder<VehicleReport> builder)
        {
            builder.HasKey(vr => vr.VReportId);

            builder.Property(vr => vr.TimesUsed).IsRequired();
            builder.Property(vr => vr.TotalMileageGained).IsRequired();

            // CompletionForms
            builder.HasMany(vr => vr.CompletionFormList)
                   .WithOne(c => c.VehicleReportLink)
                   .HasForeignKey(c => c.VReportId)
                   .OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
