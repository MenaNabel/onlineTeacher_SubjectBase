
using OnlineTeacher.ViewModels.Questions.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Questions
{
    public class AddedQuestionViewModel : QuestionViewModel
    {
        [Required]
        [Range(1,double.MaxValue)]
        public int ExamID { get; set; }
        public ExamHelperViewModel Exam { get; set; }
    }
}
