using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Expressions;
using AssetForge.Core;
using AssetForge.Core.Infrastructure;
using AssetForge.Data.Extensions;
using AssetForge.Data.Migrations;
using System.Collections.Concurrent;

namespace AssetForge.Data.Mapping;

/// <summary>
/// Provides an access to entity mapping information
/// </summary>
public static class MappingSchema
{
    #region Fields

    private static ConcurrentDictionary<Type, EntityDescriptor> EntityDescriptors { get; } = new();

    #endregion

    /// <summary>
    /// Returns mapped entity descriptor
    /// </summary>
    /// <param name="entityType">Type of entity</param>
    /// <returns>Mapped entity descriptor</returns>
    public static EntityDescriptor GetEntityDescriptor(Type entityType)
    {
        if (!typeof(BaseEntity).IsAssignableFrom(entityType))
            return null;

        return EntityDescriptors.GetOrAdd(entityType, t =>
        {
            var tableName = NameCompatibilityManager.GetTableName(t);
            var expression = new CreateTableExpression { TableName = tableName };
            var builder = new CreateTableExpressionBuilder(expression, new NullMigrationContext());
            builder.RetrieveTableExpressions(t);

            return new EntityDescriptor
            {
                EntityName = tableName,
                SchemaName = builder.Expression.SchemaName,
                Fields = builder.Expression.Columns.Select(column => new EntityFieldDescriptor
                {
                    Name = column.Name,
                    IsPrimaryKey = column.IsPrimaryKey,
                    IsNullable = column.IsNullable,
                    Size = column.Size,
                    Precision = column.Precision,
                    IsIdentity = column.IsIdentity,
                    Type = column.Type ?? System.Data.DbType.String
                }).ToList()
            };
        });
    }

    /// <summary>
    /// Get or create mapping schema with specified configuration name
    /// </summary>
    public static LinqToDB.Mapping.MappingSchema GetMappingSchema(string configurationName, LinqToDB.DataProvider.IDataProvider mappings)
    {

        if (Singleton<LinqToDB.Mapping.MappingSchema>.Instance is null)
        {
            Singleton<LinqToDB.Mapping.MappingSchema>.Instance = new LinqToDB.Mapping.MappingSchema(configurationName, mappings.MappingSchema);
            Singleton<LinqToDB.Mapping.MappingSchema>.Instance.AddMetadataReader(new FluentMigratorMetadataReader());
        }

        return Singleton<LinqToDB.Mapping.MappingSchema>.Instance;
    }
}
