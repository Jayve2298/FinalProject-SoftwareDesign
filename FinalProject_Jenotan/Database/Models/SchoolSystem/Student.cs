using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.EnrollmentSystem
{
    public class Student
    {
        public string StudentId { get; set; }
        public string Password { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
        public bool IsEnrolled { get; set; }

        //relationship
        public string? MajorId { get; set; }
        public Major? MajorLink { get; set; }
        public string? FacultyId { get; set; }
        public Faculty? FacultyLink { get; set; }

        //relationship 1 - many
        public List<Enrollment>? EnrollmentList { get; set; }
    }
}
