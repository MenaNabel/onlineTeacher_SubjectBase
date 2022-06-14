using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Services.Reviews.Helper;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Threenine.Data;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Reviews
{
    public class ReviewServicesAsync : IReviewAsync
    {
       
        private readonly IRepositoryAsync<Review> _Reviews;
        private readonly IDeleteRepository<Review> _RepoDelete;
        private readonly IMapper _Mapper;

        private readonly IUserServices _user;

        public ReviewServicesAsync(IRepositoryAsync<Review> Reviews, IMapper mapper, IUserServices user,IDeleteRepository<Review> RepoDelete
            )

        {
            _Reviews = Reviews;
            _Mapper = mapper;
            _user = user;

             _RepoDelete = RepoDelete ;
           


        }
        [Authorize]
        public async Task<ReviewViewModel> Add(ReviewViewModel reviewViewModel)
        {
            
            Review review = ConvertToReview(reviewViewModel);
            review.StudentID =  GetCurrentUserID();
            review.StudentName =  _user.GetCurrentUserName();
          var ReviewAdded = await  _Reviews.InsertAsync(review);
            return ReviewAdded.Entity is null ? null : ConvertToReviewViewModel(ReviewAdded.Entity);
        }

        public Task<IEnumerable<ReviewViewModel>> AddRange(IEnumerable<ReviewViewModel> Collection)
        {
            throw new NotImplementedException();
        }
       /// <summary>
       /// Get All Review That not confirmed yet to Admin
       /// </summary>
       /// <returns></returns>
        public async Task<IEnumerable<ReviewDetailsViewModel>> GetAll()
        { // appear to teacher is not confirme yet
            
            var Reviews = await _Reviews.GetListAsync();
            return Reviews.Items.Select(ConvertToReviewDetailsViewModel);
        }  
        
        public async Task<IEnumerable<ReviewDetailsViewModel>> GetReviewsConfirmed()
        { // appear to teacher is  confirmed 
            var Reviews = await GetReviewsWithCondition(IsAppear: true);
            return Reviews.Items.Select(ConvertToReviewDetailsViewModel);

        }
       
        public async Task<ReviewViewModel> GetAsync(int id)
        {
            var Review = await _Reviews.SingleOrDefaultAsync(rev => rev.ID == id);
            return ConvertToReviewViewModel(Review);
        }

        public async Task<bool> Delete(int ID)
        {
            var Reviews = await _Reviews.SingleOrDefaultAsync(re => re.ID == ID);

           _RepoDelete.Delete(Reviews);
          return  _RepoDelete.commit();
        }
        [Authorize(Roles = Roles.Admin)]
        public async Task<bool> UpdateAppearance(ReviewUpdatedViewModel reviewUpdatedViewModel)
        {
            if (reviewUpdatedViewModel.ID == 0) return false;
            var Review = await ISFound(reviewUpdatedViewModel.ID);
            if (Review is null)
                return false;
            Review.IsAppear = reviewUpdatedViewModel.IsAppear;
          return _Reviews.Update(Review);
        }


        #region Helper
        private async Task<Review> ISFound(int ID)
        {
            Review review = await _Reviews.SingleOrDefaultAsync(R => R.ID == ID);
            return review;
        }
        
        private async Task<IPaginate<Review>> GetReviewsWithCondition(bool IsAppear)
        {

            var reviews = await _Reviews.GetListAsync(Revi => Revi.IsAppear == IsAppear);
            return reviews;
        }
        
        private ReviewDetailsViewModel ConvertToReviewDetailsViewModel(Review Review)
        {
            var ReviewViewModel = _Mapper.Map<ReviewDetailsViewModel>(Review);

            return ReviewViewModel;
        } 
        private ReviewViewModel ConvertToReviewViewModel(Review Review)
        {
            var ReviewViewModel = _Mapper.Map<ReviewViewModel>(Review);

            return ReviewViewModel;
        } 
        private Review ConvertToReview(ReviewViewModel ReviewViewModel)
        {
            var Review = _Mapper.Map<Review>(ReviewViewModel);

            return Review;
        } 
       
        
        private string GetCurrentUserID() {

            return _user.GetCurrentUserID();
        }

      
        #endregion
    }
}
