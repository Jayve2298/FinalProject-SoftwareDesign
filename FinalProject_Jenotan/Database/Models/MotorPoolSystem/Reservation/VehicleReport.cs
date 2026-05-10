using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation
{
    public class VehicleReport
    {
        public string VReportId { get; set;  }
        public int TimesUsed { get; set; }
        public float TotalMileageGained { get; set;  }

        //relationship
        public string? VehicleId { get; set; }
        public Vehicle? VehicleLink { get; set; }
        public string? EmployeeId { get; set; }
        public Employee? EmployeeLink { get; set; }

        //relationship1-m
        public List<CompletionForm> CompletionFormList { get; set; }
    }
}
