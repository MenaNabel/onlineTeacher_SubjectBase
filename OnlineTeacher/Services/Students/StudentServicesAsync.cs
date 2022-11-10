using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Students.Helper;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Students;
using OnlineTeacher.ViewModels.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Threenine.Data;
using Threenine.Data.Paging;
using System.Security.Cryptography;

namespace OnlineTeacher.Services.Students
{
    public class StudentServicesAsync : IStudentAsync
    {
        private readonly IRepositoryAsync<Student> _Students;

        private readonly IMapper _Mapper;
        private readonly IUserServices _User;
        private readonly IFileImageUploading _ImageUploading;
        private readonly OnlineExamContext _context;
       
        public StudentServicesAsync(IRepositoryAsync<Student> Student, IMapper Mapper, IUserServices user,
            IFileImageUploading ImageUploading ,OnlineExamContext context)
        {
            _Students = Student;
            _Mapper = Mapper;
            _User = user;
            _ImageUploading = ImageUploading;
            _context = context;
        }
        public async Task<AddedStudentViewModel> Add(AddedStudentViewModel studentViewModel)
        {
            Student student = ConvertToAddStudentViewModel(studentViewModel);
            StudentAddProcess(student);
            var StudentAdded = await _Students.InsertAsync(student);

            return StudentAdded?.Entity is not null ? ConvertToAddStudentViewModel(StudentAdded.Entity) : null;

        }



        public Task<IEnumerable<AddedStudentViewModel>> AddRange(IEnumerable<AddedStudentViewModel> Collection)
        {
            throw new NotImplementedException();
        }

        public async Task<IPaginate<StudentViewModel>> GetAll(int index , int size)
        {
           
                     
            var Students = await _Students.GetListAsync(include: St => St.Include(s => s.Level),index:index , size:size);
            return new Paginate<Student, StudentViewModel>(Students, s => s.Select(st => ConvertToStudentViewModel(st)));
            
          //  var students = _context.Student.Select(x => new StudentViewModel
          //  {
          //      Level = _Mapper.Map<LevelViewModel>(x.Level),
          //      City = x.City,
          //      Email = x.Email,
          //      ID = x.ID,
          //      Name = x.Name,
          //      LevelID = x.LevelID,
          //      Phone = x.Phone
          //  });
          //return students;
        }
        public async Task<StudentViewModel> GetAsync()
        {

            var Student = await Get(GetUserID());
            if (Student is null)
                return null;
            //if(IsAdminValidate() ||  UserRequestingValiedTodealWithEntityValidation(Student.UserID))
            return ConvertToStudentViewModel(Student);
            // return null;
        }
        public async Task<StudentViewModel> GetAsyncWithoutValidate(string UserID)
        {

            try
            {
                var Student = await _Students.SingleOrDefaultAsync(S => S.UserID == UserID);
                return Student is not null ? ConvertToStudentViewModel(Student) : null;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public async Task<bool> Update(UpdatedStudnetViewModel studentviewModel)
        {
            studentviewModel.UserID = GetUserID();
            Student student = await Get(studentviewModel.UserID);

            if (student is null || !UserRequestingValiedTodealWithEntityValidation(student.UserID))
                return false;


            if (await StudentUpdateProcess(student, studentviewModel))
                return _Students.Update(student);

            return false;

        }

        #region Helper
        public bool UploadPhoto(Student Student, IFileImage fileImage)
        {

            string path = null;

            if (_ImageUploading.UploadPhoto(fileImage, out path) && !string.IsNullOrEmpty(path))
            {
                Student.Image = path;
                return true;

            }
            return false;
        }
        private AddedStudentViewModel ConvertToAddStudentViewModel(Student student)
        {

            return _Mapper.Map<AddedStudentViewModel>(student);
        }
        private StudentViewModel ConvertToStudentViewModel(Student student)
        {

            if (student is null)
                return null;

            return _Mapper.Map<StudentViewModel>(student);
        }
        private Student ConvertToAddStudentViewModel(AddedStudentViewModel studentViewModel)
        {

            return _Mapper.Map<Student>(studentViewModel);
        }
        private string GetUserID()
        {
            return _User.GetCurrentUserID();
        }
        private void StudentAddProcess(Student student)
        {
            student.ID = 0;


        }
        private async Task<Student> Get(string UserID)
        {
            try
            {
                var ID = _User.GetStudentID();
                var Student = await _Students.SingleOrDefaultAsync(S => S.UserID == UserID, include: St => St.Include(s => s.Level).Include(s => s.Subscriptions.Where(sub => sub.StudentID == ID)));
                return Student is not null ? Student : null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<IPaginate<StudentViewModelWithoutImage>> filter(Expression<Func<Student, bool>> filter , int index =0 , int size = 10) 
        {
            return await Get(filter , index , size); 
        }
        private async Task<IPaginate<StudentViewModelWithoutImage>> Get(Expression<Func<Student, bool>> filter, int index = 0, int size = 10)
        {
            try
            {
                var Students = await _Students.GetListAsync(st => new StudentViewModelWithoutImage { ID = st.ID , LevelID = st.LevelID , Name = st.Name ,Phone = st.Phone , City = st.City , Email = st.Email} , filter, include: St => St.Include(s => s.Level) , index:index , size:size);
                return Students ;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        private async Task<bool> StudentUpdateProcess(Student student, UpdatedStudnetViewModel studentviewModel)
        {
            // if any object is null return and not make update process
            if (student is null || studentviewModel is null)
                return false;

            ApplicationUser user = new ApplicationUser()
            {
                Id = GetUserID(),
                PhoneNumber = studentviewModel.Phone,
                Name = studentviewModel.Name
            };
            // update entity value 
            student.City = studentviewModel.City;
            if (student.Subscriptions.Count() == 0)
            {
                student.LevelID = studentviewModel.LevelID;
            }
            student.Name = studentviewModel.Name;
            student.Phone = studentviewModel.Phone;
            UploadPhoto(student, studentviewModel);
            return await _User.Update(user);
        }

        public async Task<bool> StudentUpdatePhoneNumber()
        {
            try
            {
                //var x = from stud in _context.Student
                //        where stud.Phone == null
                //        select stud ;

                //foreach (var item in x)
                //{
                //    item.Phone = "لا يوجد بيانات";
                //}

              var students = await  _Students.GetListAsync(st => st.Phone == null);
                foreach (var item in students.Items)
                {
                    item.Phone = "لا يوجد بيانات"; 
                    _Students.Update(item);
                   
                }
                return _Students.Commit();
                 

                 
            }
            catch
            {
                return false;
            }
        }

        #endregion
        #region Validation 
        private bool UserRequestingValiedTodealWithEntityValidation(string UserID)
        {
            return UserID == GetUserID();

        }
        private bool IsAdminValidate()
        {
            return _User.GetCurrentUserRole() == Roles.Admin ? true : false;
        }
        #endregion
    }
}
