using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess.Context.Bridge;
using OnlineTeacher.Services.Lectures.Helper;
using OnlineTeacher.ViewModels.Lecture.share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Lectures.Refactoring
{
    public class WatcherService : IWatcher
    {
        private IRepositoryAsync<Watching> _watchRepo;
        private IMapper _mapper;
        public WatcherService(IRepositoryAsync<Watching> watchRepo, IMapper mapper)
        {
            _watchRepo = watchRepo;
            _mapper = mapper;
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

        public async Task<IPaginate<ReOpenLectureDetailsViewModel>> GetRequests(int index =0 , int size=20)
        {
          var watchersRequest = await  _watchRepo.GetListAsync(w => w.HaveRequest == true, include:wa=>wa.Include(w=>w.Student).Include(w=>w.Lecture) , index: index, size: size   );
            return new Paginate<Watching, ReOpenLectureDetailsViewModel>(watchersRequest, w => w.Select(wa => ConvertToReOpenLectureDetailsViewModel(wa))) ;

        }

        private ReOpenLectureDetailsViewModel ConvertToReOpenLectureDetailsViewModel(Watching watching)
        {
         var watch =  _mapper.Map<Watching, ReOpenLectureDetailsViewModel>(watching);
            if (watching.Student is not null)
                watch.StudentName = watching.Student.Name;
            if (watching.Lecture is not null)
                watch.LectureName = watching.Lecture.Name;
            return watch;
        }
    }
}
