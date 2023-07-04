using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Levels.Helper
{
    public interface ILevelAsync : IInsertAsync<LevelViewModel> ,IReadAsync<LevelViewModel>
    {
        Task<bool> update(LevelViewModel levelViewModel);
        Task<LevelViewModel> GetAsync(int Id);
    }
}
