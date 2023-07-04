using OnlineTeacher.ViewModels.StudentExam;
using System.Collections.Generic;

namespace OnlineTeacher.ViewModels.Students
{
    public class StudentProfileViewModel : StudentViewModel
    {
        public IEnumerable<Grades> ExamGrades { get; set; }
    }
}
