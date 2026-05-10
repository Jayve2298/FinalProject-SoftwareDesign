using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation
{
    public class CompletionForm
    {
        public string CFormId { get; set; }
        public float StartOdo { get; set; }
        public float EndOdo { get; set; } 
        public string Complaints { get; set; }
        public float FuelCost { get; set; }
        public string CreditCardNum { get; set; }
        public float TripCost { get; set; }
        public DateTime DateCompleted { get; set; }

        //relationship
        public string FacultyId { get; set; }
        public string VehicleId { get; set; }
        public string? EmployeeId { get; set; }
        public string? VReportId {get; set;}
        public string? CheckoutId { get; set;  }
        
        public Faculty FacultyLink { get; set; }
        public Vehicle VehicleLink { get; set; }
        public Employee? EmployeeLink { get; set; }
        public VehicleReport? VehicleReportLink {get; set;}
        public CheckoutForm? CheckoutFormLink { get; set; }
    }
}
