using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.DataAccess.HelperConntext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Threenine.Data.Paging;

namespace OnlineTeacher.DataAccess.Repository.CustomeRepository
{
    public interface ISubscriptionReposiory
    {
        IPaginate<SubscribitionDetails> GetAllSubscrbtions(int index = 0, int size = 20);
        Task<List<SubscribitionDetails>> GetSubscrbtionsForStudnet(int StudnetID);
        Task<List<SubscribitionDetails>> GetSubscrbtionsForStudnet(string StudnetID);
        bool Update(Subscription subscription);
        void Update(IEnumerable<Subscription> subscription);
        void Remove(IEnumerable<Subscription> subscription);
        void Remove(Subscription subscription);
        
        Task InsertAsync(IEnumerable<Subscription> subscriptions);
        Task<Subscription> InsertAsync(Subscription subscription);
        Task InsertAsync(Subscription[] subscriptions);
        IOrderedQueryable<SubscribitionDetails> GetAllSubscrbtionsNotConfirmed();
        Task<bool> Commit();
        IPaginate<SubscribitionDetails> GetSubscrbtionsForStudnet(Expression<Func<Student, bool>> FilterCondition, int index, int size);
    }
}
