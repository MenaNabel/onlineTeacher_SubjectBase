using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.DataAccess.Context.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess
{
    public class OnlineExamContext : IdentityDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<ExamQuestion>().HasKey(table => new {
                table.ExamID,
                table.QuestionID
            }); 

            modelBuilder.Entity<StudentExam>().HasKey(table => new {
                table.ExamID,
                table.StudentID
            });
            modelBuilder.Entity<Subscription>().HasKey(table => new {
                table.StudentID,
                table.SubjectID
            });
            modelBuilder.Entity<Subject>().HasIndex(su => su.LevelID);
            modelBuilder.Entity<Watching>().HasKey(table => new {
                table.StudentID,
                table.LectureID

            });
            modelBuilder.Entity<Student>().Property(studen => studen.Phone).HasDefaultValue("لا يوجد هاتف لهذا الشخص").IsRequired();
            modelBuilder.Entity<Student>().Property(studen => studen.ParentPhone).HasDefaultValue("لا يوجد هاتف لهذا الشخص").IsRequired();
            base.OnModelCreating(modelBuilder);
        }
        public OnlineExamContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<StudentExam> StudentExam { get; set; }
        public DbSet<Exam> Exam { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<SiteInfo> SiteInfo { get; set; }
        public DbSet<HonerList> HonerLists { get; set; }
        public DbSet<LectureDetails> LectureDetails { get; set; }
        public DbSet<Watching> Watchings { get; set; }

    }
}
