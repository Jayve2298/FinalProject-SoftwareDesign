using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.MaintenanceConfiguration
{
    public class MaintenanceDetailsConfiguration : IEntityTypeConfiguration<MaintenanceDetails>
    {
        public void Configure(EntityTypeBuilder<MaintenanceDetails> builder)
        {
            builder.HasKey(md => md.MDetailsId);

            builder.Property(md => md.MaintenancePerformed)
                   .IsRequired();
            builder.Property(md => md.DateLogged)
                .IsRequired();

            // Mechanic
            builder.HasOne(md => md.MechanicLink)
                   .WithMany(m => m.MaintenanceDetailList)
                   .HasForeignKey(md => md.MechanicId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Maintenance Log
            builder.HasOne(md => md.MaintenanceLogLink)
                   .WithMany(ml => ml.MaintenanceDetailsList)
                   .HasForeignKey(md => md.MLogId);

            // Parts Used
            builder.HasMany(md => md.PartsUsedFormList)
                   .WithOne()
                   .HasForeignKey("MDetailsId") // shadow FK
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
