using OnlineTeacher.DataAccess.Context.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Lectures.Helper
{
    public interface IWatcher
    {
        Task<Watching> GetWatching(int studentID, int LectureID);
        bool Update(Watching watching);

    }
}
