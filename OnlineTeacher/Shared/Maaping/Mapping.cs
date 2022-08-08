using AutoMapper;
using OnlineTeacher.DataAccess.Context;

using OnlineTeacher.ViewModels.Reviews;
using OnlineTeacher.ViewModels.Exams;
using OnlineTeacher.ViewModels.Questions;

using OnlineTeacher.ViewModels.Lecture;
using OnlineTeacher.ViewModels.Subject;

using System.Threading.Tasks;
using OnlineTeacher.ViewModels.Levels;
using OnlineTeacher.ViewModels.Setting;
using OnlineTeacher.ViewModels.Students;
using OnlineTeacher.DataAccess.HelperConntext;
using OnlineTeacher.ViewModels.Subscribtions;
using OnlineTeacher.ViewModels.Lecture.Helper;
using OnlineTeacher.ViewModels.Exams.Helper;
using OnlineTeacher.ViewModels.Home;
using OnlineTeacher.ViewModels.Lecture.share;
using OnlineTeacher.DataAccess.Context.Bridge;

namespace OnlineTeacher.Shared.Maaping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            #region Subject

            CreateMap<Subject, AddingSubjectViewModel>().ReverseMap();
            CreateMap<Subject, SubjectViewModel>().ReverseMap();
            CreateMap<SubjectHelperViewModel, Subject>().ReverseMap();
            #endregion

            #region review
            CreateMap<Review, ReviewViewModel>().ReverseMap();
            CreateMap<Review, ReviewDetailsViewModel>().ReverseMap();
            CreateMap<Review, ReviewUpdatedViewModel>().ReverseMap();
            #endregion

            #region Exam
            CreateMap<Exam, AddedExamViewModel>().ReverseMap();
            CreateMap<Exam, ExamViewModelWithLecture>().ForMember(e => e.Lecture, le => le.MapFrom(l => l.Lecture)).ReverseMap();
            CreateMap<Exam, ExamViewModel>().ForMember(E => E.Questions, EV => EV.MapFrom(Q => Q.ExamQuestion)).ReverseMap();
            #endregion

            #region Question
            CreateMap<QuestionViewModel, ExamQuestion>().ForMember(EQ => EQ.Question, C => C.MapFrom(Q => Q)).ReverseMap();
            CreateMap<Question, AddedQuestionViewModel>().ReverseMap();
            CreateMap<Question, QuestionViewModel>().ReverseMap();
            #endregion

            #region Level
            CreateMap<Level, LevelViewModel>().ReverseMap(); 
            
            #endregion

            #region Setting
            CreateMap<Teacher, AddedSettingViewModel>().ReverseMap(); 
            #endregion

            #region Studnet
            CreateMap<Student, AddedStudentViewModel>().ReverseMap();
            CreateMap<Student, UpdatedStudnetViewModel>().ReverseMap();
            CreateMap<Student, StudentViewModel>().ForMember(st=>st.Level,st=>st.MapFrom(s=>s.Level)).ReverseMap();
            #endregion
            #region Subscribtions

            CreateMap<SubscribitionDetails, SubscriptionViewModel>().ReverseMap();
            CreateMap<Subscription, UpdateSubscribtionViewModel>().ReverseMap();
            CreateMap<Subscription, AddSubscibtionViewModel>().ReverseMap();


            #endregion

            #region Lecture

            CreateMap<Lecture, LectureViewModel>().ForMember(le => le.Subject, le => le.MapFrom(l => l.Subject)).ReverseMap();
            
            CreateMap<Lecture, AddStudyLectureViewModel>().ReverseMap();
            CreateMap<Lecture, AddOnlineLectureViewModel>().ReverseMap(); 
            CreateMap<Lecture, StudeingLectureViewModel>().ReverseMap();
            
            CreateMap<Lecture, LectureHelperViewModel>().ForMember(l => l.Type, l => l.MapFrom(s=>s.Type)).ReverseMap();
            #endregion
            CreateMap<SiteInfoViewModel, SiteInfo>().ReverseMap();
            CreateMap<HonerListItemViewModel, HonerList>().ReverseMap();
            CreateMap<Watching, ReOpenLectureDetailsViewModel>().ReverseMap();


        }


    }




}
