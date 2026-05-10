using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.MaintenanceConfiguration
{
    public class MechanicConfiguration : IEntityTypeConfiguration<Mechanic>
    {
        public void Configure(EntityTypeBuilder<Mechanic> builder)
        {
            builder.HasKey(m => m.MechanicId);

            builder.Property(m => m.Password)
                .IsRequired()
                .HasMaxLength(50); ;
            builder.Property(m => m.FName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(m => m.LName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(m => m.Email)
                .IsRequired()
                 .HasMaxLength(50);
            builder.Property(m => m.IsAuthorizedToSign)
                .IsRequired();
            builder.Property(m => m.IsActive)
                .IsRequired()
                ;

            // Inventory
            builder.HasMany(m => m.InventoryList)
                   .WithOne(i => i.MechanicLink)
                   .HasForeignKey(i => i.MechanicId);

            // Maintenance Details
            builder.HasMany(m => m.MaintenanceDetailList)
                   .WithOne(md => md.MechanicLink)
                   .HasForeignKey(md => md.MechanicId);

            // Parts Used
            builder.HasMany(m => m.PartUsedFormList)
                   .WithOne(p => p.MechanicLink)
                   .HasForeignKey(p => p.MechanicId);

            // Vehicle Release
            builder.HasMany(m => m.VehicleReleaseFormList)
                   .WithOne(v => v.MechanicLink)
                   .HasForeignKey(v => v.MechanicId);
        }
    }
}
