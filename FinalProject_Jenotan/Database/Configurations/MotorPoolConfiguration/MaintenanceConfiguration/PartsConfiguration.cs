using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.MaintenanceConfiguration
{
    public class PartsConfiguration : IEntityTypeConfiguration<Parts>
    {
        public void Configure(EntityTypeBuilder<Parts> builder)
        {
            builder.HasKey(p => p.PartId);

            builder.Property(p => p.PartName)
                   .IsRequired()
                   .HasMaxLength(50);

            // Inventory
            builder.HasOne(p => p.InventoryLink)
                   .WithMany(i => i.PartsList)
                   .HasForeignKey(p => p.InventoryId);

            // Parts Used
            builder.HasMany(p => p.PartsUsedFormList)
                   .WithOne(pu => pu.PartsLink)
                   .HasForeignKey(pu => pu.PartsId);
        }
    }
}
