using System;
using System.Linq.Expressions;

namespace ShopM4_DataMigrations.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        T Find(int id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true
        );

        T FirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true
        );

        void Add(T item);

        void Remove(T item);

        void Remove(IEnumerable<T> items);

        void Save();
    }
}

