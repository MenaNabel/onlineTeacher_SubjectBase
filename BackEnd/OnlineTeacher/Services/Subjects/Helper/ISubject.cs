using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Subjects.Helper
{
   public interface ISubject : IRead<Subject>  , IInsert<AddingSubjectViewModel>
    {
       
    }
}
