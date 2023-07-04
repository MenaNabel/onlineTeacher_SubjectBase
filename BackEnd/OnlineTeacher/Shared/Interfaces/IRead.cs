using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Interfaces
{
    public interface IRead<TEntity> where TEntity : class 
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Filter(Func<TEntity , bool> FilterCondition);
    }
}
