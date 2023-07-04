using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Interfaces
{
 
    public interface IInsertAsync<T> where T : class
    {
       Task<T> Add(T entity);
       Task<IEnumerable<T>> AddRange(IEnumerable<T> Collection);
    }
}
