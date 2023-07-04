
using OnlineTeacher.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Home
{
    public interface ISiteInfo
    {
      Task<SiteInfoViewModel>  getSiteInfo();
      
    }
}
