using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.ReservationConfiguration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeId);

            builder.Property(e => e.FName).IsRequired();
            builder.Property(e => e.LName).IsRequired();
            builder.Property(e => e.Email).IsRequired();

            // CompletionForms
            builder.HasMany(e => e.CompletionFormList)
                   .WithOne(c => c.EmployeeLink)
                   .HasForeignKey(c => c.EmployeeId);

            // CheckoutForms
            builder.HasMany(e => e.CheckoutFormList)
                   .WithOne(c => c.EmployeeLink)
                   .HasForeignKey(c => c.EmployeeId);

            // VehicleReports
            builder.HasMany(e => e.VehicleReportList)
                   .WithOne()
                   .HasForeignKey("EmployeeId"); // shadow FK

            // PartsUsageReports
            builder.HasMany(e => e.PartsUsageReportList)
                   .WithOne()
                   .HasForeignKey("EmployeeId"); // shadow FK
        }
    }
}
