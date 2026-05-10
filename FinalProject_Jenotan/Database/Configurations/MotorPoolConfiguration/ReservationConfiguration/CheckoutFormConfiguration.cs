using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.ReservationConfiguration
{
    public class CheckoutFormConfiguration : IEntityTypeConfiguration<CheckoutForm>
    {
        public void Configure(EntityTypeBuilder<CheckoutForm> builder)
        {
            builder.HasKey(c => c.CheckoutId);

            builder.Property(c => c.CheckOutDate)
                   .IsRequired(false);

            // Faculty
            builder.HasOne(c => c.FacultyLink)
                   .WithMany(f => f.CheckoutFormList)
                   .HasForeignKey(c => c.FacultyId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Employee
            builder.HasOne(c => c.EmployeeLink)
                   .WithMany(e => e.CheckoutFormList)
                   .HasForeignKey(c => c.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ReservationForm
            builder.HasOne(c => c.ReservationFormLink)
                   .WithMany(r => r.CheckoutFormList)
                   .HasForeignKey(c => c.RFormId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
