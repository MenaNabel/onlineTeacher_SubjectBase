using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    public class ExamQuestion
    {
        [Key]
        [ForeignKey(nameof(Question))]
        [Column(Order = 1 )]
        public int QuestionID { get; set; }
        [Key]
        [ForeignKey(nameof(Exam))]
        [Column(Order = 2)]
        public int ExamID { get; set; }
        // Bridges 
        public Question Question { get; set; }
        public Exam Exam { get; set; }
    }
}
