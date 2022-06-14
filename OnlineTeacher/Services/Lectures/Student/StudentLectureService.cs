using Microsoft.EntityFrameworkCore;
using OnlineTeacher.Services.Lectures.Helper;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Lecture;
using OnlineTeacher.ViewModels.Lecture.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Lectures
{

    public partial class StudyLectureServicesAsync : ISudentLectureService
    {
        private IUserServices _user;
        public async Task<IEnumerable<StudeingLectureViewModel>> GetLecturesForSubject(int SubjectID)
        {

            var Lectures = await _instances.GetListAsync(Lect => ((Lect.SubjectID == SubjectID) && Lect.IsAppear == true));
            if (Lectures.Items.Count() == 0)
                return null;
            if (Lectures?.Items?.FirstOrDefault().Subject?.Subscriptions?.Count() == 0)
                return null;
            
               
            return Lectures?.Items is not null ? Lectures.Items.Select(ConvertToStudeingLectureViewModel) : null;
        }
        public async Task<IEnumerable<StudeingLectureViewModel>> GetLecturesFree()
        {
            var Lectures = await _instances.GetListAsync(Lect => (Lect.IsAppear == true && Lect.IsFree == true), include: Le => Le.Include(Lec => Lec.Subject));
           
            return Lectures?.Items is not null ? Lectures.Items.Select(ConvertToStudeingLectureViewModel) : null;
        }
    }
}
