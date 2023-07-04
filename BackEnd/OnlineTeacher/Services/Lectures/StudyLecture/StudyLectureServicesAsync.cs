using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Lectures.Helper;
using OnlineTeacher.Shared.Enums;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Services;
using OnlineTeacher.Shared.ViewModel;
using OnlineTeacher.ViewModels.Lecture;
using OnlineTeacher.ViewModels.Lecture.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Threenine.Data;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Lectures
{
    public partial class StudyLectureServicesAsync : IStudyLectureAsync
    {
     
       
        private readonly IRepositoryAsync<Lecture> _instances;
        private readonly IDeleteRepository<Lecture> _deleteRepository;
        private readonly IMapper _mapper;
        private readonly IFileImageUploading _fileImageUploading;

        public StudyLectureServicesAsync(IRepositoryAsync<Lecture> instances, IDeleteRepository<Lecture> deleteRepository, IMapper mapper, IUserServices user,
            IFileImageUploading fileImageUploading)
        {
            _instances = instances;
            _mapper = mapper;
            _deleteRepository = deleteRepository;
            _user = user;
            _fileImageUploading = fileImageUploading;
        }
        public async Task<AddStudyLectureViewModel> Add(AddStudyLectureViewModel entity)
        {
            Lecture lecture = ConvertType(entity);
            UploadFile(lecture, entity);
            var lectureViewModel = await _instances.InsertAsync(lecture);
            
            return lectureViewModel.Entity is null ? throw new Exception("Server Error Commit Subject") : _mapper.Map<AddStudyLectureViewModel>(lectureViewModel.Entity);
        }

        public async Task<IEnumerable<AddStudyLectureViewModel>> AddRange(IEnumerable<AddStudyLectureViewModel> Collection)
        {
            IEnumerable<Lecture> lectures = Collection.Select(ConvertType);
            lectures.Select((le,ind) => UploadFile(le , Collection.ElementAt(ind)));
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
            var lecture = await _instances.SingleOrDefaultAsync(lec => lec.ID == ID );
            return lecture is null ? null : lecture;
        }
          public async Task<StudeingLectureViewModel> GetAsync(int ID)
        {
            var lecture = await _instances.SingleOrDefaultAsync(lec => lec.ID == ID && lec.Type == LectureType.studying.ToString(), include: Lec => Lec.Include(Lec => Lec.Subject));
            return lecture is null ? null : ConvertToStudeingLectureViewModel(lecture);
        }

        public async Task<IPaginate<StudeingLectureViewModel>> GetAll(int index =0 , int size =20)
        {
            var lectures = await _instances.GetListAsync(lec=> lec.Type == LectureType.studying.ToString(), include: Lec=>Lec.Include(Lec=>Lec.Subject) , index:index , size:size);
            //  return lectures.Items.Select(ConvertToStudeingLectureViewModel);
            return new Paginate<Lecture, StudeingLectureViewModel>(lectures, l => l.Select(le => ConvertToStudeingLectureViewModel(le)));
        }

        public async Task<bool> IsExsist(int ID)
        {
            return await _instances.SingleOrDefaultAsync(lec => lec.ID == ID) is null ? false : true;
        }

        public async Task<HttpStatusCode> Update(AddStudyLectureViewModel instance)
        {
            var lecture = await Get(instance.ID);

            if (lecture is null) return HttpStatusCode.NotFound;
            UploadFile(lecture, instance);
            lecture.Name = instance.Name;
            lecture.Description = instance.Description;
            lecture.IsAppear = instance.IsAppear;
            lecture.IsFree = instance.IsFree;
            lecture.LectureLink = instance.LectureLink;
            lecture.Month = instance.Month;
            lecture.SubjectID = instance.SubjectId;
            

            return _instances.Update(lecture) is true ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        }

        private Lecture ConvertType(AddStudyLectureViewModel entity)
        {
            var lecture = _mapper.Map<Lecture>(entity);
            lecture.Type = LectureType.studying.ToString();
            return lecture;
        }

     
        private StudeingLectureViewModel ConvertToStudeingLectureViewModel(Lecture viewModel)
        {
            var lecture = _mapper.Map<StudeingLectureViewModel>(viewModel);
            return lecture;
        }

        public async Task<FileResponse> GetFile(int LectureID)
        {
            var lecture = await _instances.SingleOrDefaultAsync(lec => lec.ID == LectureID);
            return lecture is null ? null : ConvertToFileResonse(lecture);
        }

        private FileResponse ConvertToFileResonse(Lecture lecture)
        {
            var base64 = Convert.ToBase64String(lecture.File.FileData);
            string fileSrc = string.Format("data:application/pdf;base64," + base64);
            
            return new FileResponse { Name = lecture.FileName,
                FileData = fileSrc };
        }

        public bool UploadFile(Lecture lecture, IFileImage fileImage)
        {
          
            string name = null;
            byte[] data = null;

            if (_fileImageUploading.UploadFile(fileImage, out name, out data) && !string.IsNullOrEmpty(name))
            {
                lecture.FileName = name;
                lecture.File.FileData = data;
                return true;
            }
            return false;
        }
    }
}