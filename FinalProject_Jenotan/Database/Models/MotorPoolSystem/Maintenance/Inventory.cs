using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance
{
    public class Inventory
    {
        public string InventoryId { get; set; }
        
        //relationship
        public string MechanicId { get; set; }
        public Mechanic MechanicLink { get; set; }
        
        //relationship 1 - many
        public List<Parts> PartsList { get; set; }
    }
}
