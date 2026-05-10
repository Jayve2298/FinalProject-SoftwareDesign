using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation
{
    public class CheckoutForm
    {
        public string CheckoutId { get; set;  }
        public DateTime? CheckOutDate { get;set;  }
        public bool IsVerified { get; set; }
        public bool IsOnGoing {  get; set; }
        public bool IsCheckedOut { get; set;  }
        //relationship
        
        public string FacultyId { get; set;  }
        public Faculty FacultyLink { get; set; }
        public string? EmployeeId { get; set; }
        public Employee? EmployeeLink { get; set; } 
        public string RFormId { get; set;  }
        public ReservationForm ReservationFormLink { get; set; }
        //1 - many
        public List<CompletionForm>? CompletionForms { get; set; }
    }
}
