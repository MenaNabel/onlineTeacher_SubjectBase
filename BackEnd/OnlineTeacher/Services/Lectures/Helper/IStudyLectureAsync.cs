using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.ViewModel;
using OnlineTeacher.ViewModels.Lecture;
using OnlineTeacher.ViewModels.Lecture.Helper;
using System.Net;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Lectures.Helper
{
    public interface IStudyLectureAsync : IInsertAsync<AddStudyLectureViewModel>, IReadAsync<StudeingLectureViewModel>, IGet<Lecture>
    {
        Task<StudeingLectureViewModel> GetAsync(int ID);
        Task<HttpStatusCode> Update(AddStudyLectureViewModel instance);
        Task<bool> IsExsist(int ID);
        Task<bool> Delete(int ID);
        Task<FileResponse> GetFile(int LectureID);
    }
}