using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance
{
    public class PartsUsedForm
    {
        public string PUFormId { get; set; }
        public int QtyUsed { get; set; }
        public bool IsReported { get; set; }
        //relationship
        public string PartsId { get; set; }
        public Parts PartsLink { get; set; }

        public string MLogId { get; set; }
        public MaintenanceLog MaintenanceLogLink { get; set; }

        public string? PURId {get; set;}
        public PartsUsageReport? PartsUsageReportLink {get; set;}

        public string MechanicId { get; set; }
        public Mechanic MechanicLink {  get; set; }
    }
}
