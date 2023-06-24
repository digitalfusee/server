using Data.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contexts
{
    public class CourseDataContext : DbContext
    {
        public CourseDataContext(DbContextOptions<CourseDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

            /* Creating a one-to-many relationship between the Question and Quiz tables. */
            modelBuilder.Entity<Question>().HasOne(q => q.Quiz).WithMany(q => q.Questions).HasForeignKey(q => q.QuizId);

        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Tutorial> Tutorials { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
    }
}
