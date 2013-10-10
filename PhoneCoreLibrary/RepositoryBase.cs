using System;
using System.Data.Linq;
using System.Linq;
using Cdel.PhoneFramework;

namespace PhoneCoreLibrary
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TEntity">EntityBase</typeparam>
    public class RepositoryBase<TKey, TEntity> : IRepository<TEntity> where TEntity : EntityBase<TKey>
    {
        private readonly DataContext _database;

        public RepositoryBase(DataContext database)
        {
            if (database == null)
                throw new ArgumentNullException("database");

            _database = database;
        }

        public void Add(TEntity item)
        {
            _database.GetTable<TEntity>().InsertOnSubmit(item);
        }

        public TEntity Get<T>(T id)
        {
            return _database.GetTable<TEntity>().Select(t => t).SingleOrDefault(t => t.Id.Equals(id));
        }

        public virtual System.Collections.Generic.IEnumerable<TEntity> GetAll(int pageIndex, int pageSize)
        {
            return _database.GetTable<TEntity>()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }

        public System.Collections.Generic.IEnumerable<TEntity> GetAll()
        {
            return _database.GetTable<TEntity>();
        }

        public System.Collections.Generic.IEnumerable<TEntity> GetFiltered(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, int pageIndex, int pageSize)
        {
            return _database.GetTable<TEntity>().Where(filter.Compile())
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }

        public System.Collections.Generic.IEnumerable<TEntity> GetFiltered(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return _database.GetTable<TEntity>().Where(filter.Compile());
        }

        public void Modify(TEntity item)
        {
            Save();
        }

        public void Remove(TEntity item)
        {
            _database.GetTable<TEntity>().DeleteOnSubmit(item);
        }

        public void RemoveAll()
        {
            _database.GetTable<TEntity>().DeleteAllOnSubmit(_database.GetTable<TEntity>());
        }

        public void Save()
        {
            _database.SubmitChanges();
        }
    }
}