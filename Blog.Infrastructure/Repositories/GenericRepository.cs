using Blogs.Infrastructure.Extensions;
using Blogs.Service.Configurations;
using Blogs.Service.Entities;
using Blogs.Service.RepositoryInterfaces;
using Npgsql;
using NpgsqlTypes;
using System.Data.Common;

namespace Blogs.Infrastructure.Repositories;

public abstract class GenericRepository<T>(IDbConnectionFactory connectionFactory, string tableName) : IGenericRepository<T> where T : BasicEntity, new()
{
    protected readonly IDbConnectionFactory ConnectionFactory = connectionFactory;

    /// <summary>
    /// Метод обеспечивает маппинг из Readera(данных sql) в объект определенного класса
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected abstract T MapResultToEntity(DbDataReader reader);
 
    /// <summary>
    /// Метод обеспечивает маппинг данных из объкта определенного класса в словарь, в котором:
    /// Ключем выступает кортеж из имя колонки в талице и ее тип данных
    /// Значением выступает значение которое хотим сохранить в БД
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    protected abstract IDictionary<(string FieldName, NpgsqlDbType FieldType), string> MapRequestValuesFromEntity(T entity);

    /// <summary>
    /// Метод для решения проблемы инициализации ячейки типа Uuid строковым значением
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    protected object ConvertValueToDbType(string value, NpgsqlDbType type)
    {
        return type switch
        {
            NpgsqlDbType.Uuid => Guid.Parse(value),
            _ => value
        };
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken)
    {
        var values = MapRequestValuesFromEntity(entity);

        var columnsNames = string.Join(", ", values.Keys.Select(key => $"\"{key.FieldName}\""));
        var columnsValuesAliases = string.Join(", ", values.Keys.Select(key => $"@{key.FieldName}"));

        var sql = $"""INSERT INTO "{tableName}" ({columnsNames}) VALUES ({columnsValuesAliases})""";

        using var connection = await connectionFactory.CreateConnectionAsync();
        using var command = new NpgsqlCommand(sql, connection);

        ApplyParametersValues(values, command);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var sql = $"DELETE FROM {tableName} WHERE 'Id' = @id";

        using var connection = await connectionFactory.CreateConnectionAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("id", id);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(PaginationSettings paginationSettings, CancellationToken cancellationToken)
    {
        var sql = $"""SELECT * FROM "{tableName}" LIMIT {paginationSettings.PageSize} OFFSET {paginationSettings.PageSize * (paginationSettings.PageNumber - 1)}""";

        using var connection = await connectionFactory.CreateConnectionAsync();
        using var command = new NpgsqlCommand(sql, connection);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var results = new List<T>();

        while (await reader.ReadAsync(cancellationToken))
        {
            results.Add(MapResultToEntity(reader));
        }
        return results;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var sql = $"""SELECT * FROM "{tableName}" WHERE "Id" = @id""";

        using var connection = await connectionFactory.CreateConnectionAsync();
        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("id", id);

        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return MapResultToEntity(reader);
        }

        return null;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var propertiesWithValues = MapRequestValuesFromEntity(entity);

        var updateValuesQueryPart = string.Join(", ", propertiesWithValues.Select(v => $"\"{v.Key.FieldName}\" = @{v.Key.FieldName}"));
        var sql = $"""UPDATE "{tableName}" SET {updateValuesQueryPart} WHERE "Id" = @id """;

        using var connection = await connectionFactory.CreateConnectionAsync();
        using var command = new NpgsqlCommand(sql, connection);

        command.Parameters.AddWithValue("id", entity.Id);
        ApplyParametersValues(propertiesWithValues, command);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private void ApplyParametersValues(
        IDictionary<(string FieldName, NpgsqlDbType FieldType), string> propertiesWithValues,
        NpgsqlCommand command)
    {
        foreach (var value in propertiesWithValues)
        {
            command.Parameters.AddWithValue(
                value.Key.FieldName,
                value.Key.FieldType,
                ConvertValueToDbType(value.Value, value.Key.FieldType)
            );
        }
    }
}