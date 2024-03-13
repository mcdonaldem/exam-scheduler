﻿using ExamScheduler.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamScheduler.Contexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<AlgoLanguage> AlgoLanguages { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<MentorAvailability> MentorAvailabilities { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentExamDetail> StudentExamDetails { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Mentors)
                .WithMany(m => m.Courses)
                ;

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .IsRequired()
                ;

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .IsRequired()
                ;


            modelBuilder.Entity<Exam>()
                 .HasOne(e => e.StudentDetail)
                 .WithOne()
                 .HasForeignKey<Exam>(e => e.StudentDetailId)
                 .IsRequired()
                 ;

            modelBuilder.Entity<Mentor>()
                .ToTable("Mentors")
                ;

            modelBuilder.Entity<Mentor>()
                .HasMany(m => m.Exams)
                .WithMany(e => e.Mentors)
                ;

            modelBuilder.Entity<Mentor>()
                .HasMany(m => m.AlgoLanguages)
                .WithMany()
                ;

            modelBuilder.Entity<MentorAvailability>()
                .HasOne(ma => ma.Mentor)
                .WithMany()
                .IsRequired()
                ;

            modelBuilder.Entity<Student>()
                .ToTable("Students")
                ;

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Enrollments)
                .WithOne(e => e.Student)
                .IsRequired()
                ;

            modelBuilder.Entity<Student>()
                .HasMany(s => s.ExamDetails)
                .WithOne(e => e.Student)
                .IsRequired()
                ;

            modelBuilder.Entity<StudentExamDetail>()
                .HasOne(s => s.AlgoLanguage)
                .WithMany()
                .IsRequired()
                ;
        }
    }
}
