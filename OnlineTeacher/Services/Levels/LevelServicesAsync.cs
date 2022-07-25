using AutoMapper;
using OnlineTeacher.DataAccess.Context;

using OnlineTeacher.Services.Levels.Helper;
using OnlineTeacher.ViewModels.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Levels
{
    public class LevelServicesAsync : ILevelAsync
    {
        private readonly IMapper _Mapper;
        private readonly IRepositoryAsync<Level> _Levels;
        public LevelServicesAsync(IMapper mapper , IRepositoryAsync<Level> levelRepo)
        {
            _Mapper = mapper;
            _Levels = levelRepo;
        }
        public async Task<LevelViewModel> Add(LevelViewModel levelViewModel)
        {
            Level level = ConvertToLevel(levelViewModel);
            if (await Count() >= 3)
                throw new  Exception("can't add more than three Level");
            var leveladded = await _Levels.InsertAsync(level);
            return leveladded.Entity is null ? null : ConvertToLevelViewModel(leveladded.Entity);

        }

        public Task<IEnumerable<LevelViewModel>> AddRange(IEnumerable<LevelViewModel> Collection)
        {
            throw new NotImplementedException();
        }

        public async Task<IPaginate<LevelViewModel>> GetAll(int index =0 , int size =20)
        {
            var Levels = await _Levels.GetListAsync(index:index , size:size);
            //return Levels.Items.Select(ConvertToLevelViewModel);
            return new Paginate<Level, LevelViewModel>(Levels, l => l.Select(le => ConvertToLevelViewModel(le)));
        }

        public async Task<bool> update(LevelViewModel levelViewModel)
        {
            Level level = await Get(levelViewModel.ID);
            if (level is null) return false;
            level.LevelName = levelViewModel.LevelName;
            level.TelegeramLink = levelViewModel.TelegeramLink;
            return _Levels.Update(level);

        }
        #region Helper
        private async Task<Level> Get(int id) {
           return await _Levels.SingleOrDefaultAsync(le => le.ID == id);
        }
        private LevelViewModel ConvertToLevelViewModel(Level level) {
            LevelViewModel levelViewModel = _Mapper.Map<LevelViewModel>(level);
            return levelViewModel;
        }
        private Level ConvertToLevel(LevelViewModel levelViewModel) {
            Level Level = _Mapper.Map<Level>(levelViewModel);
            return Level;
        }

        public async Task<LevelViewModel> GetAsync(int Id)
        {
            return ConvertToLevelViewModel(await Get(Id));
        }
        private async Task<int> Count() {

            var Levels = await _Levels.GetListAsync();
            return Levels.Count;
        }
        #endregion
    }
}
