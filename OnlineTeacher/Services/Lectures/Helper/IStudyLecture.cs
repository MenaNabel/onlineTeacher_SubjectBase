using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Lecture;

namespace OnlineTeacher.Services.Lectures.Helper
{
    public interface IStudyLecture : IRead<Lecture>, IInsert<AddStudyLectureViewModel>
    {
    }
}