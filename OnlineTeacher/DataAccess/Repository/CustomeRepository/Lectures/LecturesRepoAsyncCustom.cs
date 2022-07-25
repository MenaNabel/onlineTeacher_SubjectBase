using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OnlineTeacher.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Threenine.Data;
using Threenine.Data.Paging;

namespace OnlineTeacher.DataAccess.Repository.CustomeRepository.Lectures
{
    public class LecturesRepoAsyncCustom : RepositoryAsync<Lecture>, ILectureRepo
    {
        public LecturesRepoAsyncCustom(OnlineExamContext context):
            base(context)
        {
                
        }
        public  IPaginate<Lecture> GetLecturesWithoutFiles(Expression<Func<Lecture, bool>> predicate = null,
            Func<IQueryable<Lecture>, IOrderedQueryable<Lecture>> orderBy = null,
            Func<IQueryable<Lecture>, IIncludableQueryable<Lecture, object>> include = null,
            int index = 0,
            int size = 20,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {

            IQueryable<Lecture> query = _dbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).ToPaginate(index, size);
           

             return query.Select(
                l => new Lecture
                {
                    DateTime = l.DateTime,
                    Description = l.Description,
                    FileName = l.FileName,
                    ID = l.ID,
                    IsAppear = l.IsAppear,
                    IsFree = l.IsFree,
                    LectureID = l.LectureID,
                    LectureLink = l.LectureLink,
                    Month = l.Month,
                    Name = l.Name,
                    previousLecture = l.previousLecture,
                    Subject = l.Subject,
                    SubjectID = l.SubjectID,
                    Type = l.Type
                }
            ).ToPaginate(index , size);
        }

        public async Task<Lecture> SingleOrDefaultWithoutFile(Expression<Func<Lecture, bool>> predicate = null,
            Func<IQueryable<Lecture>, IOrderedQueryable<Lecture>> orderBy = null,
            Func<IQueryable<Lecture>, IIncludableQueryable<Lecture, object>> include = null,
            bool enableTracking = true,
            bool ignoreQueryFilters = false)
        {
            IQueryable<Lecture> query = _dbSet;

            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            if (ignoreQueryFilters) query = query.IgnoreQueryFilters();

            if (orderBy != null) 
              return await orderBy(query).Select(
                l => new Lecture
                {
                    DateTime = l.DateTime,
                    Description = l.Description,
                    FileName = l.FileName,
                    ID = l.ID,
                    IsAppear = l.IsAppear,
                    IsFree = l.IsFree,
                    LectureID = l.LectureID,
                    LectureLink = l.LectureLink,
                    Month = l.Month,
                    Name = l.Name,
                    previousLecture = l.previousLecture,
                    Subject = l.Subject,
                    SubjectID = l.SubjectID,
                    Type = l.Type
                }
            ).FirstOrDefaultAsync();

            return await query.Select(
                l => new Lecture
                {
                    DateTime = l.DateTime,
                    Description = l.Description,
                    FileName = l.FileName,
                    ID = l.ID,
                    IsAppear = l.IsAppear,
                    IsFree = l.IsFree,
                    LectureID = l.LectureID,
                    LectureLink = l.LectureLink,
                    Month = l.Month,
                    Name = l.Name,
                    previousLecture = l.previousLecture,
                    Subject = l.Subject,
                    SubjectID = l.SubjectID,
                    Type = l.Type
                }
            ).FirstOrDefaultAsync();
        }
    }
}
