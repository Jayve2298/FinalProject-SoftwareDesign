using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation
{
    public class ReservationForm
    {
        public string RFormId {  get; set; }
        public DateTime DepartureDateTime { get; set; }
        public string Destination { get; set; }
        public bool ISApproved { get; set; }

        //relationship
        public string FacultyId { get; set;  }
        public Faculty FacultyLink { get; set; }
        public string VehicleId { get; set; }
        public Vehicle VehicleLink { get; set; }

        public string? EmployeeId { get; set;  }
        public Employee? EmployeeLink { get; set; }
        
        //relationship 1 - many
        public List<CheckoutForm> CheckoutFormList { get; set; }
    }
}
