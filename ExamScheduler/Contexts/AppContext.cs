using ExamScheduler.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamScheduler.Contexts
{
    public class AppContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<MentorAvailability> MentorAvailabilities { get; set; }
        public DbSet<Student> Students { get; set; }

        public AppContext(DbContextOptions options) : base(options)
        {
        }
    }
}
