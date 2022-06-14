using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Subjects.Helper;
using OnlineTeacher.Shared.Maaping;
using OnlineTeacher.ViewModels.Subject;
using Threenine.Data;

namespace OnlineTeacher.Services.Subjects
{
    public class SubjectServices : ISubject
    {
        private readonly IRepository<Subject> _Subjects;
        private readonly IMapper _Mapper;

        public SubjectServices(IRepository<Subject> Subjects , IMapper mapper)
        {
            _Subjects = Subjects;
            _Mapper = mapper;
        }
        public AddingSubjectViewModel Add(AddingSubjectViewModel addingSubjectViewModel)
        {
          Subject subject =   _Mapper.Map<Subject>(addingSubjectViewModel);
          
           var AddedSubject = _Mapper.Map<AddingSubjectViewModel>(_Subjects.Insert(subject));

            return AddedSubject is null ? throw new Exception("Not Added Server Error") : AddedSubject;
          
            
        }

        public IEnumerable<AddingSubjectViewModel> AddRange(IEnumerable<AddingSubjectViewModel> Collection)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Subject> Filter(Func<Subject, bool> FilterCondition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Subject> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
