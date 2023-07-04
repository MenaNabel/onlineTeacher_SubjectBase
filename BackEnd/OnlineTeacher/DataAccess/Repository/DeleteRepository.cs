using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OnlineTeacher.DataAccess;

namespace Threenine.Data
{
    public class DeleteRepository<T> : IDeleteRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly OnlineExamContext _context;

        public DeleteRepository(OnlineExamContext context)
        {
             _context = context ?? throw new ArgumentException(nameof(context));
             _dbSet = _context.Set<T>();
        }

        public void Delete(T entity)
        {
            try
            {
                _dbSet.Remove(entity);
            }catch(Exception ex)
            {
                throw ex;
            }
          
        }

        public void Delete(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
        }
        public void Delete(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
        public bool commit() {

          return  _context.SaveChanges() > 0 ? true : false;
        
        }
    }
}