using OnlineTeacher.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Home.Helper
{
    public interface IHonerList
    {
        Task<IEnumerable<HonerListItemViewModel>> GetHonerList();
    }
}
