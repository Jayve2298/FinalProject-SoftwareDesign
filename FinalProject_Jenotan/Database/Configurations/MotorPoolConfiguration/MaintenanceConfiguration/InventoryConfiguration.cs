using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.MaintenanceConfiguration
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.HasKey(i => i.InventoryId);

            // Mechanic (Many Inventory → One Mechanic)
            builder.HasOne(i => i.MechanicLink)
                   .WithMany(m => m.InventoryList)
                   .HasForeignKey(i => i.MechanicId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Parts
            builder.HasMany(i => i.PartsList)
                   .WithOne(p => p.InventoryLink)
                   .HasForeignKey(p => p.InventoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
