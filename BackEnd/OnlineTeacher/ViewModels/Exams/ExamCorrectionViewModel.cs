using OnlineTeacher.ViewModels.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Exams
{
    public class ExamCorrectionViewModel
    {
        public int ExamID { get; set; }
        public double TotalDegree { get; set; }
        public double ActualDegree { get; set; }
        public IEnumerable<QuestionCorrectionViewModel> Questions { get; set; }
    }
}
