using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Exams
{
    public class ReopenExamFeedback
    {
        public int StudentID { get; set; }
        public int ExamID { get; set; }
        public bool IsAccept { get; set; }
    }
}
