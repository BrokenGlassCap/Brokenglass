using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokenGlassDomain.DataLayer
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private BROKEN_GLASSEntities m_context;
        private DbSet<T> m_dbSet;
        public GenericRepository(BROKEN_GLASSEntities context)
        {
            m_context = context;
            m_dbSet = m_context.Set<T>();
        }

        public void Delete(T item)
        {
            m_dbSet.Remove(item);
        }

        public void DeleteById(object id)
        {
            var deleteItem = m_dbSet.Find(id);
            if (deleteItem == null) throw new NullReferenceException(string.Format(
                "При удаление объекта {0} c id - {1}, данный объект в БД не найден.",typeof(T),id));

            m_dbSet.Remove(deleteItem);
        }

        public void DeleteRange(IEnumerable<T> rangeItem)
        {
            m_dbSet.RemoveRange(rangeItem);
        }

        public IEnumerable<T> GetAll()
        {
            return m_dbSet.AsEnumerable();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await m_dbSet.ToListAsync<T>();
        }

        public T GetById(object id)
        {
            return m_dbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await m_dbSet.FindAsync(id);
        }

        public void Insert(T item)
        {
            m_dbSet.Add(item);
        }
        public T InsertAndreturn(T item)
        {
            m_dbSet.Add(item);
            return item;
        }

        public void InsertRange(IEnumerable<T> rangeItem)
        {
            m_dbSet.AddRange(rangeItem);
        }

        public void Update(T item)
        {
            m_dbSet.Attach(item);
            m_context.Entry(item).State = EntityState.Modified;
        }
    }
}
