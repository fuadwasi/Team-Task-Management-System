namespace AssetForge.Data.Migrations;

/// <summary>
/// Attribute for schema migration
/// </summary>
///<remarks>
/// This migration will apply right after the migration runner will become available.
/// Do not us dependency injection in migrations that are marked by this attribute,
/// because IoC container not ready yet.
///</remarks>
public partial class SchemaMigrationAttribute : MigrationAttribute
{
    /// <summary>
    /// Initializes a new instance of the AssetForgeSchemaMigrationAttribute class
    /// </summary>
    /// <param name="dateTime">The migration date time string to convert on version</param>
    /// <param name="description">The migration description</param>
    /// <param name="targetMigrationProcess">The target migration process</param>
    public SchemaMigrationAttribute(string dateTime, string description,
        MigrationProcessType targetMigrationProcess = MigrationProcessType.Update) :
        base(dateTime, description, targetMigrationProcess)
    {
        IsSchemaMigration = true;
    }
}