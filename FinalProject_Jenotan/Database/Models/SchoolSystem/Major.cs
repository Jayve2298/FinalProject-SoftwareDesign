using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.EnrollmentSystem
{
    public class Major
    {
        public string MajorId { get; set; }
        public string MajorName { get; set; }
        public bool IsActive { get; set;  }
        public int StudentCount { get; set; }
        //relationship
        public string DepartmentId { get; set; }
        public Department DepartmentLink { get; set; }

        //relationship 1 - many
        public List<Student> StudentList { get; set; }
    }
}
