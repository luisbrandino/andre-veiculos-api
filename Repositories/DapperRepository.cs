using Dapper;
using Microsoft.Data.SqlClient;
using Models;
using System.Linq.Expressions;
using System.Reflection;
using static Dapper.SqlMapper;

namespace Repositories
{
    public class DapperRepository<T> : IBaseRepository<T> where T : Model, new()
    {
        private string ConnectionString = "Server=127.0.0.1;Database=db_andre_veiculos_api;TrustServerCertificate=Yes;User Id=sa;Password=SqlServer2019!";
        private SqlConnection _connection;

        public DapperRepository()
        {
            _connection = new SqlConnection(ConnectionString);
        }

        public bool Exists(object id)
        {
            var model = new T();
            return true;
        }

        public async Task<IEnumerable<T>> Find()
        {
            var table = new T().Table();
            var columns = new T().GetColumns();

            var query = $"select {string.Join(',', columns)} from {table}";
            List<T> entities = new();

            var rows = await _connection.QueryAsync(query);
            
            foreach (var row in rows)
            {
                var model = new T();
                model.FromRaw(RowToDictionary(row));
                entities.Add(model);
            }

            return entities;
        }

        public async Task<T?> Find(object id)
        {
            var model = new T();
            var table = model.Table();
            var columns = model.GetColumns();
            var primaryKeyName = model.GetMappedPrimaryKeyName();

            var query = $"select {string.Join(',', columns)} from {table} where {primaryKeyName} = @Id";

            var result = await _connection.QueryFirstOrDefaultAsync(query, new { Id = id });

            if (result == null)
                return null;

            var raw = RowToDictionary(result);

            model.FromRaw(raw);

            return model;
        }

        public async Task<IEnumerable<T>> FindWith(params Expression<Func<T, Model>>[] includes)
        {
            var entities = await Find();

            foreach (var entity in entities)
                await AddRelations(entity, includes);

            return entities;
        }

        public async Task<T?> FindWith(object id, params Expression<Func<T, Model>>[] includes)
        {
            var entity = await Find(id);

            if (entity == null)
                return null;

            await AddRelations(entity, includes);

            return entity;
        }

        private async Task AddRelations(T entity, params Expression<Func<T, Model>>[] includes)
        {
            foreach (var include in includes)
            {
                var propertyInfo = (include.Body as MemberExpression)?.Member as PropertyInfo;

                if (propertyInfo == null)
                    continue;

                var relatedModel = (Model)propertyInfo.GetValue(entity);

                var table = relatedModel.Table();
                var columns = relatedModel.GetColumns();
                var primaryKeyName = relatedModel.GetMappedPrimaryKeyName();

                var query = $"select {string.Join(',', columns)} from {table} where {primaryKeyName} = @Id";

                var result = await _connection.QueryFirstOrDefaultAsync(query, new { Id = relatedModel.GetPrimaryKeyValue() });

                if (result == null)
                {
                    propertyInfo.SetValue(entity, null);
                    continue;
                }

                var raw = RowToDictionary(result);
                relatedModel.FromRaw(raw);

                propertyInfo.SetValue(entity, relatedModel);
            }
        }

        public async Task<T> Insert(T entity)
        {
            var table = entity.Table();
            var columns = entity.GetColumns().Where(c => !entity.IsAutoIncrement(c)).ToList();
            var columnNames = string.Join(',', columns);
            var columnParameters = string.Join(',', columns.Select(c => "@" + c));

            var query = $"insert into {table} ({columnNames}) values ({columnParameters})";

            await _connection.ExecuteAsync(query, entity.Raw());

            object id = entity.GetPrimaryKeyValue();

            if (entity.IsPrimaryKeyAutoIncrement())
                id = await _connection.ExecuteScalarAsync<int>($"select IDENT_CURRENT('{table}')");

            return await Find(id);
        }

        private static Dictionary<string, object> RowToDictionary(dynamic row)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var property in (IDictionary<string, object>)row)
                dictionary[property.Key] = property.Value;

            return dictionary;
        }
    }
}
