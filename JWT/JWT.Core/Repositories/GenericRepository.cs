using JWT.Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
            _context.ChangeTracker.Context.Database.SetCommandTimeout(2400);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }
        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                queryable = queryable.Include<T, object>(includeProperty);

            return queryable;
        }
        public virtual async Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = (await GetAllAsync()).AsQueryable();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                queryable = queryable.Include<T, object>(includeProperty);

            return queryable;
        }
        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
            return query;
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, int pageIndex, int offset)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate).Skip(pageIndex * offset).Take(offset);
            return query;
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetAllAsNoTrackingAsync()
        {

            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }


        public virtual T Get(long id)
        {
            return _context.Set<T>().FirstOrDefault(t => t.Id == id && t.IsActive);
        }

        public virtual T Get(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            var res = true;
            T item = null;

            while (res)
            {
                try
                {
                    var queryable = _context.Set<T>().AsQueryable();

                    foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                        queryable = queryable.Include<T, object>(includeProperty);
                    res = false;

                    item = queryable.FirstOrDefault(t => t.Id == id);
                }
                catch (Exception)
                {
                    res = true;
                    item = null;
                }
            }
            return item;
        }

        public virtual T Get(long id, params Expression<Func<T, object>>[] includeProperties)
        {
            var res = true;
            T item = null;

            while (res)
            {
                try
                {
                    var queryable = _context.Set<T>().AsQueryable();

                    foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                        queryable = queryable.Include<T, object>(includeProperty);
                    res = false;

                    item = queryable.FirstOrDefault(t => t.Id == id);
                }
                catch (Exception)
                {
                    res = true;
                    item = null;
                }
            }
            return item;
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(t => t.Id == id);
        }

        public virtual async Task<T> GetAsync(long id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(t => t.Id == id);
        }

        public virtual T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public virtual T[] AddRange(params T[] entities)
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            _context.ChangeTracker.Context.Database.SetCommandTimeout(180);
            _context.Set<T>().AddRange(entities);
            return entities;
        }

        public virtual async Task<T> AddAsync(T entity, bool save = true, bool isActive = true)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = isActive;

            await _context.Set<T>().AddAsync(entity);

            if (save)
                await _context.SaveChangesAsync();

            return entity;
        }

        public virtual T Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().SingleOrDefault(predicate);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public ICollection<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }
        public ICollection<T> FindAll(Expression<Func<T, bool>> predicate, int pageIndex, int offset)
        {
            return _context.Set<T>().Where(predicate).Skip(offset * pageIndex).Take(offset).ToList();
        }
        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate, int pageIndex, int offset)
        {
            return await _context.Set<T>().Where(predicate).Skip(offset * pageIndex).Take(offset).ToListAsync();
        }
        public virtual void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;

            Update(entity);
        }

        public virtual async Task<int> DeleteAsync(T entity, bool save = false)
        {
            if (entity == null)
                return 0;

            T exist = await _context.Set<T>().FindAsync(entity.Id);

            if (exist != null)
            {
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;

                _context.Entry(exist).State = EntityState.Modified; 
                _context.Entry(exist).CurrentValues.SetValues(entity);


                if (save)
                    return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public virtual async Task HardDeleteAsync(T entity)
        {
            if (entity == null)
                return;

            _context.Set<T>().Remove(entity);

            await _context.SaveChangesAsync();
        }

        public virtual T? Update(T entity, bool save = true)
        {
            if (entity == null)
                return null;

            T exist = _context.Set<T>().Find(entity.Id);

            if (exist != null)
            {
                _context.Entry(exist).State = EntityState.Modified; 
                _context.Entry(exist).CurrentValues.SetValues(entity);

                if (save)
                {
                    entity.UpdatedAt = DateTime.UtcNow;

                    _context.SaveChanges();
                }
            }

            return exist;
        }

        public virtual async Task<T?> UpdateAsync(T entity, bool save = false)
        {
            if (entity == null)
                return null;

            T exist = await _context.Set<T>().FindAsync(entity.Id);
            if (exist != null)
            {
                if (exist.UpdatedById != null)
                {
                    entity.UpdatedById = exist.UpdatedById;
                    entity.UpdatedAt = DateTime.UtcNow;
                }

                _context.Entry(exist).State = EntityState.Modified; /
                _context.Entry(exist).CurrentValues.SetValues(entity);

                if (save) await _context.SaveChangesAsync();
            }
            return exist;
        }

        public virtual void UpdateRange(List<T> entities)
        {
            if (entities == null) return;

            foreach (var entity in entities)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }

            _context.Set<T>().UpdateRange(entities);
        }

        public virtual void RemoveRange(List<T> entities)
        {
            if (entities == null) return;

            _context.Set<T>().RemoveRange(entities);
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        public virtual async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var queryable = _context.Set<T>().Where(predicate);

            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);

            return await queryable.ToListAsync();
        }

        public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate, int pageIndex, int offset)
        {
            return await _context.Set<T>().Where(predicate).Skip(pageIndex * offset).Take(offset).ToListAsync();
        }

        private bool disposed = false;
        protected virtual void GenericRepositoryDispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    _context.Dispose();

                this.disposed = true;
            }
        }

        public void GenericRepositoryDispose()
        {
            GenericRepositoryDispose(true);
            GC.SuppressFinalize(this);
        }
    }
}