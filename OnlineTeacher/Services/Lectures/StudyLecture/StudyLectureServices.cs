using AutoMapper;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Lectures.Helper;
using OnlineTeacher.ViewModels.Lecture;
using System;
using System.Collections.Generic;
using Threenine.Data;

namespace OnlineTeacher.Services.Lectures
{
    public class StudyLectureServices: IStudyLecture
    {
        private readonly IRepository<Lecture> _instances;
        private readonly IMapper _mapper;

        public StudyLectureServices(IRepository<Lecture> instances, IMapper mapper)
        {
            _instances = instances;
            _mapper = mapper;
        }

        public AddStudyLectureViewModel Add(AddStudyLectureViewModel entity)
        {
            Lecture lecture = _mapper.Map<Lecture>(entity);
            var addLecture = _mapper.Map<AddStudyLectureViewModel>(_instances.Insert(lecture));
            return addLecture is null ? throw new Exception("Not Added Server Error") : addLecture;
        }

        public IEnumerable<AddStudyLectureViewModel> AddRange(IEnumerable<AddStudyLectureViewModel> Collection)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lecture> Filter(Func<Lecture, bool> FilterCondition)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Lecture> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}