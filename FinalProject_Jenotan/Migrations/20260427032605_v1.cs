using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject_Jenotan.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    BuildingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuildingName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.BuildingId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Mechanics",
                columns: table => new
                {
                    MechanicId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsAuthorizedToSign = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mechanics", x => x.MechanicId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Mileage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostPerMilage = table.Column<float>(type: "real", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    OdoReading = table.Column<float>(type: "real", nullable: false, defaultValue: 0f)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BuildingId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomCode);
                    table.ForeignKey(
                        name: "FK_Rooms_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "BuildingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartsUsageReports",
                columns: table => new
                {
                    PURId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalPartsUsed = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsUsageReports", x => x.PURId);
                    table.ForeignKey(
                        name: "FK_PartsUsageReports_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartsUsageReports_Employees_EmployeeId1",
                        column: x => x.EmployeeId1,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    InventoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MechanicId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.InventoryId);
                    table.ForeignKey(
                        name: "FK_Inventories_Mechanics_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanics",
                        principalColumn: "MechanicId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceLogs",
                columns: table => new
                {
                    MLogId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateLogged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceLogs", x => x.MLogId);
                    table.ForeignKey(
                        name: "FK_MaintenanceLogs_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceLogs_Vehicles_VehicleId1",
                        column: x => x.VehicleId1,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateTable(
                name: "VehicleReports",
                columns: table => new
                {
                    VReportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimesUsed = table.Column<int>(type: "int", nullable: false),
                    TotalMileageGained = table.Column<float>(type: "real", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmployeeLinkEmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleReports", x => x.VReportId);
                    table.ForeignKey(
                        name: "FK_VehicleReports_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_VehicleReports_Employees_EmployeeLinkEmployeeId",
                        column: x => x.EmployeeLinkEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_VehicleReports_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    PartId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PartName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    InvenotryId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.PartId);
                    table.ForeignKey(
                        name: "FK_Parts_Inventories_InvenotryId",
                        column: x => x.InvenotryId,
                        principalTable: "Inventories",
                        principalColumn: "InventoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceDetails",
                columns: table => new
                {
                    MDetailsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaintenancePerformed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateLogged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MechanicId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MLogId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceDetails", x => x.MDetailsId);
                    table.ForeignKey(
                        name: "FK_MaintenanceDetails_MaintenanceLogs_MLogId",
                        column: x => x.MLogId,
                        principalTable: "MaintenanceLogs",
                        principalColumn: "MLogId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceDetails_Mechanics_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanics",
                        principalColumn: "MechanicId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleReleaseForms",
                columns: table => new
                {
                    VRFormId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateSigned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MechanicId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MLogId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleReleaseForms", x => x.VRFormId);
                    table.ForeignKey(
                        name: "FK_VehicleReleaseForms_MaintenanceLogs_MLogId",
                        column: x => x.MLogId,
                        principalTable: "MaintenanceLogs",
                        principalColumn: "MLogId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleReleaseForms_Mechanics_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanics",
                        principalColumn: "MechanicId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartsUsedForms",
                columns: table => new
                {
                    PUFormId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QtyUsed = table.Column<int>(type: "int", nullable: false),
                    PartsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MLogId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PURId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MechanicId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MDetailsId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartsUsedForms", x => x.PUFormId);
                    table.ForeignKey(
                        name: "FK_PartsUsedForms_MaintenanceDetails_MDetailsId",
                        column: x => x.MDetailsId,
                        principalTable: "MaintenanceDetails",
                        principalColumn: "MDetailsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartsUsedForms_MaintenanceLogs_MLogId",
                        column: x => x.MLogId,
                        principalTable: "MaintenanceLogs",
                        principalColumn: "MLogId");
                    table.ForeignKey(
                        name: "FK_PartsUsedForms_Mechanics_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanics",
                        principalColumn: "MechanicId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartsUsedForms_PartsUsageReports_PURId",
                        column: x => x.PURId,
                        principalTable: "PartsUsageReports",
                        principalColumn: "PURId");
                    table.ForeignKey(
                        name: "FK_PartsUsedForms_Parts_PartsId",
                        column: x => x.PartsId,
                        principalTable: "Parts",
                        principalColumn: "PartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckoutForms",
                columns: table => new
                {
                    CheckoutId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsOnGoing = table.Column<bool>(type: "bit", nullable: false),
                    IsCheckedOut = table.Column<bool>(type: "bit", nullable: false),
                    FacultyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RFormId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutForms", x => x.CheckoutId);
                    table.ForeignKey(
                        name: "FK_CheckoutForms_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    ClassId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOnGoing = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FacultyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClassCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.ClassId);
                    table.ForeignKey(
                        name: "FK_Classes_Rooms_RoomCode",
                        column: x => x.RoomCode,
                        principalTable: "Rooms",
                        principalColumn: "RoomCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompletionForms",
                columns: table => new
                {
                    CFormId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartOdo = table.Column<float>(type: "real", nullable: false),
                    EndOdo = table.Column<float>(type: "real", nullable: false),
                    Complaints = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuelCost = table.Column<float>(type: "real", nullable: false),
                    CreditCardNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TripCost = table.Column<float>(type: "real", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FacultyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    VReportId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CheckoutId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckoutFormLinkCheckoutId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FacultyId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletionForms", x => x.CFormId);
                    table.ForeignKey(
                        name: "FK_CompletionForms_CheckoutForms_CheckoutFormLinkCheckoutId",
                        column: x => x.CheckoutFormLinkCheckoutId,
                        principalTable: "CheckoutForms",
                        principalColumn: "CheckoutId");
                    table.ForeignKey(
                        name: "FK_CompletionForms_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionForms_VehicleReports_VReportId",
                        column: x => x.VReportId,
                        principalTable: "VehicleReports",
                        principalColumn: "VReportId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionForms_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacultyId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SchoolId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Faculties",
                columns: table => new
                {
                    FacultyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculties", x => x.FacultyId);
                    table.ForeignKey(
                        name: "FK_Faculties_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    MajorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MajorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    StudentCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.MajorId);
                    table.ForeignKey(
                        name: "FK_Majors_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationForms",
                columns: table => new
                {
                    RFormId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartureDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISApproved = table.Column<bool>(type: "bit", nullable: false),
                    FacultyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FacultyId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationForms", x => x.RFormId);
                    table.ForeignKey(
                        name: "FK_ReservationForms_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservationForms_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservationForms_Faculties_FacultyId1",
                        column: x => x.FacultyId1,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId");
                    table.ForeignKey(
                        name: "FK_ReservationForms_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    SchooldId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FacultyId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.SchooldId);
                    table.ForeignKey(
                        name: "FK_Schools_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsEnrolled = table.Column<bool>(type: "bit", nullable: false),
                    MajorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FacultyId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_Faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculties",
                        principalColumn: "FacultyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "MajorId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Grade = table.Column<float>(type: "real", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_Enrollments_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutForms_EmployeeId",
                table: "CheckoutForms",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutForms_FacultyId",
                table: "CheckoutForms",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutForms_RFormId",
                table: "CheckoutForms",
                column: "RFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseId",
                table: "Classes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_FacultyId",
                table: "Classes",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_RoomCode",
                table: "Classes",
                column: "RoomCode");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionForms_CheckoutFormLinkCheckoutId",
                table: "CompletionForms",
                column: "CheckoutFormLinkCheckoutId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionForms_EmployeeId",
                table: "CompletionForms",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionForms_FacultyId",
                table: "CompletionForms",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionForms_FacultyId1",
                table: "CompletionForms",
                column: "FacultyId1");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionForms_VehicleId",
                table: "CompletionForms",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionForms_VReportId",
                table: "CompletionForms",
                column: "VReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_FacultyId",
                table: "Departments",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_SchoolId",
                table: "Departments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_ClassId",
                table: "Enrollments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId",
                table: "Enrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Faculties_DepartmentId",
                table: "Faculties",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_MechanicId",
                table: "Inventories",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceDetails_MechanicId",
                table: "MaintenanceDetails",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceDetails_MLogId",
                table: "MaintenanceDetails",
                column: "MLogId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLogs_VehicleId",
                table: "MaintenanceLogs",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceLogs_VehicleId1",
                table: "MaintenanceLogs",
                column: "VehicleId1");

            migrationBuilder.CreateIndex(
                name: "IX_Majors_DepartmentId",
                table: "Majors",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_InvenotryId",
                table: "Parts",
                column: "InvenotryId");

            migrationBuilder.CreateIndex(
                name: "IX_PartsUsageReports_EmployeeId",
                table: "PartsUsageReports",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PartsUsageReports_EmployeeId1",
                table: "PartsUsageReports",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_PartsUsedForms_MDetailsId",
                table: "PartsUsedForms",
                column: "MDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_PartsUsedForms_MechanicId",
                table: "PartsUsedForms",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_PartsUsedForms_MLogId",
                table: "PartsUsedForms",
                column: "MLogId");

            migrationBuilder.CreateIndex(
                name: "IX_PartsUsedForms_PartsId",
                table: "PartsUsedForms",
                column: "PartsId");

            migrationBuilder.CreateIndex(
                name: "IX_PartsUsedForms_PURId",
                table: "PartsUsedForms",
                column: "PURId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationForms_EmployeeId",
                table: "ReservationForms",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationForms_FacultyId",
                table: "ReservationForms",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationForms_FacultyId1",
                table: "ReservationForms",
                column: "FacultyId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationForms_VehicleId",
                table: "ReservationForms",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BuildingId",
                table: "Rooms",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_FacultyId",
                table: "Schools",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_FacultyId",
                table: "Students",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_MajorId",
                table: "Students",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReleaseForms_MechanicId",
                table: "VehicleReleaseForms",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReleaseForms_MLogId",
                table: "VehicleReleaseForms",
                column: "MLogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReports_EmployeeId",
                table: "VehicleReports",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReports_EmployeeLinkEmployeeId",
                table: "VehicleReports",
                column: "EmployeeLinkEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleReports_VehicleId",
                table: "VehicleReports",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutForms_Faculties_FacultyId",
                table: "CheckoutForms",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutForms_ReservationForms_RFormId",
                table: "CheckoutForms",
                column: "RFormId",
                principalTable: "ReservationForms",
                principalColumn: "RFormId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Courses_CourseId",
                table: "Classes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Faculties_FacultyId",
                table: "Classes",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionForms_Faculties_FacultyId",
                table: "CompletionForms",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionForms_Faculties_FacultyId1",
                table: "CompletionForms",
                column: "FacultyId1",
                principalTable: "Faculties",
                principalColumn: "FacultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Departments_DepartmentId",
                table: "Courses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Faculties_FacultyId",
                table: "Departments",
                column: "FacultyId",
                principalTable: "Faculties",
                principalColumn: "FacultyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Schools_SchoolId",
                table: "Departments",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "SchooldId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Faculties_FacultyId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Schools_Faculties_FacultyId",
                table: "Schools");

            migrationBuilder.DropTable(
                name: "CompletionForms");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "PartsUsedForms");

            migrationBuilder.DropTable(
                name: "VehicleReleaseForms");

            migrationBuilder.DropTable(
                name: "CheckoutForms");

            migrationBuilder.DropTable(
                name: "VehicleReports");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "MaintenanceDetails");

            migrationBuilder.DropTable(
                name: "PartsUsageReports");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "ReservationForms");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropTable(
                name: "MaintenanceLogs");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Mechanics");

            migrationBuilder.DropTable(
                name: "Faculties");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Schools");
        }
    }
}
