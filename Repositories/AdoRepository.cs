using Dapper;
using Microsoft.Data.SqlClient;
using Models;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repositories
{
    public class AdoRepository<T> : IBaseRepository<T> where T : Model, new()
    {
        private string ConnectionString = "Server=127.0.0.1;Database=db_andre_veiculos_api;TrustServerCertificate=Yes;User Id=sa;Password=SqlServer2019!";
        private SqlConnection _connection;

        public AdoRepository()
        {
            _connection = new SqlConnection(ConnectionString);
        }

        public async Task<IEnumerable<T>> Find()
        {
            var table = new T().Table();
            var columns = new T().GetColumns();

            var query = $"select {string.Join(',', columns)} from {table}";
            List<T> entities = new();

            var rows = DataTable(query);

            foreach (DataRow row in rows)
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

            var results = DataTable(query, new { Id = id });

            if (results == null)
                return null;

            var result = results[0];

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

                var results = DataTable(query, new { Id = relatedModel.GetPrimaryKeyValue() });

                if (results == null)
                {
                    propertyInfo.SetValue(entity, null);
                    continue;
                }

                var result = results[0];

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

            NonQuery(query, entity.Raw());

            object? id = entity.GetPrimaryKeyValue();

            if (entity.IsPrimaryKeyAutoIncrement())
                id = Scalar<int>($"select IDENT_CURRENT('{table}')");

            if (id == null)
                return entity;

            return await Find(id);
        }

        private static Dictionary<string, object> RowToDictionary(DataRow row)
        {
            var dictionary = new Dictionary<string, object>();

            return row.Table.Columns
              .Cast<DataColumn>()
              .ToDictionary(c => c.ColumnName, c => row[c]);
        }

        private SqlCommand Command(string query, Dictionary<string, object>? data = null)
        {
            SqlCommand command = new SqlCommand(query);

            if (data == null)
                return command;

            foreach (var entry in data)
                command.Parameters.AddWithValue(entry.Key, entry.Value);

            return command;
        }

        private SqlCommand Command(string query, object? data = null)
        {
            SqlCommand command = new SqlCommand(query);

            if (data == null)
                return command;

            var properties = data.GetType().GetProperties();

            foreach (var property in properties)
                command.Parameters.AddWithValue(property.Name, property.GetValue(data));

            return command;
        }

        private T? Scalar<T>(string query, object? data = null)
        {
            var command = Command(query, data);

            command.Connection = _connection;

            object result = Command(query, data).ExecuteScalar();

            if (result == null)
                return default(T);

            return (T)result;
        }

        private int NonQuery(string query, Dictionary<string, object>? data = null)
        {
            var command = Command(query, data);
            _connection.Open();
            command.Connection = _connection;
             return command.ExecuteNonQuery();
        }

        private int NonQuery(string query, object? data = null)
        {
            var command = Command(query, data);
            _connection.Open();
            command.Connection = _connection;
            return command.ExecuteNonQuery();
        }

        private DataRowCollection DataTable(string query, object? data = null)
        {
            var command = Command(query, data);

            command.Connection = _connection;

            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable result = new();

                adapter.Fill(result);

                if (result.Rows.Count <= 0)
                    return null;

                return result.Rows;
            }
        }

    }
}
