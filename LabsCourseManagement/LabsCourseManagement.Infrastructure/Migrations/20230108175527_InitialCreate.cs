using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabsCourseManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catalogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CourseCatalogId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Catalogs_CourseCatalogId",
                        column: x => x.CourseCatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Professors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Surname = table.Column<string>(type: "TEXT", nullable: true),
                    ContactInfoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professors_Contacts_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Surname = table.Column<string>(type: "TEXT", nullable: true),
                    ContactInfoId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Group = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_Contacts_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MyStrings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    String = table.Column<string>(type: "TEXT", nullable: true),
                    ContactId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CourseId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyStrings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MyStrings_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MyStrings_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TimesAndPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Classroom = table.Column<string>(type: "TEXT", nullable: true),
                    CourseId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimesAndPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimesAndPlaces_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseProfessor",
                columns: table => new
                {
                    CoursesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProfessorsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseProfessor", x => new { x.CoursesId, x.ProfessorsId });
                    table.ForeignKey(
                        name: "FK_CourseProfessor_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseProfessor_Professors_ProfessorsId",
                        column: x => x.ProfessorsId,
                        principalTable: "Professors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseStudent",
                columns: table => new
                {
                    CoursesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentsStudentId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudent", x => new { x.CoursesId, x.StudentsStudentId });
                    table.ForeignKey(
                        name: "FK_CourseStudent_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStudent_Students_StudentsStudentId",
                        column: x => x.StudentsStudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Laboratories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CourseId = table.Column<Guid>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    LaboratoryCatalogId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LaboratoryProfessorId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LaboratoryTimeAndPlaceId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laboratories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Laboratories_Catalogs_LaboratoryCatalogId",
                        column: x => x.LaboratoryCatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Laboratories_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Laboratories_Professors_LaboratoryProfessorId",
                        column: x => x.LaboratoryProfessorId,
                        principalTable: "Professors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Laboratories_TimesAndPlaces_LaboratoryTimeAndPlaceId",
                        column: x => x.LaboratoryTimeAndPlaceId,
                        principalTable: "TimesAndPlaces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Header = table.Column<string>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    PostingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WriterId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CourseId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LaboratoryId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Announcements_Laboratories_LaboratoryId",
                        column: x => x.LaboratoryId,
                        principalTable: "Laboratories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Announcements_Professors_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Professors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GradingInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ExaminationType = table.Column<int>(type: "INTEGER", nullable: false),
                    IsMandatory = table.Column<bool>(type: "INTEGER", nullable: false),
                    MinGrade = table.Column<double>(type: "REAL", nullable: false),
                    MaxGrade = table.Column<double>(type: "REAL", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    TimeAndPlaceId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CourseId = table.Column<Guid>(type: "TEXT", nullable: true),
                    LaboratoryId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradingInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradingInfos_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GradingInfos_Laboratories_LaboratoryId",
                        column: x => x.LaboratoryId,
                        principalTable: "Laboratories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GradingInfos_TimesAndPlaces_TimeAndPlaceId",
                        column: x => x.TimeAndPlaceId,
                        principalTable: "TimesAndPlaces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LaboratoryStudent",
                columns: table => new
                {
                    LaboratoriesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LaboratoryStudentsStudentId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoryStudent", x => new { x.LaboratoriesId, x.LaboratoryStudentsStudentId });
                    table.ForeignKey(
                        name: "FK_LaboratoryStudent_Laboratories_LaboratoriesId",
                        column: x => x.LaboratoriesId,
                        principalTable: "Laboratories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LaboratoryStudent_Students_LaboratoryStudentsStudentId",
                        column: x => x.LaboratoryStudentsStudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GradingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Mark = table.Column<double>(type: "REAL", nullable: false),
                    GradeType = table.Column<int>(type: "INTEGER", nullable: false),
                    Mentions = table.Column<string>(type: "TEXT", nullable: true),
                    StudentGradesId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentGrades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StudentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    FinalGradeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CatalogId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGrades_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentGrades_Grades_FinalGradeId",
                        column: x => x.FinalGradeId,
                        principalTable: "Grades",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentGrades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CourseId",
                table: "Announcements",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_LaboratoryId",
                table: "Announcements",
                column: "LaboratoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_WriterId",
                table: "Announcements",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseProfessor_ProfessorsId",
                table: "CourseProfessor",
                column: "ProfessorsId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseCatalogId",
                table: "Courses",
                column: "CourseCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudent_StudentsStudentId",
                table: "CourseStudent",
                column: "StudentsStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentGradesId",
                table: "Grades",
                column: "StudentGradesId");

            migrationBuilder.CreateIndex(
                name: "IX_GradingInfos_CourseId",
                table: "GradingInfos",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_GradingInfos_LaboratoryId",
                table: "GradingInfos",
                column: "LaboratoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GradingInfos_TimeAndPlaceId",
                table: "GradingInfos",
                column: "TimeAndPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Laboratories_CourseId",
                table: "Laboratories",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Laboratories_LaboratoryCatalogId",
                table: "Laboratories",
                column: "LaboratoryCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Laboratories_LaboratoryProfessorId",
                table: "Laboratories",
                column: "LaboratoryProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Laboratories_LaboratoryTimeAndPlaceId",
                table: "Laboratories",
                column: "LaboratoryTimeAndPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryStudent_LaboratoryStudentsStudentId",
                table: "LaboratoryStudent",
                column: "LaboratoryStudentsStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_MyStrings_ContactId",
                table: "MyStrings",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_MyStrings_CourseId",
                table: "MyStrings",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Professors_ContactInfoId",
                table: "Professors",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_CatalogId",
                table: "StudentGrades",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_FinalGradeId",
                table: "StudentGrades",
                column: "FinalGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_StudentId",
                table: "StudentGrades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ContactInfoId",
                table: "Students",
                column: "ContactInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TimesAndPlaces_CourseId",
                table: "TimesAndPlaces",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_StudentGrades_StudentGradesId",
                table: "Grades",
                column: "StudentGradesId",
                principalTable: "StudentGrades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_Catalogs_CatalogId",
                table: "StudentGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_Students_StudentId",
                table: "StudentGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_StudentGrades_StudentGradesId",
                table: "Grades");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "CourseProfessor");

            migrationBuilder.DropTable(
                name: "CourseStudent");

            migrationBuilder.DropTable(
                name: "GradingInfos");

            migrationBuilder.DropTable(
                name: "LaboratoryStudent");

            migrationBuilder.DropTable(
                name: "MyStrings");

            migrationBuilder.DropTable(
                name: "Laboratories");

            migrationBuilder.DropTable(
                name: "Professors");

            migrationBuilder.DropTable(
                name: "TimesAndPlaces");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Catalogs");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "StudentGrades");

            migrationBuilder.DropTable(
                name: "Grades");
        }
    }
}
