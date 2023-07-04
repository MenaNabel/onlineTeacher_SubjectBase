using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Home.Helper;
using OnlineTeacher.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data;

namespace OnlineTeacher.Services.Home
{
    public class HonerListServicesAsync : IHonerList
    {
        private IRepositoryAsync<HonerList> _Honers;
        private IMapper _Mapper;
        public HonerListServicesAsync(IRepositoryAsync<HonerList> Honers, IMapper Mapper)
        {
            _Honers = Honers;
            _Mapper = Mapper;

        }
        public async Task<IEnumerable<HonerListItemViewModel>> GetHonerList()
        {
           var Honers = await _Honers.GetListAsync(include: h => h.Include(e => e.Level));
            return Honers.Items.Select(_Mapper.Map<HonerListItemViewModel>);
        }
     
    }
}
