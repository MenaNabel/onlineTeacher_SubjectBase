using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.DataAccess.HelperConntext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Repository.CustomeRepository
{
    public class SubscribitionRepository : ISubscriptionReposiory
    {
        private readonly DbSet<Subscription> _dbSet;
        private readonly OnlineExamContext _dbContext;
        public SubscribitionRepository(OnlineExamContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<Subscription>();
        }

        public async Task<bool> Commit()
        {
           
                return await _dbContext.SaveChangesAsync() > 0 ? true : false;
          
        }

        public  IOrderedQueryable<SubscribitionDetails> GetAllSubscrbtions()
        {

            var Subscriptions =
                                 (from Student in _dbContext.Student
                                  join Subscribe in _dbSet on
                                 Student.ID equals Subscribe.StudentID
                                  join Subj in _dbContext.Subjects on
                                 Subscribe.SubjectID equals Subj.ID
                                 select new SubscribitionDetails
                                  {
                                     SubjectID = Subj.ID,
                                      StudentName = Student.Name,
                                      SubjectName = Subj.Name,
                                      IsActive = Subscribe.IsActive,
                                      LevelID = Subj.LevelID,
                                      Date = Subscribe.DataAndTime,
                                     StudentID = Student.ID
                                 }).OrderByDescending(SubDetailes=>SubDetailes.Date);

            
            return  Subscriptions;

           
        }
        
        public IOrderedQueryable<SubscribitionDetails> GetAllSubscrbtionsNotConfirmed()
        {

            var Subscriptions =
                                 (from Student in _dbContext.Student
                                  join Subscribe in _dbSet on
                                 Student.ID equals Subscribe.StudentID
                                 where Subscribe.IsActive == false
                                  join Subj in _dbContext.Subjects on
                                 Subscribe.SubjectID equals Subj.ID
                                  select new SubscribitionDetails
                                  {
                                      SubjectID = Subj.ID,
                                      StudentName = Student.Name,
                                      SubjectName = Subj.Name,
                                      IsActive = Subscribe.IsActive,
                                      LevelID = Subj.LevelID,
                                      Date = Subscribe.DataAndTime,
                                       StudentID = Student.ID
                                  }).OrderByDescending(SubDetailes => SubDetailes.Date);


            return Subscriptions;


        }
        
        public async Task<List<SubscribitionDetails>> GetSubscrbtionsForStudnet(int StudentID)
        {
            try
            {
                //var Subscriptions =
                //                (from Student in _dbContext.Student
                //                 where Student.ID == StudentID
                //                 join Subscribe in _dbSet on
                //                Student.ID equals Subscribe.StudentID
                //                 join Subj in _dbContext.Subjects on
                //                Subscribe.SubjectID equals Subj.ID
                //                 select new SubscribitionDetails
                //                 {
                //                     StudentID = Student.ID,
                //                     StudentName = Student.Name,
                //                     SubjectName = Subj.Name,
                //                     IsActive = Subscribe.IsActive,
                //                     LevelID = Subj.LevelID,
                //                     Date = Subscribe.DataAndTime
                //                 }).OrderByDescending(SubDetailes => SubDetailes.Date);
                
                //var result = await Subscriptions.ToListAsync().ConfigureAwait(true);
                var JoinedResult = _dbContext.Student.Where(st=>st.ID == StudentID).
                     Join(_dbSet,
                             Stud => Stud.ID,
                             Subscribe => Subscribe.StudentID,
                             (Student, Subscribe) => new { Student= Student, Subscribe = Subscribe }).
                     Join(_dbContext.Subjects,
                             Subscribtion => Subscribtion.Subscribe.SubjectID,
                             Subject => Subject.ID,
                             (StudentSubscribtion, Subject) => new SubscribitionDetails
                             {
                                 
                                 StudentID = StudentSubscribtion.Student.ID,
                                 StudentName = StudentSubscribtion.Student.Name,
                                 SubjectName = Subject.Name,
                                 SubjectID = Subject.ID,
                                 IsActive = StudentSubscribtion.Subscribe.IsActive,
                                 LevelID = Subject.LevelID,
                                 Date = StudentSubscribtion.Subscribe.DataAndTime
                             }).OrderByDescending(SubDetailes => SubDetailes.Date).ToListAsync();

                return await JoinedResult;
            }
            catch (Exception ex) {
                return null;
            
            }
        }
        public async Task<List<SubscribitionDetails>> GetSubscrbtionsForStudnet(string StudentID)
        {
            var JoinedResult = _dbContext.Student.Where(st => st.UserID == StudentID).
                     Join(_dbSet,
                             Stud => Stud.ID,
                             Subscribe => Subscribe.StudentID,
                             (Student, Subscribe) => new { Student = Student, Subscribe = Subscribe }).
                     Join(_dbContext.Subjects,
                             Subscribtion => Subscribtion.Subscribe.SubjectID,
                             Subject => Subject.ID,
                             (StudentSubscribtion, Subject) => new SubscribitionDetails
                             {
                                 StudentID = StudentSubscribtion.Student.ID,
                                 StudentName = StudentSubscribtion.Student.Name,
                                 SubjectName = Subject.Name,
                                 SubjectID = Subject.ID,
                                 IsActive = StudentSubscribtion.Subscribe.IsActive,
                                 LevelID = Subject.LevelID,
                                 Date = StudentSubscribtion.Subscribe.DataAndTime
                             }).OrderByDescending(SubDetailes => SubDetailes.Date).ToListAsync();

            return await JoinedResult;
        }

        public async Task InsertAsync(IEnumerable<Subscription> subscriptions)
        {
             await _dbSet.AddRangeAsync(subscriptions);
        }

        public async Task<Subscription> InsertAsync(Subscription subscription)
        {
          var Subscription =   await _dbSet.AddAsync(subscription);
            return Subscription.Entity;
        }

        public async Task InsertAsync(Subscription[] subscriptions)
        {
            await _dbSet.AddRangeAsync(subscriptions);
        }

        public void Remove(IEnumerable<Subscription> subscription)
        {
            _dbSet.RemoveRange(subscription);
        }

        public void Remove(Subscription subscription)
        {
            _dbSet.Remove(subscription);
        }

        public bool Update(Subscription subscription)
        {
            var Result = _dbSet.Update(subscription);
            
            return Result.Entity is null ? false : true; 
        } 
        public void Update(IEnumerable<Subscription> subscription)
        {
           _dbSet.UpdateRange(subscription);
        
        }
    }
}
