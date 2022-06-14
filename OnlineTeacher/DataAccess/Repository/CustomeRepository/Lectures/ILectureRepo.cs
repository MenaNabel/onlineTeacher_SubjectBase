using Microsoft.EntityFrameworkCore.Query;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.ViewModels.Lecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Threenine.Data;

namespace OnlineTeacher.DataAccess.Repository.CustomeRepository.Lectures
{
    public interface ILectureRepo : IRepositoryAsync<Lecture>
    {
        Task<IEnumerable<Lecture>> GetLecturesWithoutFiles(Expression<Func<Lecture, bool>> predicate = null,
            Func<IQueryable<Lecture>, IOrderedQueryable<Lecture>> orderBy = null,
            Func<IQueryable<Lecture>, IIncludableQueryable<Lecture, object>> include = null,
            int index = 0,
            int size = 20,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);
        Task<Lecture> SingleOrDefaultWithoutFile(Expression<Func<Lecture, bool>> predicate = null,
              Func<IQueryable<Lecture>, IOrderedQueryable<Lecture>> orderBy = null,
              Func<IQueryable<Lecture>, IIncludableQueryable<Lecture, object>> include = null,
              bool enableTracking = true,
              bool ignoreQueryFilters = false);
    }
}
