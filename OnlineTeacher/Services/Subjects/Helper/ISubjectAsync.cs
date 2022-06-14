using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Subjects.Helper
{
    public interface ISubjectAsync : IReadAsync<SubjectViewModel>, IGet<Subject>
    {
        Task<IEnumerable<SubjectViewModel>> GetAllForCurrentStudent();
        Task<SubjectMangerResponse> Add(AddingSubjectViewModel Subject);
        Task<SubjectMangerResponse> Update(AddingSubjectViewModel subjectViewModel);

        Task<SubjectViewModel> GetAsync(int ID);


        Task<bool> Delete(int ID);
        Task<IEnumerable<SubjectViewModel>> Filter(Expression<Func<Subject, bool>> FilterCondition);
    }
}