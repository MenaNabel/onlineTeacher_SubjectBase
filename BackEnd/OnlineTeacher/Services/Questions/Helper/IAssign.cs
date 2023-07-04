using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Questions.Helper
{
    public interface IAssign
    {
        Task<bool> Assign(int ExamID, int QuestionID);
    }
}
