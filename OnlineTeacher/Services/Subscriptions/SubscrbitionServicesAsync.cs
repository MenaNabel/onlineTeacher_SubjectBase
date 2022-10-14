using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.DataAccess.HelperConntext;
using OnlineTeacher.DataAccess.Repository.CustomeRepository;
using OnlineTeacher.Services.Students.Helper;
using OnlineTeacher.Services.Subscriptions.Helper;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Subscribtions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Threenine.Data;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Subscriptions
{
    public class SubscrbitionServicesAsync : ISubscribtion
    {
        private readonly ISubscriptionReposiory _Subscribtions;
        private readonly IMapper _Mapper;
        private readonly IUserServices _User;
        private readonly IStudentAsync _Students;
        private readonly IRepositoryAsync<Subject> _Subjscts;
        public SubscrbitionServicesAsync(ISubscriptionReposiory Subscribtions, IMapper Mapper,IUserServices User, IStudentAsync Students, IRepositoryAsync<Subject> Subjscts)
        {
            _Subscribtions = Subscribtions;
            _Mapper = Mapper;
            _User = User;
            _Students = Students;
            _Subjscts = Subjscts;
        }

        public async Task<SubscrptionResponseManger> Active(UpdateSubscribtionViewModel subscribtionViewModel)
        {
            var Subscribtion = ConvertToSubscribtion(subscribtionViewModel);
            Subscribtion.IsActive = subscribtionViewModel.IsActive;
            _Subscribtions.Update(Subscribtion);
            if (await _Subscribtions.Commit() == true)
            {
                return new SubscrptionResponseManger
                {
                    Message = "Activate Subscribtion Subccessfuly",
                    IsSuccess = true
                };
            }

            return new SubscrptionResponseManger
            {
                Message = "Not Activate Subscribtion Subccessfuly",
                IsSuccess = false
            };
        }

        public async Task<SubscrptionResponseManger> Active(IEnumerable<UpdateSubscribtionViewModel> subscribtionViewModel)
        {
            var Subscribtion = subscribtionViewModel.Select(ConvertToSubscribtion);

            _Subscribtions.Update(Subscribtion);
            if (await _Subscribtions.Commit() == true)
            {
                return new SubscrptionResponseManger
                {
                    Message = "Activate Subscribtion Subccessfuly",
                    IsSuccess = true
                };
            }

            return new SubscrptionResponseManger
            {
                Message = "Not Activate Subscribtion Subccessfuly",
                IsSuccess = false
            };
        }

        public IPaginate<SubscribitionDetails> GetAllSubscrbtion(int index , int size)
        {
            return _Subscribtions.GetAllSubscrbtions(index , size);
          
        }

        public async Task<IEnumerable<SubscriptionViewModel>> GetAllSubscrbtionForStudent(int StudentID)
        {
            
            if (_User.GetCurrentUserRole() != Roles.Admin) {
                var User = await _Students.GetAsync(); // get all subscribtion for current user 
                if (User.ID != StudentID)
                    throw new UnauthorizedAccessException();
            }
            var Subscribtion = await  _Subscribtions.GetSubscrbtionsForStudnet(StudentID);
            return  Subscribtion.Select(ConvertToSubscribitionViewModel);
        }
        public async Task<IEnumerable<SubscriptionViewModel>> GetAllSubscrbtionForCurrentStudent()
        {
            
            
            var Subscribtion = await  _Subscribtions.GetSubscrbtionsForStudnet(_User.GetCurrentUserID());
            return  Subscribtion.Select(ConvertToSubscribitionViewModel);
        }
        public async Task<IEnumerable<SubscriptionViewModel>> GetAllSubscrbtionForStudent(string StudentID)
        {
            var Subscribtion = await _Subscribtions.GetSubscrbtionsForStudnet(StudentID);
            return Subscribtion.Select(ConvertToSubscribitionViewModel);
        }

        public async Task<IEnumerable<SubscriptionViewModel>> filter(Expression<Func<Student, bool>> FilterCondition)
        {
            var Subscribtion = await _Subscribtions.GetSubscrbtionsForStudnet(FilterCondition);
            return Subscribtion.Select(ConvertToSubscribitionViewModel);
        }

        public IEnumerable<SubscriptionViewModel> GetSubscrbtionsNotAccepeted()
        {
            var Subscribtion = _Subscribtions.GetAllSubscrbtionsNotConfirmed();
            return Subscribtion.Select(ConvertToSubscribitionViewModel);
        }

        public async Task<SubscrptionResponseManger> Remove(AddSubscibtionViewModel subscribtionViewModel)
        {

            _Subscribtions.Remove(ConvertToSubscribtion(subscribtionViewModel));
            if (await _Subscribtions.Commit() == true)
            {
                return new SubscrptionResponseManger
                {
                    Message = "removing Subscribtion Subccessfuly",
                    IsSuccess = true
                };
            }

            return new SubscrptionResponseManger
            {
                Message = "Not Remove Subscribtion Subccessfuly",
                IsSuccess = false
            };
        }

        public async Task<SubscrptionResponseManger> Remove(IEnumerable<AddSubscibtionViewModel> subscribtionViewModel)
        {

            var Subscribtions = subscribtionViewModel.Select(ConvertToSubscribtion);
            _Subscribtions.Remove(Subscribtions);
            if (await _Subscribtions.Commit() == true)
            {
                return new SubscrptionResponseManger
                {
                    Message = "removing Subscribtion Subccessfuly",
                    IsSuccess = true
                };
            }

            return new SubscrptionResponseManger
            {
                Message = "Not Remove Subscribtion Subccessfuly",
                IsSuccess = false
            };
        }

        public async Task<SubscrptionResponseManger> Subscribe(IEnumerable<AddSubscibtionViewModel> subscribtionsViewModel)
        {
            IEnumerable<Subscription> subscribtions = subscribtionsViewModel.Select(ConvertToSubscribtion);
            try
            {
                int StudentID = _User.GetStudentID();
                bool IsValidate = subscribtions.All(S => S.StudentID == StudentID);
                if (!IsValidate)
                    return new SubscrptionResponseManger
                    {
                         Message = "Unauthorize Subsecribtion",
                         IsSuccess= false

                    };

                #region Validate that student the same Level of Subjects 
                //var Student = await _Students.GetAsyncWithoutValidate(_User.GetCurrentUserID());
                //var Subject = await _Subjscts.GetListAsync(s => subscribtions.));

                //if (!Subject.Items.All(sub => sub.LevelID == Student.LevelID))
                //{
                //    return new SubscrptionResponseManger
                //    {
                //        Message = "not in the same level can't subscribtion",
                //        IsSuccess = false

                //    };
                //}
                #endregion

                await _Subscribtions.InsertAsync(subscribtions);

                var Result = await _Subscribtions.Commit();
                if (Result is true)
                    return new SubscrptionResponseManger
                    {
                        Message = "Subscribe Successfuly",
                        IsSuccess = true
                    };
                return new SubscrptionResponseManger
                {
                    Message = "Not Subscribe Successfuly",
                    IsSuccess = false
                };
            }
            catch (SqlException ex)
            {
                return new SubscrptionResponseManger
                {
                    Message = "Not Subscribe Successfuly"  ,
                    IsSuccess = false,
                    Errors = new List<string> { "This subscription already exists" , ex.Errors.ToString() },
                };

            }catch (DbUpdateException ex)
            {
                return new SubscrptionResponseManger
                {
                    Message = "Not Subscribe Successfuly"  ,
                    IsSuccess = false,
                    Errors = new List<string> { "This subscription already exists" , ex.InnerException.ToString() },
                };

            }
        }
        public async Task<SubscrptionResponseManger> Subscribe(AddSubscibtionViewModel subscribtionsViewModel)
        {
            var subscribtion = ConvertToSubscribtion(subscribtionsViewModel);
            try
            {
                await _Subscribtions.InsertAsync(subscribtion);
                var Result = await _Subscribtions.Commit();
                if (Result is true)
                    return new SubscrptionResponseManger
                    {
                        Message = "Subscribe Successfuly",
                        IsSuccess = true
                    };
                return new SubscrptionResponseManger
                {
                    Message = "Not Subscribe Successfuly",
                    IsSuccess = false
                };
            }
            catch (Exception ex)
            {
                return new SubscrptionResponseManger
                {
                    Message = "Not Subscribe Successfuly" + ex.Message + "\n" + ex.InnerException?.Message,
                    IsSuccess = false
                };

            }
        }


        #region Helper
        private SubscriptionViewModel ConvertToSubscribitionViewModel(SubscribitionDetails subscrbition)
        {
            return _Mapper.Map<SubscriptionViewModel>(subscrbition);
        }

        private Subscription ConvertToSubscribtion(AddSubscibtionViewModel subscriptionView)
        {
            return _Mapper.Map<Subscription>(subscriptionView);
        }
        private Subscription ConvertToSubscribtion(UpdateSubscribtionViewModel subscriptionView)
        {
            return _Mapper.Map<Subscription>(subscriptionView);
        }

        #endregion
    }
}
