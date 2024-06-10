using Models;
using System.Linq.Expressions;

namespace Repositories
{
    public interface IBaseRepository<T> where T : Model, new()
    {
        public Task<T> Insert(T entity);

        public Task<IEnumerable<T>> Find();

        public Task<T?> Find(object id);

        public Task<IEnumerable<T>> FindWith(params Expression<Func<T, Model>>[] includes);

        public Task<T?> FindWith(object id, params Expression<Func<T, Model>>[] includes);

        public bool Exists(object id);
    }
}
