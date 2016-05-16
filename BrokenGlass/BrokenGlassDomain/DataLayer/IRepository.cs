using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokenGlassDomain.DataLayer
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id); 
        void Insert(T item);
        void InsertRange(IEnumerable<T> rangeItem);
        void Update(T item);
        void Delete(T item);
        void DeleteById(object id);
        void DeleteRange(IEnumerable<T> rangeItem);
    }
}
