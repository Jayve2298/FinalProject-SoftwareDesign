using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation
{
    public enum VehicleType
    {
        Sedan, StationWagon, PanelTruck, Minivan, MiniBus
    }
    public class Vehicle
    {
        public string VehicleId { get; set; }
        public string Brand { get; set; }
        public VehicleType Type { get; set; }
        public string Mileage { get; set; }
        public float CostPerMilage { get; set; }
        public bool IsAvailable { get; set;  }
        public float OdoReading { get; set;  }

        //Relationship
        //public List<CheckoutForm> CheckoutFormList { get; set; }
        public List<ReservationForm>? ReservationFormList { get; set; }
        public List<CompletionForm>? CompletionFormList { get; set; }
        public List<MaintenanceLog>? MaintenanceLogList { get; set; }
        public List<VehicleReport>? VehicleReportList { get; set; }
    }
}
