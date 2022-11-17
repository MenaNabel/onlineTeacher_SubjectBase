using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Subjects.Helper;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Services;
using OnlineTeacher.ViewModels.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Threenine.Data;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Subjects
{
    /// <summary>
    /// Teacher Functionality 
    /// </summary>
    public partial class SubjectServicesAsync : ISubjectAsync
    {
        private readonly IRepositoryAsync<Subject> _Subjects;
        private readonly IDeleteRepository<Subject> _DeleteRepo;
        private readonly IMapper _Mapper;
        private IFileImageUploading _ImageUploading;
        private IUserServices _user;
        private IRepositoryAsync<Subscription> _Subscribtions;
        private IServiceProvider _serviceProvider;


        public SubjectServicesAsync(
            IRepositoryAsync<Subject> Subjects,
            IMapper mapper,
            IDeleteRepository<Subject> deleteSubjects,
             IServiceProvider serviceProvider
            )
        {
            _Subjects = Subjects;
            _Mapper = mapper;
            _DeleteRepo = deleteSubjects;
            _serviceProvider = serviceProvider;
        }
        public async Task<SubjectMangerResponse> Add(AddingSubjectViewModel addingSubjectViewModel)
        {
            SubjectMangerResponse Resonse = null;
            Subject subject = ConvertType(addingSubjectViewModel);


            if (UploadPhoto(subject, addingSubjectViewModel) && !string.IsNullOrEmpty(subject.ImagePath))
            {

                if (await InsertToDB(subject))
                    return Resonse = new SubjectMangerResponse("Subject  added Correctly", true);
            }


            if (await InsertToDB(subject))
                return Resonse = new SubjectMangerResponse("Subject added Correctly but Upload Photo Have Problem", true);

            return Resonse = new SubjectMangerResponse("Subject not added Correctly", false);
        }
        private async Task<bool> InsertToDB(Subject subject)
        {

            var AddedSubjectViewModel = await _Subjects.InsertAsync(subject);
            return AddedSubjectViewModel.Entity is not null ? true : false;
        }
        public async Task<SubjectViewModel> GetAsync(int ID)
        {
            var Subject = await _Subjects.SingleOrDefaultAsync(sub => sub.ID == ID, include: Sub => Sub.Include(s => s.level));
            return ConvertType_Subject(Subject);
        }
        public async Task<IEnumerable<AddingSubjectViewModel>> AddRange(IEnumerable<AddingSubjectViewModel> addingSubjects)
        {
            IEnumerable<Subject> subjects = addingSubjects.Select(ConvertType);
            try
            {
                await _Subjects.InsertAsync(subjects);
            }
            catch (Exception ex)
            {
                throw new Exception("Server Error in addedd Range " + ex.Message);
            }
            return _Subjects.Commit() == true ? addingSubjects : throw new Exception("Server Error Commit Range");

        }

        public async Task<bool> Delete(int ID)
        {
            var subject = await Get(ID);

            if (subject is null)
                return false;

            _DeleteRepo.Delete(subject);

            return _DeleteRepo.commit();

        }

        public async Task<IPaginate<SubjectViewModel>> Filter(Expression<Func<Subject, bool>> FilterCondition, int index =0 , int size = 10)
        {
            var Subjects = await _Subjects.GetListAsync(FilterCondition, include: Sub => Sub.Include(s => s.level) , index:index , size:size);
            return new Paginate<Subject, SubjectViewModel>(Subjects, s => s.Select(su => ConvertType_Subject(su)));


        }
        /// <summary>
        /// Used in Services Layer  Not in  Presentaion Layer
        /// </summary>
        /// <param name="ID">Subject ID </param>
        /// <returns>
        /// Subject 
        /// </returns>
        public async Task<Subject> Get(int ID)
        {
            var Subject = await _Subjects.SingleOrDefaultAsync(Subj => Subj.ID == ID, include: Sub => Sub.Include(s => s.level));
            return Subject is null ? null : Subject;
        }

        public async Task<IPaginate<SubjectViewModel>> GetAll(int index =0 , int size =20)
        {
            var Subjects = await _Subjects.GetListAsync(include: Sub => Sub.Include(s => s.level) , index:index , size:size);
            // return Subjects.Items.Select(ConvertType_Subject);
            return new Paginate<Subject, SubjectViewModel>(Subjects, s => s.Select(su => ConvertType_Subject(su)));

        }
        public async Task<IEnumerable<SubjectViewModel>> GetAll()
        {
            var subjects = await _Subjects.GetListAsync<SubjectViewModel>(selector: sub => new SubjectViewModel { ID = sub.ID, Name = sub.Name });
            return subjects.Items;
        }



        public async Task<SubjectMangerResponse> Update(AddingSubjectViewModel subjectViewModel)
        {

            var Subject = await Get(subjectViewModel.ID);

            if (Subject is null) return new SubjectMangerResponse("Subjec  Not Found ", false);

            #region Update Data
            Subject.Name = subjectViewModel.Name;
            Subject.Price = subjectViewModel.Price;
            Subject.LevelID = subjectViewModel.LevelID;
            if (UploadPhoto(Subject, subjectViewModel))
            {
                if (_Subjects.Update(Subject))
                    return new SubjectMangerResponse("Subject Updated Sucessfuly", true);
                else
                    return new SubjectMangerResponse("Subject Not  Updated Sucessfuly ", false);
            }

            #endregion


            return _Subjects.Update(Subject) is true ?
                new SubjectMangerResponse("Subject Updated Sucessfuly but photo Not updated ", true,
                                      new List<string> { "Photo Not Updated have bad Format " })

                : new SubjectMangerResponse("Subject Not  Updated Sucessfuly ", false);

        }


        #region Helper
        private Subject ConvertType(AddingSubjectViewModel viewModel)
        {
            var subject = _Mapper.Map<Subject>(viewModel);

            return subject;
        }
        private SubjectViewModel ConvertType_Subject(Subject viewModel)
        {
            var subject = _Mapper.Map<SubjectViewModel>(viewModel);

            return subject;
        }
        private AddingSubjectViewModel ConvertToAddedSubjectViewModel(Subject viewModel)
        {
            var subject = _Mapper.Map<AddingSubjectViewModel>(viewModel);

            return subject;
        }
        public bool UploadPhoto(Subject subject, IFileImage fileImage)
        {
            _ImageUploading = _serviceProvider.GetRequiredService<IFileImageUploading>();
            string path = null;

            if (_ImageUploading.UploadPhoto(fileImage, out path) && !string.IsNullOrEmpty(path))
            {
                subject.ImagePath = path;
                return true;

            }
            return false;
        }



        #endregion

    }

    /// <summary>
    /// student Functionality 
    /// </summary>
    public partial class SubjectServicesAsync
    {

        public async Task<IEnumerable<SubjectViewModel>> GetAllForCurrentStudent()
        {
            #region Injection
            _user = _serviceProvider.GetRequiredService<IUserServices>();
            _Subscribtions = _serviceProvider.GetRequiredService<IRepositoryAsync<Subscription>>();
            #endregion

            int UserID = _user.GetStudentID();
            if (UserID == 0)
                return null;
            var Subjects = await _Subscribtions.GetListAsync(Subsc =>
            (Subsc.StudentID == UserID) && (Subsc.IsActive == true),
                include: Sub => Sub.Include(S => S.Subject).ThenInclude(Sub => Sub.level));
            return Subjects.Items.Select(ite => ConvertType_Subject(ite.Subject));

        }
    }
}