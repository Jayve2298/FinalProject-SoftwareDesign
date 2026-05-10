using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance
{
    public class MaintenanceDetails
    {
        public string MDetailsId { get; set; }
        public string MaintenancePerformed { get; set; }
        public DateTime DateLogged { get; set; }
        public DateTime? CompleteDate { get; set; }

        //relationship
        public string? MechanicId { get; set;  }
        public Mechanic? MechanicLink { get; set; }
        public string MLogId { get; set; }
        public MaintenanceLog MaintenanceLogLink { get; set; }

        //relationship 1 - many
        public List<PartsUsedForm> PartsUsedFormList { get; set; }
    }
}
