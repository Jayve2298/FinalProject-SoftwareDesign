using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.EnrollmentSystem
{
    public class Course
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public bool IsActive { get; set; }
        //relationship
        public string DepartmentId { get; set; }
        public Department DepartmentLink { get; set; }
        public int ClassCount { get; set; }

        //relationship 1 - many
        public List<Class>? ClassList { get; set; }
    }
}
