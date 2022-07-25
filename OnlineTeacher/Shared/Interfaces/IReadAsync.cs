using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Threenine.Data.Paging;

namespace OnlineTeacher.Shared.Interfaces
{

    public interface IReadAsync<TEntity> where TEntity : class
    {
        Task<IPaginate<TEntity>> GetAll(int index =0, int Size=20);
    }
}
