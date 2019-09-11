using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace Services.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        SchoolLibraryDbContext _context;

        public UnitOfWork(SchoolLibraryDbContext context)
        {
            _context = context;
        }

        public T Get<T>(int id) where T : class
        {
            return _context.Set<T>().Find(id);
        }

        //public T Get<T>(Expression<Func<T, bool>> expresion) where T : class
        //{
        //    return _context.Set<T>().Find(expresion);
        //}

        public DbSet<T> GetAll<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Add<T>(T entity) where T : class
        {
            try
            {
                DbEntityEntry entityEntry = _context.Entry(entity);

                if (entityEntry.State != EntityState.Detached)
                {
                    entityEntry.State = EntityState.Added;
                }
                else
                {
                    _context.Set<T>().Add(entity);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update<T>(T entity) where T : class
        {
            try
            {
                DbEntityEntry entityEntry = _context.Entry(entity);

                if (entityEntry.State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                }

                entityEntry.State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete<T>(T entity) where T : class
        {
            try
            {
                DbEntityEntry entityEntry = _context.Entry(entity);

                if (entityEntry.State != EntityState.Deleted)
                {
                    entityEntry.State = EntityState.Deleted;
                }
                else
                {
                    _context.Set<T>().Attach(entity);
                    _context.Set<T>().Remove(entity);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        

        public void Dispose()
        {
            _context.Dispose();
        }

        

        

        
    }
}
