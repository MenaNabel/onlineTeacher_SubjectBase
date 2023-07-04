using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    [Index(nameof(StudentID))]
    public class StudentExam
    {
        [Key]
        [Column(Order =1)]
        [ForeignKey(nameof(Student))]
        [Required]
        public int StudentID { get; set; }
        [Key]
        [Column(Order = 2)]
        [ForeignKey(nameof(Exam))]
        [Required]
        public int ExamID { get; set; }
        public double Degree { get; set; } = 0;
        public bool IsSubmitted { get; set; } = true;
        public bool IsActive { get; set; } = false;
        public string Reason { get; set; }
        public bool HaveRequest { get; set; } = false;
        public DateTime SubmitTime { get; set; }
        [Required]
        public bool IsPassed { get; set; }
        public string ExamType { get; set; }

        // Bridges
        public Student Student { get; set; }
        public Exam Exam { get; set; }
      //  public object Type { get; internal set; }
    }
}
