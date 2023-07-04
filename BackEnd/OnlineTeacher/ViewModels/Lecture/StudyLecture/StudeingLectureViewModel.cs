using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Lecture.Helper
{
    public class StudeingLectureViewModel : LectureViewModel
    {
        public string FileName { get; set; }
        public bool IsAppear { get; set; }
        public bool IsFree { get; set; }
        public int Month { get; set; }

        internal StudeingLectureViewModel Select(object addExamID)
        {
            throw new NotImplementedException();
        }
    }
}
