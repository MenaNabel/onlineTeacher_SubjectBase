using OnlineTeacher.ViewModels.Lecture;
using OnlineTeacher.ViewModels.Lecture.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Lectures.Helper
{
    public interface ISudentLectureService
    {
        Task<IEnumerable<StudeingLectureViewModel>> GetLecturesForSubject(int SubjectID);
        Task<IEnumerable<StudeingLectureViewModel>> GetLecturesFree();
    }
}
