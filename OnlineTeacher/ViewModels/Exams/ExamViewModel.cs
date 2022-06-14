using OnlineTeacher.ViewModels.Exams.Helper;
using OnlineTeacher.ViewModels.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Exams
{
    public class ExamViewModel : ExamViewModelWithLecture
    {
        public IEnumerable<QuestionViewModel> Questions { get; set; }
        

        public ExamViewModel()
        {
            Questions = new HashSet<QuestionViewModel>();
        }
    }
}
