using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.EnrollmentSystem
{
    public class Class
    {
        public string ClassId { get; set; }
        public TimeOnly ClassTime {  get; set; }
        public int DurationInMinutes { get; set; }
        public DayOfWeek DayOfWeek { get; set; }    
        public bool IsOnGoing { get; set; }

        //relationship
        public string CourseId { get; set; }
        public Course CourseLink { get; set; }
        public string FacultyId { get; set; }
        public Faculty FacultyLink { get; set;  }
        public string? RoomCode { get; set; }
        public Room? RoomLink { get; set;  }
        public int ClassCount { get; set; }

        //relationship 1 - many
        public List<Enrollment>? EnrollmentList { get; set; }

        [NotMapped]
        public string TimeRange
         => $"{ClassTime:hh\\:mm} - {ClassTime.AddMinutes(DurationInMinutes):hh\\:mm}";
    }

    public class Room
    {
        public string RoomCode { get; set; }
        
        //relationship
       public string BuildingId { get; set; }
        public Building BuildingLink { get;set; }

        //relationship 1 - many
        public List<Class> ClassList { get; set; }

    }

    public class Building
    {
        public string BuildingId { get; set; }
        public string BuildingName { get; set; }

        //relationship
        public List<Room> RoomList { get; set; }
    }
}
