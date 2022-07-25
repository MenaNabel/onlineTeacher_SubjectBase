using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Exams.Helper;
using OnlineTeacher.Services.StudentExams.Helper;
using OnlineTeacher.Shared.Enums;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Exams;
using OnlineTeacher.ViewModels.Questions;
using Threenine.Data;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Exams
{
    public partial class ExamServicesAsync : IExamAsync
    {
        private readonly IRepositoryAsync<Exam> _Exams;
        private readonly IDeleteRepository<Exam> _DeleteRepo;
        private readonly IDeleteRepository<Question> _DeleteQues;
        private readonly IMapper _Mapper;
        private readonly IUserServices _User;
        private readonly IStudentExamServiceAsync _studentExam;
        public ExamServicesAsync(
            IRepositoryAsync<Exam> Exams,
            IMapper mapper,
            IDeleteRepository<Exam> deleteExam,  IDeleteRepository<Question> DeleteQues,
            IUserServices user,
            IStudentExamServiceAsync studentExam
            )
        {
            _Exams = Exams;
            _DeleteRepo = deleteExam;
            _DeleteQues = DeleteQues;
            _Mapper = mapper;
            _User = user;
            _studentExam = studentExam;
        }
        public async Task<AddedExamViewModel> Add(AddedExamViewModel AddedExamViewModel)
        {
            Exam Exam = ConvertToExam(AddedExamViewModel);
            try
            {
                Exam.EaxamType = AddedExamViewModel.EaxamType.ToString();
                var AddedExam = await _Exams.InsertAsync(Exam);
                return AddedExam.Entity is null ? null : ConvertToAddedExamViewModel(AddedExam.Entity);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<IEnumerable<AddedExamViewModel>> AddRange(IEnumerable<AddedExamViewModel> Collection)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int ID)
        {
            Exam Exam = await SingleOrDefult(ID , include: ex=>ex.Include(e=>e.ExamQuestion.Where(e=>e.ExamID == ID)).ThenInclude(e=>e.Question));
            if (Exam is null)
                return false;
            try

            {
                _DeleteRepo.Delete(Exam);
                if (Exam.ExamQuestion != null && Exam.ExamQuestion.Select(e => e.Question) != null) {
                    _DeleteQues.Delete(Exam.ExamQuestion.Select(e => e.Question));
                    
                  }
                return _DeleteRepo.commit();
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<bool> IsExsist(int examID)
        {
            var Exam = await _Exams.SingleOrDefaultAsync(ex => ex.ID == examID);
            return Exam is null ? false : true;
        }
        public async Task<ExamViewModel> Get(int Id)
        {

            Exam Exam = await SingleOrDefult(Id, EX => EX.Include(EQ => EQ.ExamQuestion).ThenInclude(Q => Q.Question).Include(ex=>ex.Lecture));

            return Exam is null ? null : ConvertToExamViewModel(Exam);

        }


        public async Task<IPaginate<ExamViewModelWithLecture>> GetAll(int index , int size)
        {
            var Exams = await _Exams.GetListAsync(include: Ex => Ex.Include(e => e.Lecture), orderBy: Ex => Ex.OrderByDescending(ex => ex.DateAndExamminationExpireTime) , index:index,size:size);
            //return Exams.Items.Select(ConvertToExamViewModelWithLecture);
            return new Paginate<Exam, ExamViewModelWithLecture>(Exams , l => l.Select(le => ConvertToExamViewModelWithLecture(le)));
        }

       

        public async Task<HttpStatusCode> Update(AddedExamViewModel ExamVieWModel)
        {

            var Exam = await SingleOrDefult(ExamVieWModel.ID);

            if (Exam is null) return HttpStatusCode.NotFound;

            #region Update Data
            Exam.Name = ExamVieWModel.Name;
            Exam.Degree = ExamVieWModel.Degree;
            Exam.ExamDuration = ExamVieWModel.Duration;
            Exam.LectureID = ExamVieWModel.LectureID;
            Exam.DateAndExamminationExpireTime =ExamVieWModel.DateAndExamminationExpireTime;
            Exam.EaxamType = (ExamVieWModel.EaxamType).ToString();
            #endregion

            return _Exams.Update(Exam) is true ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        }

        public async Task<bool> AcceptReOpenExam(ReopenExamFeedback feedback)
        {
            return await _studentExam.AcceptRequest_ReExam(feedback);
        }
        public async Task<IEnumerable<ReOpenExamDetailsViewModel>> GetReOpenExamRequests()
        {
            return await _studentExam.GetReOpenExamRequests();

        }
        #region Helper  
        public AddedExamViewModel ConvertToAddedExamViewModel(Exam Exam)
        {
            if (Exam is null)
                return null;
            return _Mapper.Map<AddedExamViewModel>(Exam);
        }
        //public ExamViewModel ConvertToExamViewModel(Exam Exam)
        public ExamStatusViewModel ConvertToExamViewModel(Exam Exam)
        {
            if (Exam is null)
                return null;
            try
            {
                ExamStatusViewModel examViewModel = new ExamStatusViewModel();
                //var examQuestion = Exam.ExamQuestion.Select(x => x.ExamID == Exam.ID).ToList();
                //for(int i = 0; i < Exam.ExamQuestion.Count(); i++)
                //{
                //    examViewModel.Questions. Exam.ExamQuestion
                //}
                _Mapper.Map(Exam, examViewModel);
                examViewModel.IsSubmited = Exam.StudentExams.Count > 0 ? Exam.StudentExams.FirstOrDefault(se => se.ExamID == Exam.ID).IsSubmitted : false ;
                examViewModel.Duration = Exam.ExamDuration;
                foreach(QuestionViewModel ques in examViewModel.Questions)
                {
                    var x = Exam.ExamQuestion; // all questions avaiable
                    var y = x.FirstOrDefault(x => x.Question.ID == ques.ID);
                        if(y.Question.QuestionImage != null)
                    {
                        var base64 = Convert.ToBase64String(y.Question.QuestionImage);
                        var imageSrc = string.Format("data:image/gif;base64," + base64);
                        ques.FileName = imageSrc;
                    }
                    
                }
                return examViewModel;
                //return _Mapper.Map<ExamViewModel>(Exam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
        public ExamViewModelWithLecture ConvertToExamViewModelWithLecture(Exam Exam)
        {
            if (Exam is null)
                return null;
            try
            {
                return _Mapper.Map<ExamViewModelWithLecture>(Exam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
       
        private Exam ConvertToExam(AddedExamViewModel ExamViewModel)
        {
            Exam examViewModel = new Exam();
            examViewModel.ExamDuration = ExamViewModel.Duration;
            if (ExamViewModel is null)
                return null;
            return _Mapper.Map(ExamViewModel, examViewModel);
        }
        private Exam ConvertToExam(ExamViewModel ExamViewModel)
        {
            if (ExamViewModel is null)
                return null;
            return _Mapper.Map<Exam>(ExamViewModel);
        }
        private async Task<Exam> SingleOrDefult(int ID, Func<IQueryable<Exam>, IIncludableQueryable<Exam, object>> include = null)
        {
            return await _Exams.SingleOrDefaultAsync(Exam => Exam.ID == ID, include: include);
        }

        public async Task<IEnumerable<ExamViewModelWithLecture>> Filter(Expression<Func<Exam, bool>> FilterCondition)
        {
            var Exams = await _Exams.GetListAsync(FilterCondition, include: Ex => Ex.Include(e => e.Lecture));
            return Exams.Items.Select(ConvertToExamViewModelWithLecture);

        }

        #endregion
    }

}