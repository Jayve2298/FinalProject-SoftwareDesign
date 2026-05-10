using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.MaintenanceConfiguration
{
    public class VehicleReleaseFormConfiguration : IEntityTypeConfiguration<VehicleReleaseForm>
    {
        public void Configure(EntityTypeBuilder<VehicleReleaseForm> builder)
        {
            //Primary Key
            builder.HasKey(vr => vr.VRFormId);

            //Properties
            builder.Property(vr => vr.DateSigned)
                   .IsRequired();
            
            //Mechanic (Many Forms → One Mechanic)
            builder.HasOne(vr => vr.MechanicLink)
                   .WithMany(m => m.VehicleReleaseFormList)
                   .HasForeignKey(vr => vr.MechanicId)
                   .OnDelete(DeleteBehavior.Restrict);

            //Maintenance Log (One Log → Many Release Forms)
            builder.HasOne(vr => vr.MaintenanceLogLink)
                   .WithMany(ml => ml.VehicleReleaseFormList)
                   .HasForeignKey(vr => vr.MLogId)
                   .OnDelete(DeleteBehavior.Cascade);

            //Prevent duplicate releases per log (1 log = 1 release)
            builder.HasIndex(vr => vr.MLogId)
                   .IsUnique(); //makes it 1:1 instead of 1:M
        }
    }
}
