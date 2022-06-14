using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Lectures.Helper;
using OnlineTeacher.Shared.Enums;
using OnlineTeacher.ViewModels.Lecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Threenine.Data;

namespace OnlineTeacher.Services.Lectures
{
    public class OnlineLectureServicesAsync : IOnlineLectureAsync
    {
        private readonly IRepositoryAsync<Lecture> _instances;
        private readonly IDeleteRepository<Lecture> _deleteRepository;
        private readonly IMapper _mapper;

        public OnlineLectureServicesAsync(IRepositoryAsync<Lecture> instances, IDeleteRepository<Lecture> deleteRepository, IMapper mapper)
        {
            _instances = instances;
            _mapper = mapper;
            _deleteRepository = deleteRepository;
        }
        public async Task<AddOnlineLectureViewModel> Add(AddOnlineLectureViewModel entity)
        {
            Lecture lecture = ConvertType(entity);
            
            var lectureViewModel = await _instances.InsertAsync(lecture);
            return lectureViewModel.Entity is null ? throw new Exception("Server Error Commit Subject") : _mapper.Map<AddOnlineLectureViewModel>(lectureViewModel.Entity);
        }

        public async Task<IEnumerable<AddOnlineLectureViewModel>> AddRange(IEnumerable<AddOnlineLectureViewModel> Collection)
        {
            IEnumerable<Lecture> lectures = Collection.Select(ConvertType);
            try
            {
                await _instances.InsertAsync(lectures);
            }
            catch (Exception ex)
            {
                throw new Exception("Server Error in addedd Range " + ex.Message);
            }
            return _instances.Commit() == true ? Collection : throw new Exception("Server Error Commit Range");
        }

        public async Task<bool> Delete(int ID)
        {
            var lecture = await Get(ID);
            if (lecture is null)
                return false;
            _deleteRepository.Delete(lecture);
            return _deleteRepository.commit();
        }

        public Task<IEnumerable<LectureViewModel>> Filter(Func<LectureViewModel, bool> FilterCondition)
        {
            throw new NotImplementedException();
        }

        public async Task<Lecture> Get(int ID)
        {
            var lecture = await _instances.SingleOrDefaultAsync(lec => lec.ID == ID);
           
            return lecture is not null ? lecture: null ;
        }
        public async Task<LectureViewModel> GetAsync(int ID)
        {
            var lecture = await _instances.SingleOrDefaultAsync(lec => lec.ID == ID && lec.Type == LectureType.online.ToString(),include:Lec=>Lec.Include(le=>le.Subject));
           
            return lecture is not null ? ConvertType_Lecture(lecture): null ;
        }

        public async Task<IEnumerable<LectureViewModel>> GetAll()
        {
            var lectures = await _instances.GetListAsync(lec=>lec.Type == LectureType.online.ToString(),include: Lec => Lec.Include(le => le.Subject));
            return lectures.Items.Select(ConvertType_Lecture);
        }

        public async Task<bool> IsExsist(int ID)
        {
            return await _instances.SingleOrDefaultAsync(lec => lec.ID == ID) is null ? false : true;
        }

        public async Task<HttpStatusCode> Update(AddOnlineLectureViewModel instance)
        {
            var lecture = await Get(instance.ID);

            if (lecture is null) return HttpStatusCode.NotFound;

            lecture.Name = instance.Name;
            lecture.Description = instance.Description;
            lecture.LectureLink = instance.LectureLink;
            lecture.SubjectID = instance.SubjectId;

            return _instances.Update(lecture) is true ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        }

        private Lecture ConvertType(AddOnlineLectureViewModel entity)
        {
            var lecture = _mapper.Map<Lecture>(entity);
            lecture.Type = LectureType.online.ToString();
            return lecture;
        }

        private LectureViewModel ConvertType_Lecture(Lecture viewModel)
        {
            var lecture = _mapper.Map<LectureViewModel>(viewModel);
            
            return lecture;
        }
    }
}