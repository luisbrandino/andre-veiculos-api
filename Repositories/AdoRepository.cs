using Models;
using System.Linq.Expressions;

namespace Repositories
{
    public class AdoRepository<T> : IBaseRepository<T> where T : Model, new()
    {
        public bool Exists(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> Find()
        {
            throw new NotImplementedException();
        }

        public Task<T?> Find(object id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindWith(params Expression<Func<T, Model>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<T?> FindWith(object id, params Expression<Func<T, Model>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<T> Insert(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
