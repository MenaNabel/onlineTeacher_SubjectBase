using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Teachers.Helper
{
   public interface ISettingAsync 
    {
        Task<AddedSettingViewModel> Add(AddedSettingViewModel settingViewModel);
        Task<bool> Update(AddedSettingViewModel settingViewModel);
        Task<AddedSettingViewModel>  Get();
        Task delete();
    }
}
