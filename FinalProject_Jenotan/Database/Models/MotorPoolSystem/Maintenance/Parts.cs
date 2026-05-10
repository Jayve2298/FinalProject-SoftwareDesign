using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance
{
    public class Parts
    {
        public string PartId { get; set; }
        public string PartName { get; set; }
        public int Quantity { get; set; }

        //relationship
        public string InventoryId { get; set; }
        public Inventory InventoryLink { get; set; }

        //relationship 1 - many
        public List<PartsUsedForm> PartsUsedFormList { get; set; }
    }
}
