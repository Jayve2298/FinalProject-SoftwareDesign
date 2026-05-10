using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.MaintenanceConfiguration
{
    public class PartsUsageReportConfiguration : IEntityTypeConfiguration<PartsUsageReport>
    {
        public void Configure(EntityTypeBuilder<PartsUsageReport> builder)
        {
            builder.HasKey(pur => pur.PURId);

            builder.Property(pur => pur.TotalPartsUsed)
                   .IsRequired();

            // Employee
            builder.HasOne(pur => pur.EmployeeLink)
                   .WithMany()
                   .HasForeignKey(pur => pur.EmployeeId);

            // Parts Used
            builder.HasMany(pur => pur.PartsUsedFormList)
                   .WithOne(pu => pu.PartsUsageReportLink)
                   .HasForeignKey(pu => pu.PURId);
        }
    }
}
