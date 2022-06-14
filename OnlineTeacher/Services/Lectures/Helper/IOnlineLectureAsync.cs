using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Lecture;
using System.Net;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Lectures.Helper
{
    public interface IOnlineLectureAsync : IInsertAsync<AddOnlineLectureViewModel>, IReadAsync<LectureViewModel>, IGet<Lecture>
    {
        Task<HttpStatusCode> Update(AddOnlineLectureViewModel instance);
        Task<bool> IsExsist(int ID);
        Task<bool> Delete(int ID);
        Task<LectureViewModel> GetAsync(int ID);
    }
}