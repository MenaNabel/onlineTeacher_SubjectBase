using OnlineTeacher.DataAccess.Context.Bridge;
using OnlineTeacher.ViewModels.Lecture.share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Lectures.Helper
{
    public interface IWatcher
    {
        Task<Watching> GetWatching(int studentID, int LectureID);
        bool Update(Watching watching);
        Task<IPaginate<ReOpenLectureDetailsViewModel>> GetRequests(int index = 0, int size = 20);




    }
}
