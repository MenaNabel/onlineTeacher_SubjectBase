using OnlineTeacher.ViewModels.Exams.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Exams
{
    public class ExamViewModelWithLecture : AddedExamViewModel
    {
        public LectureHelperViewModel Lecture { get; set; }
    }
}
