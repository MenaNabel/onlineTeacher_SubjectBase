using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    public class Question
    {
        [Key]
        public int ID { get; set;}
        [Required]
        public string Description { get; set;}
        [Required]
        public string OneAnswer { get; set;}
        [Required]
        public string SecondAnswer { get; set;}
        [Required]
        public string ThirdAnswer { get; set;}
        [Required]
        public string FourthAnswer { get; set;}
        [Required]
        [Range(1,4)]
        public int CorrectAnswer { get; set; }
        public byte[] QuestionImage { get; set; }
        public string File { get; set; }
        // Bridges
        public ICollection<ExamQuestion> ExamQuestions { get; set; }
        public Question()
        {
            ExamQuestions = new HashSet<ExamQuestion>();
         
        }
    }
}
