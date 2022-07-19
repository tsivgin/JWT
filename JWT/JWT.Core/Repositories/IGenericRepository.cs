using JWT.Core.Entity;
using System.Linq.Expressions;

namespace JWT.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        T Add(T entity);

        T[] AddRange(params T[] entities);

        Task<T> AddAsync(T entity, bool save = true, bool isActive = true);

        int Count();

        Task<int> CountAsync();

        void Delete(T entity);

        Task<int> DeleteAsync(T entity, bool save = false);

        Task HardDeleteAsync(T entity);

        void GenericRepositoryDispose();

        T Find(Expression<Func<T, bool>> predicate);

        ICollection<T> FindAll(Expression<Func<T, bool>> predicate);
        ICollection<T> FindAll(Expression<Func<T, bool>> predicate, int pageIndex, int offset);

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate, int pageIndex, int offset);

        Task<T> FindAsync(Expression<Func<T, bool>> predicate);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate, int pageIndex, int offset);
        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        T Get(long id); 

        T Get(int id, params Expression<Func<T, object>>[] includeProperties);

        T Get(long id, params Expression<Func<T, object>>[] includeProperties);


        IQueryable<T> GetAll();

        Task<ICollection<T>> GetAllAsync();

        Task<ICollection<T>> GetAllAsNoTrackingAsync();

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeProperties);
        Task<IQueryable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);


        Task<T> GetAsync(int id);

        Task<T> GetAsync(long id);

        void Save();

        Task<int> SaveAsync();

        T Update(T entity, bool save = true);

        Task<T> UpdateAsync(T entity, bool save = false);

        void UpdateRange(List<T> entities);

        void RemoveRange(List<T> entities);
    }
}