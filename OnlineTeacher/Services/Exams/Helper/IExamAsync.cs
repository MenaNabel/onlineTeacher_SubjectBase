using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Exams.Helper
{
    public interface IExamAsync : IReadAsync<ExamViewModelWithLecture>, IInsertAsync<AddedExamViewModel>
    {
        Task<HttpStatusCode> Update(AddedExamViewModel Exam);
        Task<bool> Delete(int ID);
        Task<ExamViewModel> Get(int Id);
        Task<bool> IsExsist(int examID);
        Task<bool> AcceptReOpenExam(ReopenExamFeedback reopenExam);
        Task<IEnumerable<ExamViewModelWithLecture>> Filter(Expression<Func<Exam, bool>> FilterCondition);
        Task<IEnumerable<ReOpenExamDetailsViewModel>> GetReOpenExamRequests();
    }
}