using DAL.Connection_Maper;
using DAL.Interfaces;
using DAL.Model;
using System.Data.SqlClient;

namespace Northwind.DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ConnectionSetting _connectionString;
        private readonly string _tableName;

        public BaseRepository(ConfigurationProvider provider, string tableName)
        {
            provider = new ConfigurationProvider();
            _connectionString = new ConnectionSetting
            {
                Connection = provider.GetConnectionString()
            };
            _tableName = tableName;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = new List<T>();

            using (var connection = new SqlConnection(_connectionString.Connection))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT * FROM Person", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var entity = Maper<T>.MapDataReaderToEntity(reader);
                        entities.Add(entity);
                    }
                }
            }

            return entities;
        }


        public async Task<T> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString.Connection))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand($"SELECT * FROM {_tableName} WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return Maper<T>.MapDataReaderToEntity(reader);
                        }
                    }
                }
            }
            return null;
        }

        public async Task AddAsync(T entity)
        {
            using (var connection = new SqlConnection(_connectionString.Connection))
            {
                await connection.OpenAsync();

                var insertQuery = $"INSERT INTO {_tableName} VALUES (...);"; // Replace '...' with actual values
                using (var command = new SqlCommand(insertQuery, connection))
                {
                    // Set parameters for the insert query based on the properties of the entity
                    // command.Parameters.AddWithValue("@PropertyName", entity.PropertyName);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(int id, T entity)
        {
            using (var connection = new SqlConnection(_connectionString.Connection))
            {
                await connection.OpenAsync();

                var updateQuery = $"UPDATE {_tableName} SET ... WHERE Id = @Id"; // Replace '...' with actual update values
                using (var command = new SqlCommand(updateQuery, connection))
                {
                    // Set parameters for the update query based on the properties of the entity
                    // command.Parameters.AddWithValue("@PropertyName", entity.PropertyName);
                    command.Parameters.AddWithValue("@Id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString.Connection))
            {
                await connection.OpenAsync();

                var deleteQuery = $"DELETE FROM {_tableName} WHERE Id = @Id";
                using (var command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
