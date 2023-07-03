using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Students.Helper
{
    public interface IStudentAsync : IInsertAsync<AddedStudentViewModel>, IReadAsync<StudentViewModel>
    {
        Task<bool> Update(UpdatedStudnetViewModel studentviewModel);
        Task<StudentViewModel> GetAsync();
        Task<StudentViewModel> GetAsyncWithoutValidate(int studentID);
        Task<IPaginate<StudentViewModelWithoutImage>> filter(Expression<Func<Student, bool>> filter, int index = 0, int size = 10);

        Task<bool> StudentUpdatePhoneNumber();
        //int GetStudentID(string ID);

    }
}
