using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.DataAccess.HelperConntext;
using OnlineTeacher.ViewModels.Subscribtions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Threenine.Data.Paging;

namespace OnlineTeacher.Services.Subscriptions.Helper
{
    public interface ISubscribtion
    {

        IPaginate<SubscribitionDetails> GetAllSubscrbtion(int index =0, int size = 20);
         IEnumerable<SubscriptionViewModel> GetSubscrbtionsNotAccepeted();
         Task<SubscrptionResponseManger> Subscribe(IEnumerable<AddSubscibtionViewModel> subscribtions);
        Task<SubscrptionResponseManger> Subscribe(AddSubscibtionViewModel subscribtionsViewModel);
        Task<IEnumerable<SubscriptionViewModel>> GetAllSubscrbtionForStudent(int StudentID);
        Task<IEnumerable<SubscriptionViewModel>> GetAllSubscrbtionForStudent(string StudentID);
        Task<IEnumerable<SubscriptionViewModel>> GetAllSubscrbtionForCurrentStudent();
        Task<SubscrptionResponseManger> Active(UpdateSubscribtionViewModel subscribtionViewModel);
        Task<SubscrptionResponseManger> Active(IEnumerable<UpdateSubscribtionViewModel> subscribtionViewModel);
        Task<SubscrptionResponseManger> Remove(AddSubscibtionViewModel subscribtionViewModel);
        Task<SubscrptionResponseManger> Remove(IEnumerable<AddSubscibtionViewModel> subscribtionViewModel);
        IPaginate<SubscriptionViewModel> filter(Expression<Func<Student, bool>> FilterCondition, int index = 0, int size = 20);


    }
}
