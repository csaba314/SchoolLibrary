using System;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Services.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        T Get<T>(int id) where T : class;
        //T Get<T>(Expression<Func<T, bool>> expresion) where T : class;
        DbSet<T> GetAll<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Commit();
    }
}