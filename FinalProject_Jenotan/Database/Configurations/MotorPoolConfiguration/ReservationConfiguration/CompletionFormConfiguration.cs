using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.ReservationConfiguration
{
    public class CompletionFormConfiguration : IEntityTypeConfiguration<CompletionForm>
    {
        public void Configure(EntityTypeBuilder<CompletionForm> builder)
        {
            builder.HasKey(c => c.CFormId);

            builder.Property(c => c.StartOdo).IsRequired();
            builder.Property(c => c.EndOdo).IsRequired();
            builder.Property(c => c.TripCost).IsRequired();

            // Faculty
            builder.HasOne(c => c.FacultyLink)
                   .WithMany()
                   .HasForeignKey(c => c.FacultyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Vehicle
            builder.HasOne(c => c.VehicleLink)
                   .WithMany(v => v.CompletionFormList)
                   .HasForeignKey(c => c.VehicleId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Employee
            builder.HasOne(c => c.EmployeeLink)
                   .WithMany(e => e.CompletionFormList)
                   .HasForeignKey(c => c.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // VehicleReport
            builder.HasOne(c => c.VehicleReportLink)
                   .WithMany(vr => vr.CompletionFormList)
                   .HasForeignKey(c => c.VReportId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
