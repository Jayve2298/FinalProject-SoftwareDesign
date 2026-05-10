using Bogus;
using FinalProject_Jenotan.Database;
using FinalProject_Jenotan.Database.Models.EnrollmentSystem;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Maintenance;
using FinalProject_Jenotan.Database.Models.MotorPoolSystem.Reservation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FinalProject_Jenotan.DatabaseSeed
{
    public static class SeedIds
    {
        // Schools
        public const string School1 = "SCH-BUS";
        public const string School2 = "SCH-ART";
        public const string School3 = "SCH-EDU";
        public const string School4 = "SCH-APS";

        // Departments
        public const string Dept1 = "DEP-MKT";
        public const string Dept2 = "DEP-FIN";
        public const string Dept3 = "DEP-ENG";
        public const string Dept4 = "DEP-EDU";

        // Special Faculty (LOGIN USER)
        public const string AdminFaculty = "FAC-ADMIN";
    }

    public static class SchoolSeeder
    {
        public static void Seed(TinyCollegeDbContextSQLServer context)
        {
            if (context.Schools.Any()) return;

            var faker = new Faker();
            var rand = new Random();

            
            // 1. CREATE 4 SCHOOLS
         
            var schoolEnums = Enum.GetValues(typeof(SchoolName))
                                  .Cast<SchoolName>()
                                  .Take(4)
                                  .ToList();

            var schools = new List<School>();

            for (int i = 0; i < 4; i++)
            {
                schools.Add(new School
                {
                    SchooldId = $"SCH - {1000 + i}",
                    SchoolName = schoolEnums[i]
                });
            }

            context.Schools.AddRange(schools);

           
            // 2. CREATE 5 DEPARTMENTS PER SCHOOL
           
            var departments = new List<Department>();
            int deptCounter = 0;

            string GenerateDepartmentName()
            {
                var prefixes = new[]
                {
                    "Department of",
                    "School of",
                    "Institute of"
                };

               var fields = new[]
               {
                    "Computer Science",
                    "Information Systems",
                    "Engineering",
                    "Business Administration",
                    "Finance",
                    "Marketing",
                    "Economics",
                    "Psychology",
                    "Education",
                    "Mathematics",
                    "Biology",
                    "Physics",
                    "Political Science",
                    "Communication",
                    "Data Science"
                };

                return $"{prefixes[rand.Next(prefixes.Length)]} {fields[rand.Next(fields.Length)]}";
            }

            foreach (var school in schools)
            {
                for (int i = 0; i < 5; i++)
                {
                    departments.Add(new Department
                    {
                        DepartmentId = $"DEPT - {1000 + deptCounter}",
                        DepartmentName = GenerateDepartmentName(),
                        IsActive = true,
                        SchoolId = school.SchooldId
                    });

                    deptCounter++;
                }
            }

            context.Departments.AddRange(departments);

            
            // 3. CREATE 50 FACULTY PER DEPARTMENT (ALL PROFESSORS)
          
            var faculties = new List<Faculty>();
            int facultyCounter = 0;

            foreach (var dept in departments)
            {
                for (int i = 0; i < 20; i++)
                {
                    var role = faker.Random.Double() < 0.2
                        ? Role.Researcher
                        : Role.Professor;

                    faculties.Add(new Faculty
                    {
                        FacultyId = $"FAC{1000 + facultyCounter}",
                        Password = $"TC{1000 + facultyCounter}",
                        FName = faker.Name.FirstName(),
                        LName = faker.Name.LastName(),
                        Email = faker.Internet.Email(),
                        ContactNumber = faker.Phone.PhoneNumber(),
                        ClassCount = 0,
                        IsActive = true,
                        Role = role,
                        DepartmentId = dept.DepartmentId
                    });

                    facultyCounter++;
                }
            }

            context.Faculties.AddRange(faculties);
            context.SaveChanges();

            
            // 3.5 CREATE 10 COURSES PER DEPARTMENT
           
            var courses = new List<Course>();
            int courseCounter = 0;

            var courseNames = new[]
            {
                "Introduction to Programming",
                "Data Structures",
                "Database Systems",
                "Operating Systems",
                "Software Engineering",
                "Web Development",
                "Mobile Application Development",
                "Computer Networks",
                "Artificial Intelligence",
                "Information Security",
                "Discrete Mathematics",
                "Algorithms",
                "Human Computer Interaction",
                "System Analysis",
                "Cloud Computing"
            };

            foreach (var dept in departments)
            {
                for (int i = 0; i < 10; i++)
                {
                    courses.Add(new Course
                    {
                        CourseId = $"CRS - {1000 + courseCounter}",
                        CourseName = courseNames[rand.Next(courseNames.Length)],

                        DepartmentId = dept.DepartmentId,
                        DepartmentLink = null,
                        ClassList = null,
                        IsActive = true
                    });
                   
                    courseCounter++;
                }
            }

            context.Courses.AddRange(courses);
            context.SaveChanges();

            //4. AssignDean

            foreach (var school in schools)
            {
                var schoolDepartments = departments
                    .Where(d => d.SchoolId == school.SchooldId)
                    .Select(d => d.DepartmentId)
                    .ToList();

                var schoolFaculty = faculties
                    .Where(f => schoolDepartments.Contains(f.DepartmentId))
                    .Where(f => f.Role == Role.Professor) // ensure no conflict
                    .ToList();

                var dean = schoolFaculty[rand.Next(schoolFaculty.Count)];

                dean.Role = Role.Dean;
                school.FacultyId = dean.FacultyId;
            }

            //5. Assign DepartmentChair

            foreach (var dept in departments)
            {
                var deptFaculty = faculties
                    .Where(f => f.DepartmentId == dept.DepartmentId)
                    .Where(f => f.Role == Role.Professor) // exclude deans
                    .ToList();

                var chair = deptFaculty[rand.Next(deptFaculty.Count)];

                chair.Role = Role.DepartmentChair;
                dept.FacultyId = chair.FacultyId;
            }
            context.SaveChanges();

            // 6 CREATE MAJORS PER DEPARTMENT

            var majors = new List<Major>();
            int majorCounter = 0;

            string GenerateMajorName()
            {
                var fields = new[]
                {
                    "Computer Science",
                    "Information Technology",
                    "Software Engineering",
                    "Data Science",
                    "Business Administration",
                    "Marketing Management",
                    "Financial Management",
                    "Psychology",
                    "Education",
                    "Mathematics"
                };

                return $"{fields[rand.Next(fields.Length)]}";
            }

            foreach (var dept in departments)
            {
                for (int i = 0; i < 3; i++) // 3 majors per department
                {
                    majors.Add(new Major
                    {
                        MajorId = $"MAJ - {1000 + majorCounter}",
                        MajorName = GenerateMajorName(),
                        IsActive = true,
                        StudentCount = 0,
                        DepartmentId = dept.DepartmentId,
                        DepartmentLink = null,
                        StudentList = null
                    });

                    majorCounter++;
                }
            }

            context.Majors.AddRange(majors);
            context.SaveChanges();

            //6.5. Create Student

            var students = new List<Student>();
            int studentCounter = 0;

            var allMajors = context.Majors.ToList();
            var allFaculties = context.Faculties.ToList();

            for (int i = 0; i < 50; i++)
            {
                var idNumber = 1000 + studentCounter;

                var selectedMajor = allMajors[rand.Next(allMajors.Count)];

                var validFaculties = allFaculties
                    .Where(f => f.DepartmentId == selectedMajor.DepartmentId)
                    .ToList();

                var selectedFaculty = validFaculties.Any()
                    ? validFaculties[rand.Next(validFaculties.Count)]
                    : null;

                students.Add(new Student
                {
                    StudentId = $"STU{idNumber}",
                    Password = $"TC{idNumber}",
                    FName = faker.Name.FirstName(),
                    LName = faker.Name.LastName(),
                    Email = faker.Internet.Email(),
                    Year = rand.Next(1, 5),
                    IsEnrolled = true,

                    MajorId = selectedMajor.MajorId,
                    FacultyId = selectedFaculty?.FacultyId,

                    MajorLink = null,
                    FacultyLink = null,
                    EnrollmentList = null
                });

                // ✅ increment major student count
                selectedMajor.StudentCount++;

                studentCounter++;
            }

            context.Students.AddRange(students);
            context.SaveChanges();

            //7. Create Buildings and room

            var buildings = new List<Building>();
            var rooms = new List<Room>();

            int buildingCounter = 0;
            int roomCounter = 0;

            for (int i = 0; i < 5; i++)
            {
                var building = new Building
                {
                    BuildingId = $"BLD - {1000 + buildingCounter}",
                    BuildingName = $"Building {faker.Address.City()}"
                };

                buildings.Add(building);

                for (int j = 0; j < 5; j++)
                {
                    rooms.Add(new Room
                    {
                        RoomCode = $"RM - {1000 + roomCounter}",
                        BuildingId = building.BuildingId
                    });

                    roomCounter++;
                }

                buildingCounter++;
            }

            context.Buildings.AddRange(buildings);
            context.Rooms.AddRange(rooms);
            context.SaveChanges();

            //8. Create Class

            var classes = new List<Class>();
            int classCounter = 0;

            // Track schedules with duration
            var roomSchedules = new Dictionary<string, List<(DayOfWeek day, TimeOnly start, TimeOnly end)>>();

            var durations = new[] { 90, 180 }; // 1.5 hrs or 3 hrs

            TimeOnly startBound = new TimeOnly(7, 30);
            TimeOnly endBound = new TimeOnly(19, 30);

            foreach (var dept in departments)
            {
                var deptCourses = courses
                    .Where(c => c.DepartmentId == dept.DepartmentId)
                    .ToList();

                var deptFaculty = faculties
                    .Where(f => f.DepartmentId == dept.DepartmentId &&
                           (f.Role == Role.Professor ||
                            f.Role == Role.Dean ||
                            f.Role == Role.DepartmentChair))
                    .ToList();

                for (int i = 0; i < 10; i++)
                {
                    bool valid = false;

                    while (!valid)
                    {
                        var selectedCourse = deptCourses[rand.Next(deptCourses.Count)];
                        var selectedFaculty = deptFaculty[rand.Next(deptFaculty.Count)];
                        var duration = durations[rand.Next(durations.Length)];

                        var day = (DayOfWeek)rand.Next(1, 6);

                        int totalMinutes = (endBound.Hour * 60 + endBound.Minute) -
                                           (startBound.Hour * 60 + startBound.Minute);

                        int randMinutes = rand.Next(0, totalMinutes);
                        var startTime = startBound.AddMinutes(randMinutes);
                        var endTime = startTime.AddMinutes(duration);

                        //
                        if (endTime > endBound) continue;

                        var room = rooms[rand.Next(rooms.Count)].RoomCode;

                        if (!roomSchedules.ContainsKey(room))
                            roomSchedules[room] = new List<(DayOfWeek, TimeOnly, TimeOnly)>();

                        // 🔥 FULL OVERLAP CHECK
                        bool hasConflict = roomSchedules[room].Any(s =>
                            s.day == day &&
                            (startTime < s.end && endTime > s.start)
                        );

                        if (!hasConflict)
                        {
                            roomSchedules[room].Add((day, startTime, endTime));

                            classes.Add(new Class
                            {
                                ClassId = $"CLASS - {1000 + classCounter}",
                                ClassTime = startTime,
                                DurationInMinutes = duration,
                                DayOfWeek = day,
                                CourseId = selectedCourse.CourseId,
                                FacultyId = selectedFaculty.FacultyId,
                                RoomCode = room,
                                ClassCount = 0,
                                EnrollmentList = null,
                                IsOnGoing = true
                            });

                            selectedCourse.ClassCount++;
                            selectedFaculty.ClassCount++;
                            classCounter++;
                            valid = true;
                        }
                    }
                }
            }

            context.Classes.AddRange(classes);
            context.SaveChanges();

            // 9. CREATE ENROLLMENTS

            var enrollments = new List<Enrollment>();
            int enrollmentCounter = 0;

            var allStudents = context.Students.ToList();
            var allClasses = context.Classes
                    .Include(c => c.CourseLink)
                    .ToList();

            string[] semesters = { "First", "Second" };

            foreach (var student in allStudents)
            {
                // get student's department via Major
                var studentMajor = allMajors.FirstOrDefault(m => m.MajorId == student.MajorId);
                if (studentMajor == null) continue;

                var studentDeptId = studentMajor.DepartmentId;

                // get classes under same department
                var validClasses = allClasses
                    .Where(c => c.CourseLink != null && c.CourseLink.DepartmentId == studentDeptId)
                    .ToList();

                if (!validClasses.Any()) continue;

                var usedClasses = new HashSet<string>();

                for (int i = 0; i < 5; i++)
                {
                    Class selectedClass;

                    do
                    {
                        selectedClass = validClasses[rand.Next(validClasses.Count)];
                    }
                    while (usedClasses.Contains(selectedClass.ClassId));

                    usedClasses.Add(selectedClass.ClassId);

                    var startDate = DateTime.Now.AddDays(-rand.Next(1, 60));
                    var endDate = startDate.AddMonths(3);

                    var enrollment = new Enrollment
                    {
                        EnrollmentId = $"ENR - {1000 + enrollmentCounter}",
                        Semester = semesters[rand.Next(semesters.Length)],
                        StartDate = startDate,
                        EndDate = endDate,
                        IsActive = true,
                        Grade = null,
                        StudentId = student.StudentId,
                        ClassId = selectedClass.ClassId
                    };

                    enrollments.Add(enrollment);

                    selectedClass.ClassCount++;

                    enrollmentCounter++;
                }
            }

            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();

            // 10. CREATE EMPLOYEES

            var employees = new List<Employee>();
            int employeeCounter = 0;

            for (int i = 0; i < 20; i++)
            {
                var idNumber = 1000 + employeeCounter;

                employees.Add(new Employee
                {
                    EmployeeId = $"EMP{idNumber}",
                    Password = $"TC{idNumber}",
                    FName = faker.Name.FirstName(),
                    LName = faker.Name.LastName(),
                    Email = $"employee{idNumber}@tinycollege.com",
                    ContactNumber = $"09{faker.Random.Number(100000000, 999999999)}",

                    CompletionFormList = null,
                    CheckoutFormList = null,
                    VehicleReportList = null,
                    PartsUsageReportList = null
                });

                employeeCounter++;
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            // 11. CREATE MECHANICS

            var mechanics = new List<Mechanic>();
            int mechanicCounter = 0;

            for (int i = 0; i < 20; i++)
            {
                var idNumber = 1000 + mechanicCounter;

                mechanics.Add(new Mechanic
                {
                    MechanicId = $"MEC{idNumber}",
                    Password = $"TC{idNumber}",
                    FName = faker.Name.FirstName(),
                    LName = faker.Name.LastName(),
                    Email = $"mechanic{idNumber}@tinycollege.com",

                    IsAuthorizedToSign = rand.NextDouble() < 0.5,
                    IsActive = true,

                    VehicleReleaseFormList = null,
                    InventoryList = null,
                    MaintenanceDetailList = null,
                    PartUsedFormList = null
                });

                mechanicCounter++;
            }

            context.Mechanics.AddRange(mechanics);
            context.SaveChanges();

            SeedInventoriesAndParts(context);

            // 12. CREATE VEHICLES

            var vehicles = new List<Vehicle>();
            int vehicleCounter = 0;

            var brands = new[]
            {
                "Toyota",
                "Honda",
                "Ford",
                "Nissan",
                "Mitsubishi", 
                "Hyundai",
                "Kia"
            };

            for (int i = 0; i < 10; i++)
            {
                var idNumber = 1000 + vehicleCounter;

                var mileageValue = rand.Next(10000, 200000);

                vehicles.Add(new Vehicle
                {
                    VehicleId = $"V - {idNumber}",
                    Brand = brands[rand.Next(brands.Length)],
                    Type = (VehicleType)rand.Next(Enum.GetValues(typeof(VehicleType)).Length),

                    Mileage = mileageValue.ToString(),
                    OdoReading = mileageValue,

                    CostPerMilage = rand.Next(10, 101), 
                    IsAvailable = true,

                    ReservationFormList = null,
                    CompletionFormList = null,
                    MaintenanceLogList = null
                });

                vehicleCounter++;
            }

            context.Vehicles.AddRange(vehicles);
            context.SaveChanges();
        }

        private static void SeedInventoriesAndParts(TinyCollegeDbContextSQLServer context)
        {
            if (context.Inventories.Any() || context.Parts.Any()) return;

            var rand = new Random();

            var inventories = new List<Inventory>();
            var parts = new List<Parts>();

            int inventoryCounter = 1000;
            int partCounter = 1000;

            // 🔥 ONLY authorized mechanics
            var mechanics = context.Mechanics
                .Where(m => m.IsAuthorizedToSign)
                .ToList();

            var partNames = new[]
            {
                "Brake Pad",
                "Oil Filter",
                "Air Filter",
                "Spark Plug",
                "Battery",
                "Headlight",
                "Radiator Hose",
                "Fuel Pump",
                "Alternator",
                "Clutch Plate"
            };

            foreach (var mechanic in mechanics)
            {
                for (int i = 0; i < 3; i++)
                {
                    var inventory = new Inventory
                    {
                        InventoryId = $"INV - {inventoryCounter++}",
                        MechanicId = mechanic.MechanicId,
                        PartsList = new List<Parts>()
                    };

                    inventories.Add(inventory);

                    for (int j = 0; j < 5; j++)
                    {
                        var part = new Parts
                        {
                            PartId = $"PRT - {partCounter++}",
                            PartName = partNames[rand.Next(partNames.Length)],
                            Quantity = rand.Next(1, 50),

                            InventoryId = inventory.InventoryId,
                            PartsUsedFormList = null
                        };

                        parts.Add(part);
                    }
                }
            }

            context.Inventories.AddRange(inventories);
            context.Parts.AddRange(parts);
            context.SaveChanges();
        }
    }
}
