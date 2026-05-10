using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.EnrollmentSystem
{
    public enum SchoolName
    {
        Business, Arts_And_Science, Education, Applied_Science
    }

    public class School
    {
        public string SchooldId { get; set; }
        public SchoolName SchoolName { get; set; }

        //relationships
        public string? FacultyId { get; set; } // Dean FK
        public Faculty? FacultyLink { get; set;  }

        //relationship 1 - many
        public List<Department> DepartmentList { get; set; } 
    }
}
