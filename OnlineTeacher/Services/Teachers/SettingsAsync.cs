using AutoMapper;
using Microsoft.AspNetCore.Http;
using OnlineTeacher.DataAccess;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Teachers.Helper;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Setting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data;

namespace OnlineTeacher.Services.Teachers
{
    public class SettingsAsync : ISettingAsync
    {
        private readonly IRepositoryAsync<Teacher> _Teachers;
        private readonly IMapper _Mapper;
       private readonly IUserServices _user;
       private readonly IDeleteRepository<Teacher> _deleteTeacher;
        public SettingsAsync(IRepositoryAsync<Teacher> Teachers , IMapper mapper, IUserServices user, IDeleteRepository<Teacher> deleteTeacher)
        {
            _Teachers = Teachers;
           _Mapper = mapper;
            _user = user;
            _deleteTeacher = deleteTeacher;
        }
         public  async Task<AddedSettingViewModel> Add(AddedSettingViewModel settingViewModel)
        {
            // calc teacher count in system 
            int teacherCount = await Count();
            // can add one teacher in this virsion 
            if (teacherCount > 0) throw new Exception("You system have  premission to one teacher only if you need upgrades please contact with administrator , Thank you ");
            Teacher teacher = new Teacher();
            if (settingViewModel.FormFile.Length > 0
                /*&& _validExtensions.Contains(settingViewModel.FormFile.FileName.Substring(settingViewModel.FormFile.FileName.Length - 3,3))*/)
            {
                using (var stream = new MemoryStream())
                {
                    await settingViewModel.FormFile.CopyToAsync(stream);
                    teacher.PersonalImage = stream.ToArray();
                }
            }
            teacher = _Mapper.Map<Teacher>(settingViewModel);
            teacher.UserID = await _user.CreateAdminAdndAssignToRole();

            // add teacher setting in system
            var TeacherAdded = await _Teachers.InsertAsync(teacher);
             // check if  not added return null 
            return TeacherAdded.Entity is not null ? _Mapper.Map<AddedSettingViewModel> (TeacherAdded.Entity) : null;

        }
        public async Task delete() {
            var techer = await _Teachers.GetListAsync();
            _deleteTeacher.Delete(techer.Items);
             _deleteTeacher.commit();
        }
        

        public async  Task<AddedSettingViewModel> Get()
        {
            var Teachers = await _Teachers.GetListAsync(size:1);
            Teacher teacher = Teachers.Items.FirstOrDefault();
            if(teacher is null)
            {
                return null;
            }

            //return teacher is not null ? _Mapper.Map<AddedSettingViewModel>(teacher) : null ;
            AddedSettingViewModel addedSettingViewModel = new AddedSettingViewModel();
            string imageSrc;
            if(teacher.PersonalImage != null)
            {
                var base64 = Convert.ToBase64String(teacher.PersonalImage);
                imageSrc = string.Format("data:image/gif;base64,"+base64);
            } else
            {
                imageSrc = "";
            }
            addedSettingViewModel.FileName = imageSrc;
            return _Mapper.Map(teacher, addedSettingViewModel);
        }

        public async Task<bool> Update(AddedSettingViewModel settingViewModel)
        {
           Teacher teacher  = await singleOrDefault();
            if (teacher is null) return false;

            teacher.Name = settingViewModel.Name;
            teacher.PhoneNumber = settingViewModel.PhoneNumber;
            teacher.SecretarialPhoneNumber = settingViewModel.SecretarialPhoneNumber;
            teacher.TelegramLink = settingViewModel.TelegramLink;
            teacher.VodafonCachPhoneNumber = settingViewModel.VodafonCachPhoneNumber;
            teacher.WhatsappLink = settingViewModel.WhatsappLink;
            teacher.FacebookLink = settingViewModel.FacebookLink;
            teacher.Description = settingViewModel.Description;
            teacher.Address = settingViewModel.Address;
            teacher.ActivationSubscriptionPhoneNumber = settingViewModel.ActivationSubscriptionPhoneNumber;
            if (settingViewModel.FormFile != null)
            {
                teacher.File = settingViewModel.FormFile.FileName;
            }

            if(settingViewModel.FormFile != null)
            {
                if (settingViewModel.FormFile.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await settingViewModel.FormFile.CopyToAsync(stream);
                        teacher.PersonalImage = stream.ToArray();
                    }
                }
            }
            
            
            ApplicationUser user = new ApplicationUser
            {
                Id = teacher.UserID,
                Name = teacher.Name,
                Email = settingViewModel.Email

            };
                
            return await _user.Update(user) && _Teachers.Update(teacher);
        }
        public async Task<Teacher> singleOrDefault()
        {
            string id = GetUserID();
           
            Teacher teacher = await _Teachers.SingleOrDefaultAsync(setting => setting.UserID == "69e23a28-3a6f-4838-82e1-3436607d89d3");
            //Teacher teacher = await _Teachers.SingleOrDefaultAsync(setting => setting.UserID == id);
            return teacher;
        }
        #region Helper
        private async Task<int> Count()
        {
            var Teachers = await _Teachers.GetListAsync();
         return Teachers.Count;
        }
        private string GetUserID()
        {
            return  _user.GetCurrentUserID();
        }
        #endregion
    }
}
