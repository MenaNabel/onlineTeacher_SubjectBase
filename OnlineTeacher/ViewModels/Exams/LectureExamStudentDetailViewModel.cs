using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Exams
{
    public class LectureExamDetailViewModel
    {

        public double Degree { get; set; }
        public DateTime SubmitTime { get; set; }
        public string LectureName { get; set; }
        public string ExamName { get; set; }
    }
    public class LectureExamStudentDetailViewModel : LectureExamDetailViewModel
    {
        public string StudentName { get; set; }
        public string PhoneNumber { get; set; }
        public string ExamPeriod { get; set; }
    }
}
