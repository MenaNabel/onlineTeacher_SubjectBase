
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Interfaces
{
    public interface IGet<TEntity> where TEntity : class 
    {
        Task<TEntity> Get(int ID);
    }
}
