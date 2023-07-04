using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Exams.Helper;
using OnlineTeacher.Services.Questions.Helper;
using OnlineTeacher.Shared.Exceptions;
using OnlineTeacher.ViewModels.Questions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Threenine.Data;

namespace OnlineTeacher.Services.Questions
{
    public class QuestionServiceAsync : IQuestionAsync
    {
        private readonly IRepositoryAsync<Question> _Questions;
        private readonly IDeleteRepository<Question> _DeleteRepo;
        private readonly IMapper _Mapper;
        private readonly IAssign _Assign;
        private readonly IExamAsync _Exam;
        public QuestionServiceAsync(IRepositoryAsync<Question> Questions,IDeleteRepository<Question> DeleteRepo,IMapper Mapper, IAssign assign, IExamAsync Exam)
        {
            _Questions = Questions;
            _DeleteRepo = DeleteRepo;
            _Mapper = Mapper;
            _Assign = assign;
            _Exam = Exam;
        }
        public async Task<AddedQuestionViewModel> Add(AddedQuestionViewModel AddedQuestionViewModel)
        {
            bool IsExsist = await _Exam.IsExsist(AddedQuestionViewModel.ExamID);
            if (!IsExsist)
                throw new EntityNotFound("Exam Is not found");
            Question Question = ConvertToQuestion(AddedQuestionViewModel);
            var AddedQuestion = await _Questions.InsertAsync(Question);

            if (AddedQuestion.Entity is null)
                throw new Exception("Question Not Added");

            bool IsAssign = await _Assign.Assign(AddedQuestionViewModel.ExamID, AddedQuestion.Entity.ID);

          return IsAssign is false ? throw new Exception("Question Not Assign")
                 :ConvertToAddedQuestionViewModel(AddedQuestion.Entity);
        }

        public Task<IEnumerable<AddedQuestionViewModel>> AddRange(IEnumerable<AddedQuestionViewModel> Collection)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(int ID)
        {
            Question question = await SingleOrDefult(ID);
            if (question is null)
                return false;
            try
            {
                _DeleteRepo.Delete(question);
                return _DeleteRepo.commit();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<AddedQuestionViewModel> Get(int Id)
        {
            Question question = await SingleOrDefult(Id , include: Ques=>Ques.Include(Q=>Q.ExamQuestions).ThenInclude(EQ=>EQ.Exam));

            return question is null ? null : ConvertToAddedQuestionViewModel(question);
        }

        public async Task<IEnumerable<QuestionViewModel>> GetAll()
        {
            var Questions = await _Questions.GetListAsync();
            return Questions.Items.Select(ConvertToQuestionViewModel);
        }

        public async Task<HttpStatusCode> Update(AddedQuestionViewModel AddedQuestionViewModel)
        {

            var Question = await SingleOrDefult(AddedQuestionViewModel.ID);

            if (Question is null) return HttpStatusCode.NotFound;

            #region Update Data
            Question.Description = AddedQuestionViewModel.Description;
            Question.OneAnswer = AddedQuestionViewModel.OneAnswer;
            Question.SecondAnswer = AddedQuestionViewModel.SecondAnswer;
            Question.FourthAnswer = AddedQuestionViewModel.FourthAnswer;
            Question.CorrectAnswer = AddedQuestionViewModel.CorrectAnswer;
            if (AddedQuestionViewModel.FormFile != null)
            {
                if (AddedQuestionViewModel.FormFile.Length > 0
                /*&& _validExtensions.Contains(settingViewModel.FormFile.FileName.Substring(settingViewModel.FormFile.FileName.Length - 3,3))*/)
                {
                    using (var stream = new MemoryStream())
                    {
                        AddedQuestionViewModel.FormFile.CopyTo(stream);
                        Question.QuestionImage = stream.ToArray();
                        Question.File = AddedQuestionViewModel.FormFile.FileName;
                    }
                }
            }
            #endregion

            return _Questions.Update(Question) is true ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        }
        #region Helper
        private Question ConvertToQuestion(AddedQuestionViewModel AddedQuestionViewModel)
        {
            if (AddedQuestionViewModel is null)
                return null;
            Question question = new Question();
            if(AddedQuestionViewModel.FormFile != null)
            {
                if (AddedQuestionViewModel.FormFile.Length > 0
                /*&& _validExtensions.Contains(settingViewModel.FormFile.FileName.Substring(settingViewModel.FormFile.FileName.Length - 3,3))*/)
                {
                    using (var stream = new MemoryStream())
                    {
                        AddedQuestionViewModel.FormFile.CopyTo(stream);
                        question.QuestionImage = stream.ToArray();
                        question.File = AddedQuestionViewModel.FormFile.FileName;
                        AddedQuestionViewModel.File = question.File;
                    }
                }
            }
            
            return _Mapper.Map(AddedQuestionViewModel,question);
        }
      
        private async Task<Question> SingleOrDefult(int ID, Func<IQueryable<Question>, IIncludableQueryable<Question, object>> include = null)
        {
            return await _Questions.SingleOrDefaultAsync(Question => Question.ID == ID, include: include);
        }
        public QuestionViewModel ConvertToQuestionViewModel(Question Question)
        {
            if (Question is null)
                return null;
            try
            {
                return _Mapper.Map<QuestionViewModel>(Question);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public AddedQuestionViewModel ConvertToAddedQuestionViewModel(Question Question)
        {
            if (Question is null)
                return null;
            try
            {
                AddedQuestionViewModel addedQuestionViewModel = _Mapper.Map<AddedQuestionViewModel>(Question);
                if(Question.QuestionImage != null)
                {
                    var base64 = Convert.ToBase64String(Question.QuestionImage);
                    var imageSrc = string.Format("data:image/gif;base64," + base64);
                    addedQuestionViewModel.FileName = imageSrc;
                }
                var examQuestion = Question.ExamQuestions.FirstOrDefault();
                if (examQuestion?.Exam is null)
                    return addedQuestionViewModel;
                addedQuestionViewModel.ExamID = examQuestion.Exam.ID;
                addedQuestionViewModel.Exam = new ViewModels.Questions.Helper.ExamHelperViewModel(examQuestion.Exam.ID, examQuestion.Exam.Name);
               
                return addedQuestionViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
