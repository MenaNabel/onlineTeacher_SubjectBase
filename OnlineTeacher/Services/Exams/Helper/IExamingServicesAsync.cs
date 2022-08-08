using OnlineTeacher.ViewModels.Exams;
using OnlineTeacher.ViewModels.StudentExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Exams.Helper
{
   public  interface IExamingServicesAsync
    {
        Task<ExamStatusViewModel> GetExamForCurrentStudent(int Id);
        Task<ExamCorrectionViewModel> ExamCorrection(ExamCorrectionViewModel examCorrection);
        Task<ExamCorrectionViewModel> Re_ExamCorrection(ExamCorrectionViewModel examCorrection);
        Task<bool> ReOpenExam(ReOpenExamViewModel reOpenExamView);
        Task<IEnumerable<Grades>> GradesProgress(int studentID);
        Task<IEnumerable<LectureExamDetailViewModel>> GetExamsForCurrentStudent();


    }
}
