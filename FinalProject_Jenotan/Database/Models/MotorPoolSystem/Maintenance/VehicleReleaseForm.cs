using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance
{
    public class VehicleReleaseForm
    {
        public string VRFormId { get; set; }
        public DateTime DateSigned { get; set; }

        //relationship
        public string MechanicId { get; set;  }
        public string MLogId { get; set; }
        public Mechanic MechanicLink { get; set; }
        public MaintenanceLog MaintenanceLogLink { get; set; }

    }
}
