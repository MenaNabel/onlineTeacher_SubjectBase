using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.DataAccess.Context.Bridge;
using OnlineTeacher.DataAccess.Repository.CustomeRepository.Lectures;
using OnlineTeacher.Services.Lectures.Helper;
using OnlineTeacher.Services.Subjects.Helper;
using OnlineTeacher.Shared.Enums;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.ViewModel;
using OnlineTeacher.ViewModels.Lecture;
using OnlineTeacher.ViewModels.Lecture.Helper;
using OnlineTeacher.ViewModels.Lecture.share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Threenine.Data;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Lectures.Refactoring
{
    public interface ILectureServices
    {
        Task<bool> Delete(int ID);
        Task<bool> IsExsist(int ID);
        Task<LectureResponseManger> Add(AddLectureViewModel entity, LectureType type);
        Task<LectureViewModel> GetAsync(int ID, LectureType type);
        IPaginate<LectureViewModel> GetAll(LectureType type, int index = 0, int Size = 20);
      //  Task<IEnumerable<LectureViewModel>> GetAll();
        Task<Lecture> Get(int ID);
        Task<LectureResponseManger> Update(AddLectureViewModel lec, LectureType type);
        Task<FileResponse> GetFile(int LectureID);
        Task<bool> ReOpenWatchingRequest(ReOpenLectureViewModel reOpenLecture);
        Task<IPaginate<ReOpenLectureDetailsViewModel>> GetReOpenLectureRequest(int index=0, int size=20);
        Task<bool> ConfirmReOpenWatching(ReOpenLectureDetailsViewModel reOpenLecture);
        Task<IEnumerable<LectureViewModel>> filter(Expression<Func<Lecture, bool>> filter);


    }
    
    public  class LectureServices : ILectureServices
    {
        private ILectureRepo _Lectures;
        private ISubjectAsync _Subject;
        private IDeleteRepository<Lecture> _DeleteLectures;
        private IMapper _mapper;
        private IFileImageUploading _fileImageUploading;
        private IWatcher _Watcher;
        private IUserServices _user;


        List<(LectureType Type, Func<AddLectureViewModel, Lecture> func)> _ConvertToLecture;
        public LectureServices(ILectureRepo Lectures, IDeleteRepository<Lecture> DeleteLectures, IMapper mapper, IFileImageUploading fileImageUploading, ISubjectAsync Subject, IWatcher watcher , IUserServices user)
        {

            _Lectures = Lectures;
            _DeleteLectures = DeleteLectures;
            _mapper = mapper;
            _fileImageUploading = fileImageUploading;
            _Subject = Subject;
            _Watcher = watcher;
            _user = user;
            SetDefaultConfiguration();
        }


        #region Helper
        private void SetDefaultConfiguration()
        {

            _ConvertToLecture = new List<(LectureType, Func<AddLectureViewModel, Lecture>)>
            {
                (LectureType.online,ConvertOnlineLectureToLecture),
                (LectureType.studying,ConvertStudingTectureToLecture)
            };

        }
        private Lecture ConvertOnlineLectureToLecture(AddLectureViewModel entity)
        {
            var OnlineLecture = (AddOnlineLectureViewModel)entity;
            var lecture = _mapper.Map<Lecture>(OnlineLecture);
            lecture.Type = LectureType.online.ToString();
            return lecture;
        }
        private Lecture ConvertStudingTectureToLecture(AddLectureViewModel entity)
        {
            var StudingLecture = (AddStudyLectureViewModel)entity;
            var lecture = _mapper.Map<Lecture>(StudingLecture);
            lecture.Type = LectureType.studying.ToString();
            return lecture;
        }

       
        private StudeingLectureViewModel ConvertLectureToStudingLecture(Lecture entity , int watchingCount =0)
        {

            var lecture = _mapper.Map<StudeingLectureViewModel>(entity);
            lecture.watchingCount = watchingCount;
            return lecture;
        }
        private bool UploadFile(Lecture lecture, IFileImage fileImage)
        {

            string name = null;
            byte[] data = null;

            if (_fileImageUploading.UploadFile(fileImage, out name, out data) && !string.IsNullOrEmpty(name))
            {
                lecture.FileName = name;
                if (lecture.File == null) lecture.File = new LectureDetails();
                lecture.File.FileData = data;
                return true;

            }
            return false;
        }

        #endregion
      

        public async Task<bool> Delete(int ID){
            // get lecture 
            var CurrentLecture = await _Lectures.SingleOrDefaultAsync(Lect => Lect.ID == ID, include: Lec => Lec.Include(CurrentLec => CurrentLec.previousLecture));
             // if not lecture not found return false
            if (CurrentLecture is null)
                return false ;

            // if lecture contain a lecture that depends on viewing
            if (CurrentLecture.previousLecture is not null)
            {
                
                // get lecture that  depend on current deleted lecture
               var LectureDependOnCurrentLecture  =   await _Lectures.
                    SingleOrDefaultAsync(lectures => lectures.LectureID == CurrentLecture.ID);
                // if found lecture 
                if (LectureDependOnCurrentLecture is not null)
                {
                    LectureDependOnCurrentLecture.LectureID = CurrentLecture.LectureID; 
                    CurrentLecture.LectureID = null;

                }
                _Lectures.Update(LectureDependOnCurrentLecture);
            }
            else
            {
                var LectureDependOnCurrentLecture = await _Lectures.
                      SingleOrDefaultAsync(lectures => lectures.LectureID == CurrentLecture.ID);
                // if found lecture 
                if (LectureDependOnCurrentLecture is not null)
                {
                    LectureDependOnCurrentLecture.LectureID = null;
                    

                }
                _Lectures.Update(LectureDependOnCurrentLecture);

            }
           _DeleteLectures.Delete(CurrentLecture);

           return _DeleteLectures.commit();
        
        }

        public async Task<bool> IsExsist(int ID)
        {
          var Lecture = await _Lectures.SingleOrDefaultAsync(lec => lec.ID == ID);
            if (Lecture is null)
                return false;
            return true;

        }
        public async Task<LectureResponseManger> Add(AddLectureViewModel entity, LectureType type)
        {
            Lecture lecture = _ConvertToLecture.Where(e => e.Type == type).Select((l) => l.func(entity)).FirstOrDefault();
            if (type == LectureType.studying)
            {
                if (UploadFile(lecture, (IFileImage)entity))
                {
                    var lecturAddedd = await _Lectures.InsertAsync(lecture);

                    return lecturAddedd.Entity is not null ?
                     new LectureResponseManger
                     {
                         IsSuccess = true,
                         Message = "Lecture Addedd Successfuly"
                     } :
                     new LectureResponseManger
                     {
                         IsSuccess = false,
                         Message = " Not Addedd succcessfuly"
                     };
                }
            }
                var lectureViewModel = await _Lectures.InsertAsync(lecture);

                return lectureViewModel.Entity is not null ?
                  new LectureResponseManger
                  {
                      IsSuccess = true,
                      Message = "Lecture Addedd Successfuly",
                      Errors = new string[] { "File not Addedd successfuly" }
                  } :
                  new LectureResponseManger
                  {
                      IsSuccess = false,
                      Message = " Not updated succcessfuly"
                  };
            
        }
        public async Task<LectureViewModel> GetAsync(int ID ,LectureType type)
        {
            
        
            var lecture = await _Lectures.SingleOrDefaultWithoutFile(lec => lec.ID == ID && lec.Type == type.ToString(), include: Lec => Lec.Include(le => le.Subject));
           int studentId=  _user.GetStudentID();
         var watching=  await  _Watcher.GetWatching(studentId, ID);
            if(watching is null)
            {
                return DetectType(lecture, type);
            }
            return DetectType(lecture, type , watching.WatchingCount);

        }
        public async Task<IEnumerable<LectureViewModel>> filter(Expression<Func<Lecture, bool>> filter) 
        {
        var Lectures = await  _Lectures.GetListAsync(filter);
            return Lectures.Items.Select(ConvertToLectureViewModel);

        }
        private LectureViewModel DetectType(Lecture lecture, LectureType type, int watchingCount = 0)
        {
            if (lecture is null)
                return null;


            if (type == LectureType.studying)
            {
                return  ConvertLectureToStudingLecture(lecture, watchingCount);
            }
            else
            {
                return  ConvertToLectureViewModel(lecture);
            }
        }
        public  IPaginate<LectureViewModel> GetAll(LectureType type ,int index = 0 ,int Size = 20)
        {
            

            var lectures =  _Lectures.GetLecturesWithoutFiles(lec => lec.Type == type.ToString(), include: Lec => Lec.Include(le => le.Subject).Include(le=>le.previousLecture) , index:index , size:Size);

            //var Lectures=   lectures.Items.Select(l => DetectType(l,type));

            return new Paginate<Lecture, LectureViewModel>(lectures, l => l.Select(le => DetectType(le, type))); ;
        }
        //public async IEnumerable<LectureViewModel> GetAll()
        //{
        //    // var lectures = await _Lectures.GetLecturesWithoutFiles(include: Lec => Lec.Include(le => le.Subject));

        //  //  return lectures.Select(ConvertToLectureViewModel);
        //}
        private LectureViewModel ConvertToLectureViewModel(Lecture viewModel)
        {
            var lecture = _mapper.Map<LectureViewModel>(viewModel);

            return lecture;
        }
        public async Task<Lecture> Get(int ID)
        {
            var lecture = await _Lectures.SingleOrDefaultWithoutFile(lec => lec.ID == ID ,include: l=>l.Include(le=>le.previousLecture));
            if (lecture == null)
                 return null;
            return lecture ;
        }
        #region Updates
        public async Task<LectureResponseManger> Update(AddLectureViewModel lec , LectureType type)
        {
            if(type == LectureType.online)
            {
                return await Update((AddOnlineLectureViewModel)lec);
            }
            else
            {
                return await Update((AddStudyLectureViewModel)lec);
            }
        }
        private async Task<LectureResponseManger> Update(AddOnlineLectureViewModel instance)
        {
            var lecture = await Get(instance.ID);

            if (lecture is null) return new LectureResponseManger() { IsSuccess = false, Message = "Not Updated", Errors = new[] { "Not Found" } };

            lecture.Name = instance.Name;
            lecture.Description = instance.Description;
            lecture.LectureLink = instance.LectureLink;
            lecture.SubjectID = instance.SubjectId;
            lecture.DateTime = instance.DateTime;
            lecture.LectureID = instance.LectureID;
            lecture.Subject = await _Subject.Get(instance.SubjectId);



            return _Lectures.Update(lecture) is true ? new LectureResponseManger()
            {
                IsSuccess = true,
                Message = "lecture updated successfuly"
            } :
            new LectureResponseManger
            {
                IsSuccess = false
            };

        }
        private async Task<LectureResponseManger> Update(AddStudyLectureViewModel lectureViewModel)
        {
            var lecture = await _Lectures.SingleOrDefaultAsync(l=>l.ID == lectureViewModel.ID);

            if (lecture is null) return new LectureResponseManger()
            {
                IsSuccess = false,
                Message = "Not Updated",
                Errors = new[] { "Not Found" }
            };

            if (UploadFile(lecture, lectureViewModel))
            {
                UpdatingStudingLectureProcess(lectureViewModel, lecture);
               
                return _Lectures.Update(lecture) is true ? new LectureResponseManger
                {
                    IsSuccess = true,
                    Message = "updated successfully"

                } : new LectureResponseManger
                {
                    IsSuccess = false,
                    Message = "Not updated successfully"
                };
            }
            else
            {
                UpdatingStudingLectureProcess(lectureViewModel , lecture);

                return _Lectures.Update(lecture) is true ? new LectureResponseManger
                {
                    IsSuccess = true,
                    Message = "updated successfully but file not updated successfuly"

                } : new LectureResponseManger
                {
                    IsSuccess = false,
                    Message = "Not updated successfully"
                };
            }

        }
        #endregion
        private void UpdatingStudingLectureProcess(AddStudyLectureViewModel LectureViewModel, Lecture lecture) 
        {
            
            lecture.Name = LectureViewModel.Name;
            lecture.Description = LectureViewModel.Description;
            lecture.IsAppear = LectureViewModel.IsAppear;
            lecture.IsFree = LectureViewModel.IsFree;
            lecture.LectureLink = LectureViewModel.LectureLink;
            lecture.Month = LectureViewModel.Month;
            lecture.SubjectID = LectureViewModel.SubjectId;
            lecture.LectureID = LectureViewModel.LectureID;
           
        
        }
        public async Task<FileResponse> GetFile(int LectureID)
        {
            var lecture = await _Lectures.SingleOrDefaultAsync(lec => lec.ID == LectureID , include: Lec=>Lec.Include(l=>l.File) );
            return lecture is null ? null : ConvertToFileResonse(lecture);
        }

        private FileResponse ConvertToFileResonse(Lecture lecture)
        {
           var Response =  new FileResponse { Name = lecture.FileName};
            if(lecture.File is not null)
            {
                var base64 = Convert.ToBase64String(lecture.File.FileData);
                string fileSrc = string.Format("data:application/pdf;base64," + base64);
                Response.FileData = fileSrc; 
            }
            return Response; 
        }
        /// <summary>
        /// student Request Ro open Lecture To Wtching 
        /// </summary>
        /// <param name="reOpenLecture">
        ///     Lecture ID 
        ///     Current Student ID request 
        ///     Reason to re open watching
        /// </param>
        /// <returns></returns>
        public async Task<bool> ReOpenWatchingRequest(ReOpenLectureViewModel reOpenLecture)
        {
            var studentID = _user.GetStudentID();
            if (studentID == 0)
                return false;
            var Watcher = await _Watcher.GetWatching(studentID, reOpenLecture.LectureID);
            if (Watcher == null)
                return false;

            Watcher.ReOpenRequest(reOpenLecture.Reason);
            return _Watcher.Update(Watcher);

        }
        public async Task<IPaginate<ReOpenLectureDetailsViewModel>> GetReOpenLectureRequest(int index = 0 , int size =20)
        {
           return await _Watcher.GetRequests(index , size);   
        }
            /// <summary>
            /// Teacher Confirm To open Re Watching 
            /// </summary>
            /// <param name="reOpenLecture"></param>
            /// <returns></returns>
        public async Task<bool> ConfirmReOpenWatching(ReOpenLectureDetailsViewModel reOpenLecture)
        {
            if (reOpenLecture is null)
                return false;
            var Watcher = await _Watcher.GetWatching(reOpenLecture.StudentID, reOpenLecture.LectureID);
            if (Watcher == null)
                return false;
            if (!reOpenLecture.IsConfirmed)
                return false;


            Watcher.ConfirmReOpenRequest();
           return _Watcher.Update(Watcher);
            

          
        }
    }

    public interface ILectureServicesForStudent
    {
        Task<IEnumerable<StudeingLectureViewModel>> GetLecturesbyMonth(int subjectID, int month);
        Task<StudeingLectureViewModel> GetStudingLecture(int ID);
        Task<StudeingLectureViewModel> GetFreeStudingLecture(int ID);
         
    }
    public class LectureServicesForStudent : ILectureServicesForStudent
    {
        private ILectureRepo _Lectures;
        private IRepositoryAsync<Exam> _Exams;
        private IUserServices _user;
        private IMapper _Mapper;
        private IWatcher _Watcher;
        public LectureServicesForStudent(ILectureRepo Lectures , IRepositoryAsync<Exam> Exam , IUserServices user,IMapper mapper , IWatcher watcher)
        {
            _Lectures = Lectures;
            _Exams = Exam;
            _user = user;
            _Mapper = mapper;
            _Watcher = watcher;
        }

        public async Task<StudeingLectureViewModel> GetStudingLecture(int ID)
        {
           Lecture lec = await GetLecture(ID); // get lecture
           if(lec is null) // not found lecture 
             return null;
            return await validateUserAuthorization(lec);

        }
        public async Task<StudeingLectureViewModel> GetFreeStudingLecture(int ID)
        {
            Lecture lec = await GetFreeLecture(ID); // get lecture
            if (lec is null) // not found lecture 
                return null;
            return await validateUserAuthorization1(lec);

        }

        private async Task<Lecture> GetLecture(int id)
        {
            
            int StudentID = _user.GetStudentID();
            Lecture lecture = await _Lectures.SingleOrDefaultWithoutFile(L => L.ID == id, include: l => l.Include(l => l.previousLecture).Include(s=>s.Subject).ThenInclude(Sub=>Sub.Subscriptions.Where(sc=>sc.StudentID == StudentID)));
            return lecture;
        }

        private async Task<Lecture> GetFreeLecture(int id)
        {
            Lecture lecture = await _Lectures.SingleOrDefaultWithoutFile(L => L.ID == id, include: l => l.Include(l => l.previousLecture).Include(s => s.Subject));
            return lecture;
        }
        private async Task<StudeingLectureViewModel> validateUserAuthorization(Lecture lec)
        {

            if (lec is null)
                return null;
            if (!CheckSubscription(lec))
                return null;
            
            int studentID = _user.GetStudentID(); // get student ID

            var CurrentLectureExam = await _Exams.SingleOrDefaultAsync(e => e.LectureID == lec.ID);

            if (lec.previousLecture is null) // not found previous Lecture then can show lecture
                return await checkAvilabilityWatching_And_AddExamIfNotFound(lec, CurrentLectureExam, studentID);

             // Get Exam For previous Lecture 
             var exam = await _Exams.SingleOrDefaultAsync(e => e.LectureID == lec.previousLecture.ID, include: e => e.Include(e => e.StudentExams.Where(E => E.StudentID == studentID)));
            // check not found exams for privuios lecture or passing Exam 
            if (exam is null || checkPassingExam(exam)) 
                return await checkAvilabilityWatching_And_AddExamIfNotFound(lec, CurrentLectureExam, studentID);
            
            return null; //student not pass exam

        }
    
        private async Task<StudeingLectureViewModel> validateUserAuthorization1(Lecture lec)
        {

            var CurrentLectureExam = await _Exams.SingleOrDefaultAsync(e => e.LectureID == lec.ID);

            if (lec.previousLecture is null) // not found previous Lecture then can show lecture
                if (CurrentLectureExam is null)
                    return ConvertToStudingLectureViewModel(lec);
                else return AddExamID(ConvertToStudingLectureViewModel(lec), CurrentLectureExam.ID);


            // Get Exam For previous Lecture 
            var exam = await _Exams.SingleOrDefaultAsync(e => e.LectureID == lec.previousLecture.LectureID);

            if (exam is null) // not found exams for privuios lecture
                if (CurrentLectureExam is null)
                    return ConvertToStudingLectureViewModel(lec);
                else
                    return AddExamID(ConvertToStudingLectureViewModel(lec), CurrentLectureExam.ID);
            if (exam.StudentExams is null || exam.StudentExams.Count() == 0) // Student not test exam
                return null;
            var StudentExam = exam.StudentExams.FirstOrDefault();
            if (StudentExam is null) // student not test exam
                return null;
            if (StudentExam.IsPassed == true) // student pass exam 
                if (CurrentLectureExam is null)
                    return ConvertToStudingLectureViewModel(lec);
                else
                    return AddExamID(ConvertToStudingLectureViewModel(lec), CurrentLectureExam.ID);

            return null; //student not pass exam

        }
        private async Task<bool> CheckWatchingAvailiableTimes(int studentID , int LectureID)
        {
            var wacher = await _Watcher.GetWatching(studentID, LectureID);
            if (wacher is null)
                return false;
            else if (wacher.Watch())
                if (_Watcher.Update(wacher))
                    return true;
               
            return false;

        }
        private StudeingLectureViewModel AddExamID(StudeingLectureViewModel LectureViewModel , int ExamID)
        {
            LectureViewModel.ExamID = ExamID;
            return LectureViewModel;
        }
        private StudeingLectureViewModel ConvertToStudingLectureViewModel(Lecture lecture)
        {
            return _Mapper.Map<StudeingLectureViewModel>(lecture);
        }

        public async Task<IEnumerable<StudeingLectureViewModel>> GetLecturesbyMonth(int subjectID, int month)
        {
            int studentID = _user.GetStudentID();
            var Lectures = await _Lectures.GetListAsync(Le => Le.SubjectID == subjectID && Le.Month == month , include: l=>l.Include(le=>le.Subject).ThenInclude(lec=>lec.Subscriptions.Where(s=>s.StudentID == studentID)));

          var validateLect =   Lectures.Count > 0 ? validateUserAuthorization(Lectures.Items.FirstOrDefault()) : null;
            if (validateLect is null)
                return null;
            return Lectures.Items.Select(ConvertToStudingLectureViewModel);

        }

        private bool checkPassingExam(Exam PreviousLectureExam)
        {
            if (PreviousLectureExam.StudentExams is null || PreviousLectureExam.StudentExams.Count() == 0) // Student not test exam
                return false;
            var StudentExam = PreviousLectureExam.StudentExams.FirstOrDefault();
            if (StudentExam is null) // student not test exam
                return false;
            return StudentExam.IsPassed;

        }
        private async Task<StudeingLectureViewModel> checkAvilabilityWatching_And_AddExamIfNotFound(Lecture lecture, Exam CurrentLectureExam, int StudentID)
        {
            if (await CheckWatchingAvailiableTimes(StudentID, lecture.ID))
                if (CurrentLectureExam is null)
                    return ConvertToStudingLectureViewModel(lecture);
                else
                    return AddExamID(ConvertToStudingLectureViewModel(lecture), CurrentLectureExam.ID);

            return null; //not allaw to watch lecture
        }
        private bool CheckSubscription(Lecture lecture)
        {
            if (lecture.Subject.Subscriptions is null || lecture.Subject.Subscriptions.Count() == 0 || (!lecture.Subject.Subscriptions.FirstOrDefault().IsActive))
                return false;
            return true;
        }
    }
}
