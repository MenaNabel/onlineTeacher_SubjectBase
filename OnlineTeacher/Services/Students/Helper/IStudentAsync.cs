using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Students.Helper
{
    public interface IStudentAsync : IInsertAsync<AddedStudentViewModel> , IReadAsync<StudentViewModel>
    {
        Task<bool> Update(UpdatedStudnetViewModel studentviewModel);
        Task<StudentViewModel> GetAsync();
        Task<StudentViewModel> GetAsyncWithoutValidate(string UserID);
    }
}
