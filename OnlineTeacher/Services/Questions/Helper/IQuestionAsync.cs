using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Questions.Helper
{
    public interface IQuestionAsync : IInsertAsync<AddedQuestionViewModel>,IReadAsync<QuestionViewModel>
    {
        Task<HttpStatusCode> Update(AddedQuestionViewModel AddedQuestionViewModel);
        Task<bool> Delete(int ID);
        Task<AddedQuestionViewModel> Get(int Id);
    }
}
