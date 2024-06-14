using Models;

namespace Endpoints
{
    public interface IEndpoint<T> where T : Model
    {
        public Task<IEnumerable<T>> Find();

        public Task<T?> Find(object id);

        public Task<T> Insert(T item);
    }
}
