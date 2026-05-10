using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance
{
    
    public class Mechanic
    {
        public string MechanicId { get;set;  }
        public string Password { get; set; }
        public string FName { get; set;  }
        public string LName { get; set;  }
        public string Email { get; set;  }
        public bool IsAuthorizedToSign { get; set; }
        public bool IsActive { get; set; }

        //relationship
        public List<VehicleReleaseForm>? VehicleReleaseFormList { get; set; }
        public List<Inventory> InventoryList { get; set; }
        public List<MaintenanceDetails> MaintenanceDetailList { get; set; }
        public List<PartsUsedForm> PartUsedFormList { get; set; }
    }
}
