using Microsoft.EntityFrameworkCore;
using Models;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Repositories
{
    public class EntityFrameworkRepository<T> : IBaseRepository<T> where T : Model, new()
    {
        protected DbContext _context;

        public EntityFrameworkRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<T> Insert(T entity)
        {
            _context.Set<T>().Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<T>> Find() => await _context.Set<T>().ToListAsync();

        public async Task<T?> Find(object id) => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> FindWith(params Expression<Func<T, Model>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<T?> FindWith(object id, params Expression<Func<T, Model>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            foreach (var include in includes)
                query = query.Include(include);

            var primaryKeyProperty = typeof(T).GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(KeyAttribute)));

            if (primaryKeyProperty == null)
                throw new InvalidOperationException($"Entidade {typeof(T).Name} não tem chave primária definida");

            var parameter = Expression.Parameter(typeof(T), "entity");

            var primaryKeyAccess = Expression.Property(parameter, primaryKeyProperty);

            var convertedId = Convert.ChangeType(id, primaryKeyProperty.PropertyType);

            var comparison = Expression.Equal(primaryKeyAccess, Expression.Constant(convertedId));

            var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);

            return await query.Where(lambda).FirstOrDefaultAsync();
        }

        public bool Exists(object id) => (_context.Set<T>()?.Any(e => e.GetPrimaryKeyValue() == id)).GetValueOrDefault();
    }
}
