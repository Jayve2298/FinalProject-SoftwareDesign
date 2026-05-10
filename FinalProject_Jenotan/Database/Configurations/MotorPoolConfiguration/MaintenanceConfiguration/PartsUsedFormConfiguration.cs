using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.MaintenanceConfiguration
{
    public class PartsUsedFormConfiguration : IEntityTypeConfiguration<PartsUsedForm>
    {
        public void Configure(EntityTypeBuilder<PartsUsedForm> builder)
        {
            builder.HasKey(pu => pu.PUFormId);

            builder.Property(pu => pu.QtyUsed)
                   .IsRequired();

            // Parts
            builder.HasOne(pu => pu.PartsLink)
                   .WithMany(p => p.PartsUsedFormList)
                   .HasForeignKey(pu => pu.PartsId);

            // Maintenance Log
            builder.HasOne(pu => pu.MaintenanceLogLink)
           .WithMany(ml => ml.PartsUsedFormList)
           .HasForeignKey(pu => pu.MLogId)
           .OnDelete(DeleteBehavior.NoAction);

            // Report
            builder.HasOne(pu => pu.PartsUsageReportLink)
           .WithMany(pur => pur.PartsUsedFormList)
           .HasForeignKey(pu => pu.PURId)
           .OnDelete(DeleteBehavior.NoAction);

            // Mechanic
            builder.HasOne(pu => pu.MechanicLink)
                   .WithMany(m => m.PartUsedFormList)
                   .HasForeignKey(pu => pu.MechanicId);
        }
    }
}
