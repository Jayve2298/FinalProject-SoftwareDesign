using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string Password { get; set; }
        public string FName { get; set;  }
        public string LName { get; set;  }
        public string Email { get;  set;  }
        public string ContactNumber { get; set;  }

        //relationship 1 - many
        public List<CompletionForm> CompletionFormList { get;set;  }
        public List<CheckoutForm> CheckoutFormList { get;set;  }
        public List<VehicleReport> VehicleReportList { get;set;  }
        public List<PartsUsageReport> PartsUsageReportList { get;set;  }


    }
}
