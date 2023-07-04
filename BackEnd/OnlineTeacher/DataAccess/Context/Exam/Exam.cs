using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    public class Exam
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Column("Total Degree")]
        [Range(0 , double.MaxValue)]
        public double Degree { get; set; }
        [ForeignKey(nameof(Lecture))]
        public int LectureID { get; set; }
        // Date For Exam Date
        // Time For Examming Expire Time
        public DateTime DateAndExamminationExpireTime { get; set; }
        [Range(0, double.MaxValue)]
        public double ExamDuration { get; set; }
        public string EaxamType { get; set; }
        public ICollection<StudentExam> StudentExams { get; set; }
        public ICollection<ExamQuestion> ExamQuestion { get; set; }
        public Lecture Lecture { get; set; }
        public Exam()
        {
            StudentExams = new HashSet<StudentExam>();
            ExamQuestion = new HashSet<ExamQuestion>();
        }
    }
}
