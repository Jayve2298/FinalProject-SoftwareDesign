using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject_Jenotan.Database.Models.EnrollmentSystem
{
    public class Enrollment
    {
        public string EnrollmentId {  get; set; }
        public string Semester { get; set; }
        public DateTime StartDate {  get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public float? Grade { get; set; }
        //relationship
        public string StudentId { get; set; }
        public Student StudentLink { get; set; }
        public string ClassId { get; set; }
        public Class ClassLink { get; set; }
    }
}
