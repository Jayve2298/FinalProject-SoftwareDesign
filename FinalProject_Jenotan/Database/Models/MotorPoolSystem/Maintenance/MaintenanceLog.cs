using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance
{
    public class MaintenanceLog
    {
        public string MLogId { get; set; }
        public string Description { get; set; }
        public DateTime DateLogged { get; set; }
        public DateTime? CompleteDate { get; set;  }
        public bool IsCompleted { get; set;  }
        
        //relationship
        public string VehicleId { get; set; }
        public Vehicle VehicleLink { get; set;  }
        
        //relationship 1 - many
        public List<MaintenanceDetails> MaintenanceDetailsList { get; set; }
        public List<PartsUsedForm> PartsUsedFormList { get; set; }
        public List<VehicleReleaseForm> VehicleReleaseFormList { get; set; }
    }
}
