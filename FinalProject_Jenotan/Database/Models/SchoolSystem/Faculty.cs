using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.EnrollmentSystem
{
    public enum Role
    {
        Dean, DepartmentChair, Researcher, Professor
    }

    public class Faculty
    {
        public string FacultyId { get; set;  }
        public string Password { get; set; }
        public string FName { get; set; }
        public string LName { get; set;  }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public int ClassCount { get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; }

        //relationship
        public string? DepartmentId { get; set; }
        public Department? DepartmentLink { get; set; }

        //relationship 1 - many
        public List<Class>? ClassTeachingList { get; set; }
        public List<Student>? StudentAdviseList { get; set; }
        public List<ReservationForm> ReservationFormList { get; set;  }
        public List<CheckoutForm> CheckoutFormList { get; set; }
        public List<CompletionForm> CompletionFormList { get; set; }
    }
}
