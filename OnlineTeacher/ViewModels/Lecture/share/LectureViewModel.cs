﻿using OnlineTeacher.ViewModels.Exams.Helper;
using OnlineTeacher.ViewModels.Lecture.Helper;
using System;

namespace OnlineTeacher.ViewModels.Lecture
{
    public class LectureViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LectureLink { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public SubjectHelperViewModel Subject { get; set; }
        public LectureHelperViewModel previousLecture { get; set; }
        public int ExamID { get; set; }
    }
}