using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.EnrollmentSystem
{
    public class Department
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        //relationship
        public string? FacultyId { get; set; } //DeptChair FK
        public Faculty? FacultyLink { get; set; }
        public string SchoolId {  get; set; }
        public School SchoolLink { get; set; }
        public bool IsActive { get; set; }

        //relationship 1 - Many
        public List<Faculty> FacultyList { get; set; }
        public List<Major> MajorList { get; set; }
        public List<Course> CourseList { get; set; }
    }
}
