using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Questions.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data;

namespace OnlineTeacher.Services.Questions
{
    public class AssignServices : IAssign
    {
        private readonly IRepositoryAsync<ExamQuestion> _ExamQuestion;
        public AssignServices(IRepositoryAsync<ExamQuestion> ExamQuestion)
        {
            _ExamQuestion = ExamQuestion;
        }
        public async Task<bool> Assign(int ExamID, int QuestionID)
        {
       
           var examQuestion = await  _ExamQuestion.InsertAsync(new ExamQuestion() { ExamID = ExamID, QuestionID = QuestionID });

            return examQuestion.Entity is null ? false : true;
        }
    }
}
