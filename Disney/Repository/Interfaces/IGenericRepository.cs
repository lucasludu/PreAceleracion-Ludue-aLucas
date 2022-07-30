using System.Linq.Expressions;

namespace Disney.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int? id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int? id);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}
