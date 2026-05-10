using FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.MaintenanceConfiguration;
using FinalProject_Jenotan.Database.Configurations.MotorPoolConfiguration.ReservationConfiguration;
using FinalProject_Jenotan.Database.Configurations.SchoolSystemConfiguration;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using FinalProject_Jenotan.DatabaseSeed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database
{
    public class TinyCollegeDbContextSQLServer : DbContext
    {
        //SchoolSystem Set
        public DbSet<Class> Classes { get; set;  }
        public DbSet<Course> Courses { get; set;  }
        public DbSet<Department> Departments { get; set;  }
        public DbSet<Enrollment> Enrollments { get; set;  }
        public DbSet<Faculty> Faculties { get; set;  }
        public DbSet<Major> Majors { get; set;  }
        public DbSet<School> Schools { get; set;  }
        public DbSet<Student> Students { get; set;  }
        public DbSet<Room> Rooms { get; set;  }
        public DbSet<Building> Buildings { get; set;  }
        //MotorpoolSystem Set
        //Reservation
        public DbSet<CheckoutForm> CheckoutForms { get; set; }
        public DbSet<CompletionForm> CompletionForms { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ReservationForm>  ReservationForms { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleReport> VehicleReports { get; set; }
        //Maintenance
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<MaintenanceDetails> MaintenanceDetails { get; set; }
        public DbSet<MaintenanceLog> MaintenanceLogs { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Parts> Parts { get; set; }
        public DbSet<PartsUsageReport> PartsUsageReports { get; set; }
        public DbSet<PartsUsedForm> PartsUsedForms { get; set; }
        public DbSet<VehicleReleaseForm> VehicleReleaseForms { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($@"Data Source=JAYVE\JVCJENOTAN;Database=TinyCollegeDb;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Application Name=""SQL Server Management Studio"";Command Timeout=0");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //SchoolSystem
            modelBuilder.ApplyConfiguration(new ClassConfiguration()); 
            modelBuilder.ApplyConfiguration(new CourseConfiguration()); 
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration()); 
            modelBuilder.ApplyConfiguration(new EnrollmentConfiguration()); 
            modelBuilder.ApplyConfiguration(new FacultyConfiguration()); 
            modelBuilder.ApplyConfiguration(new MajorConfiguration()); 
            modelBuilder.ApplyConfiguration(new SchoolConfiguration()); 
            modelBuilder.ApplyConfiguration(new StudentConfiguration()); 
            modelBuilder.ApplyConfiguration(new RoomConfiguration()); 
            modelBuilder.ApplyConfiguration(new BuildingConfiguration());
            //Reservation
            modelBuilder.ApplyConfiguration(new CheckoutFormConfiguration());
            modelBuilder.ApplyConfiguration(new CompletionFormConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationFormConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleReportConfiguration());
            //Maintenance
            modelBuilder.ApplyConfiguration(new InventoryConfiguration());
            modelBuilder.ApplyConfiguration(new MaintenanceDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new MaintenanceLogConfiguration());
            modelBuilder.ApplyConfiguration(new MechanicConfiguration());
            modelBuilder.ApplyConfiguration(new PartsConfiguration());
            modelBuilder.ApplyConfiguration(new PartsUsageReportConfiguration());
            modelBuilder.ApplyConfiguration(new PartsUsedFormConfiguration());
            modelBuilder.ApplyConfiguration(new VehicleReleaseFormConfiguration());
            
        }
    }
}
