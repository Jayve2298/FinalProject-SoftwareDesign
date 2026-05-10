using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.MaintenanceConfiguration
{
    public class MaintenanceLogConfiguration : IEntityTypeConfiguration<MaintenanceLog>
    {
        public void Configure(EntityTypeBuilder<MaintenanceLog> builder)
        {
            builder.HasKey(ml => ml.MLogId);

            builder.Property(ml => ml.Description)
                   .IsRequired();
            builder.Property(ml => ml.DateLogged)
                .IsRequired();

            // Vehicle
            builder.HasOne(ml => ml.VehicleLink)
                   .WithMany()
                   .HasForeignKey(ml => ml.VehicleId);

            // Details
            builder.HasMany(ml => ml.MaintenanceDetailsList)
                   .WithOne(md => md.MaintenanceLogLink)
                   .HasForeignKey(md => md.MLogId);

            // Parts Used
            builder.HasMany(ml => ml.PartsUsedFormList)
                   .WithOne(p => p.MaintenanceLogLink)
                   .HasForeignKey(p => p.MLogId);

            // Release Form
            builder.HasMany(ml => ml.VehicleReleaseFormList)
                   .WithOne()
                   .HasForeignKey("MLogId"); // shadow FK
        }
    }
}
