using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.ViewModels.Subscribtions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Services.Subscriptions.Helper
{
    public interface ISubscribtion
    {

        IEnumerable<SubscriptionViewModel> GetAllSubscrbtion();
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


    }
}
