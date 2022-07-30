using Disney.Data;
using Disney.Repository.Interfaces;
using System.Linq.Expressions;

namespace Disney.Repository.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Find
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }
        #endregion

        #region Get by id
        public T GetById(int? id)
        {
            return _context.Set<T>().Find(id);
        }
        #endregion

        #region Delete
        public void Delete(int? id)
        {
            var entity = GetById(id);
            if(entity == null)
            {
                throw new Exception("Object Not Found");
            } 
            else
            {
                _context.Set<T>().Remove(entity);
            }
        }
        #endregion

        #region Get All
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        #endregion

        #region Insert
        public void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        #endregion

        #region Update
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        #endregion
 
    }
}
