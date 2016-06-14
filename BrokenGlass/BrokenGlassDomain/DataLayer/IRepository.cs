using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BrokenGlassDomain.DataLayer
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        void Insert(T item);
        void InsertRange(IEnumerable<T> rangeItem);
        void Update(T item);
        void Delete(T item);
        void DeleteById(object id);
        void DeleteRange(IEnumerable<T> rangeItem);
        T Find(Expression<Func<T,bool>> match);
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> match);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> match);

    }
}
