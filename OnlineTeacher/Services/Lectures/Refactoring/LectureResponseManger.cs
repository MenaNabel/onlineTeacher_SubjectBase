﻿using OnlineTeacher.Shared.ViewModel;
using OnlineTeacher.ViewModels.Lecture.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Lectures.Refactoring
{
    public class LectureResponseManger : UserMangerResonse
    {
       public StudeingLectureViewModel studeingLecture { get; set; }
    }
}
