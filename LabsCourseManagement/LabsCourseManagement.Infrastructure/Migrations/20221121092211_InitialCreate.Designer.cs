﻿// <auto-generated />
using System;
using LabsCourseManagement.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LabsCourseManagement.Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20221121092211_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("CourseProfessor", b =>
                {
                    b.Property<Guid>("CoursesId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProfessorsId")
                        .HasColumnType("TEXT");

                    b.HasKey("CoursesId", "ProfessorsId");

                    b.HasIndex("ProfessorsId");

                    b.ToTable("CourseProfessor");
                });

            modelBuilder.Entity("CourseStudent", b =>
                {
                    b.Property<Guid>("CourseStudentsStudentId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CoursesId")
                        .HasColumnType("TEXT");

                    b.HasKey("CourseStudentsStudentId", "CoursesId");

                    b.HasIndex("CoursesId");

                    b.ToTable("CourseStudent");
                });

            modelBuilder.Entity("LaboratoryStudent", b =>
                {
                    b.Property<Guid>("LaboratoriesId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LaboratoryStudentsStudentId")
                        .HasColumnType("TEXT");

                    b.HasKey("LaboratoriesId", "LaboratoryStudentsStudentId");

                    b.HasIndex("LaboratoryStudentsStudentId");

                    b.ToTable("LaboratoryStudent");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Announcement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Header")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("LaboratoryId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PostingDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("WriterId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("LaboratoryId");

                    b.HasIndex("WriterId");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Catalog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Catalogs");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CourseCatalogId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseCatalogId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Grade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("GradeType")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("GradingDate")
                        .HasColumnType("TEXT");

                    b.Property<double>("Mark")
                        .HasColumnType("REAL");

                    b.Property<string>("Mentions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("StudentGradesId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("StudentGradesId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.GradingInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ExaminationType")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsMandatory")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("LaboratoryId")
                        .HasColumnType("TEXT");

                    b.Property<double>("MaxGrade")
                        .HasColumnType("REAL");

                    b.Property<double>("MinGrade")
                        .HasColumnType("REAL");

                    b.Property<Guid>("TimeAndPlaceId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("LaboratoryId");

                    b.HasIndex("TimeAndPlaceId");

                    b.ToTable("GradingInfos");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Laboratory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("LaboratoryCatalogId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LaboratoryProfessorId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LaboratoryTimeAndPlaceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("LaboratoryCatalogId");

                    b.HasIndex("LaboratoryProfessorId");

                    b.HasIndex("LaboratoryTimeAndPlaceId");

                    b.ToTable("Laboratories");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.MyString", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ContactId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<string>("String")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("CourseId");

                    b.ToTable("MyStrings");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Professor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ContactInfoId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContactInfoId");

                    b.ToTable("Professors");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Student", b =>
                {
                    b.Property<Guid>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ContactInfoId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("StudentId");

                    b.HasIndex("ContactInfoId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.StudentGrades", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CatalogId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("FinalGradeId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CatalogId");

                    b.HasIndex("FinalGradeId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentGrades");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.TimeAndPlace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Classroom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateAndTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("TimesAndPlaces");
                });

            modelBuilder.Entity("CourseProfessor", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LabsCourseManagement.Domain.Professor", null)
                        .WithMany()
                        .HasForeignKey("ProfessorsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseStudent", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Student", null)
                        .WithMany()
                        .HasForeignKey("CourseStudentsStudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LabsCourseManagement.Domain.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LaboratoryStudent", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Laboratory", null)
                        .WithMany()
                        .HasForeignKey("LaboratoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LabsCourseManagement.Domain.Student", null)
                        .WithMany()
                        .HasForeignKey("LaboratoryStudentsStudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Announcement", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Course", null)
                        .WithMany("CourseAnnouncements")
                        .HasForeignKey("CourseId");

                    b.HasOne("LabsCourseManagement.Domain.Laboratory", null)
                        .WithMany("LaboratoryAnnouncements")
                        .HasForeignKey("LaboratoryId");

                    b.HasOne("LabsCourseManagement.Domain.Professor", "Writer")
                        .WithMany()
                        .HasForeignKey("WriterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Writer");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Course", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Catalog", "CourseCatalog")
                        .WithMany()
                        .HasForeignKey("CourseCatalogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseCatalog");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Grade", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.StudentGrades", null)
                        .WithMany("Grades")
                        .HasForeignKey("StudentGradesId");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.GradingInfo", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Course", null)
                        .WithMany("CourseGradingInfo")
                        .HasForeignKey("CourseId");

                    b.HasOne("LabsCourseManagement.Domain.Laboratory", null)
                        .WithMany("LaboratoryGradingInfo")
                        .HasForeignKey("LaboratoryId");

                    b.HasOne("LabsCourseManagement.Domain.TimeAndPlace", "TimeAndPlace")
                        .WithMany()
                        .HasForeignKey("TimeAndPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TimeAndPlace");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Laboratory", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Course", "Course")
                        .WithMany("Laboratorys")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LabsCourseManagement.Domain.Catalog", "LaboratoryCatalog")
                        .WithMany()
                        .HasForeignKey("LaboratoryCatalogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LabsCourseManagement.Domain.Professor", "LaboratoryProfessor")
                        .WithMany("Laboratories")
                        .HasForeignKey("LaboratoryProfessorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LabsCourseManagement.Domain.TimeAndPlace", "LaboratoryTimeAndPlace")
                        .WithMany()
                        .HasForeignKey("LaboratoryTimeAndPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("LaboratoryCatalog");

                    b.Navigation("LaboratoryProfessor");

                    b.Navigation("LaboratoryTimeAndPlace");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.MyString", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Contact", null)
                        .WithMany("EmailAddresses")
                        .HasForeignKey("ContactId");

                    b.HasOne("LabsCourseManagement.Domain.Course", null)
                        .WithMany("HelpfulMaterials")
                        .HasForeignKey("CourseId");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Professor", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Contact", "ContactInfo")
                        .WithMany()
                        .HasForeignKey("ContactInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContactInfo");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Student", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Contact", "ContactInfo")
                        .WithMany()
                        .HasForeignKey("ContactInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContactInfo");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.StudentGrades", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Catalog", null)
                        .WithMany("StudentGrades")
                        .HasForeignKey("CatalogId");

                    b.HasOne("LabsCourseManagement.Domain.Grade", "FinalGrade")
                        .WithMany()
                        .HasForeignKey("FinalGradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LabsCourseManagement.Domain.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinalGrade");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.TimeAndPlace", b =>
                {
                    b.HasOne("LabsCourseManagement.Domain.Course", null)
                        .WithMany("CourseProgram")
                        .HasForeignKey("CourseId");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Catalog", b =>
                {
                    b.Navigation("StudentGrades");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Contact", b =>
                {
                    b.Navigation("EmailAddresses");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Course", b =>
                {
                    b.Navigation("CourseAnnouncements");

                    b.Navigation("CourseGradingInfo");

                    b.Navigation("CourseProgram");

                    b.Navigation("HelpfulMaterials");

                    b.Navigation("Laboratorys");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Laboratory", b =>
                {
                    b.Navigation("LaboratoryAnnouncements");

                    b.Navigation("LaboratoryGradingInfo");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.Professor", b =>
                {
                    b.Navigation("Laboratories");
                });

            modelBuilder.Entity("LabsCourseManagement.Domain.StudentGrades", b =>
                {
                    b.Navigation("Grades");
                });
#pragma warning restore 612, 618
        }
    }
}