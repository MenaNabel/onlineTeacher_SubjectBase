using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Exams
{
    public class ReOpenExamDetailsViewModel
    {
        public int StudentID { get; set; }
        public int ExamID { get; set; }
        public string StudentName { get; set; }
        public string ExamName { get; set; }
        public string Reasone { get; set; }
    }
}
