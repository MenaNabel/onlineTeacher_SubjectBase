using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.ViewModels.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Reviews.Helper
{
    public interface IReviewAsync : IInsertAsync<ReviewViewModel> , IReadAsync<ReviewDetailsViewModel>
    {

        Task<bool> UpdateAppearance(ReviewUpdatedViewModel reviewUpdatedViewModel);
        Task<bool> Delete(int ID);
        /// <summary>
        /// git Spaciifc Review
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ReviewViewModel> GetAsync(int id);
        /// <summary>
        // get reviews for site that admin confirm to appear
        /// </summary>
        /// <returns>
        /// Task<IEnumerable<ReviewDetailsViewModel>>
        /// </returns>
        Task<IPaginate<ReviewDetailsViewModel>> GetReviewsConfirmed(int index =0 , int size=20);
    }
}
