using OnlineTeacher.DataAccess.Context.Bridge;
using OnlineTeacher.Services.Lectures.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data;

namespace OnlineTeacher.Services.Lectures.Refactoring
{
    public class WatcherService : IWatcher
    {
        private IRepositoryAsync<Watching> _watchRepo;
        public WatcherService(IRepositoryAsync<Watching> watchRepo)
        {
            _watchRepo = watchRepo;
        }
        public async Task<Watching> GetWatching(int studentID, int LectureID)
        {
          var Watch =  await _watchRepo.SingleOrDefaultAsync(wa => wa.LectureID == LectureID && wa.StudentID == studentID);
            if (Watch is null)
                if (await insertWatching(studentID, LectureID))
                    Watch = await _watchRepo.SingleOrDefaultAsync(wa => wa.LectureID == LectureID && wa.StudentID == studentID);
               
            return Watch;
        }

        private async Task<bool> insertWatching(int studentID, int lectureID)
        {
            try
            {
                var insertingWatch = await _watchRepo.InsertAsync(new Watching { LectureID = lectureID, StudentID = studentID, WatchingCount = 0 });
                return insertingWatch is null ? false : true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public bool Update(Watching watching)
        {
            if (_watchRepo.Update(watching))
                return true;
            return false;
        }
    }
}
