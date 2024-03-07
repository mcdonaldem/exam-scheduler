using ExamScheduler.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamScheduler.Contexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<MentorAvailability> MentorAvailabilities { get; set; }
        public DbSet<Student> Students { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Enrollments)
                .WithOne(e => e.Course)
                .IsRequired()
                ;

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Mentors)
                .WithMany(m => m.Courses)
                ;

            modelBuilder.Entity<Mentor>()
                .HasMany(m => m.Exams)
                .WithMany(e => e.Mentors)
                ;

            modelBuilder.Entity<MentorAvailability>()
                .HasOne(ma => ma.Mentor)
                .WithMany()
                .IsRequired()
                ;

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Enrollments)
                .WithOne(e => e.Student)
                .IsRequired()
                ;

            modelBuilder.Entity<Student>()
                .HasMany(s => s.Exams)
                .WithOne(e => e.Student)
                .IsRequired()
                ;
        }
    }
}
