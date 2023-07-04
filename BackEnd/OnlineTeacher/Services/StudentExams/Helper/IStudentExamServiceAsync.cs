
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.ViewModels.Exams;
using OnlineTeacher.ViewModels.StudentExam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.StudentExams.Helper
{
   public interface IStudentExamServiceAsync 
    {
        Task<StudentExam> AddAsync(StudentExam Student);
        Task<bool> ReExam(StudentExam StudentExam);
        Task<bool> Request_ReOpenExam(StudentExam StudentExam);
        Task<bool> AcceptRequest_ReExam(ReopenExamFeedback feedback);
        Task<IEnumerable<ReOpenExamDetailsViewModel>> GetReOpenExamRequests();
        Task<IEnumerable<Grades>> GradesProgrees(int studentID);
    }
}
