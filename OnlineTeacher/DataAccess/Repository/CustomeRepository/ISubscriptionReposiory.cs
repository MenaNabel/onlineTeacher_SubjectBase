﻿using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.DataAccess.HelperConntext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Repository.CustomeRepository
{
    public interface ISubscriptionReposiory
    {
        IOrderedQueryable<SubscribitionDetails> GetAllSubscrbtions();
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
    }
}