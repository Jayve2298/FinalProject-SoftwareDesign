using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance
{
    public class PartsUsageReport
    {
        public string PURId { get; set;  }
        public int TotalPartsUsed { get; set;  }
         

        //relationship
        public string EmployeeId { get; set; }
        public Employee EmployeeLink { get; set;  }
        //relationship 1 - many
        public List<PartsUsedForm> PartsUsedFormList { get; set; }
    }
}
