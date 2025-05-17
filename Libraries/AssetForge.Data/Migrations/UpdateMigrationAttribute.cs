using AssetForge.Core;

namespace AssetForge.Data.Migrations;

/// <summary>
/// Attribute for update migration
/// </summary>
public partial class UpdateMigrationAttribute : MigrationAttribute
{
    /// <summary>
    /// Initializes a new instance of the AssetForgeUpdateMigrationAttribute class
    /// </summary>
    /// <param name="dateTime">The migration date time string to convert on version</param>
    /// <param name="AssetForgeVersion">AssetForge full version</param>
    /// <param name="migrationType">The migration type</param>
    public UpdateMigrationAttribute(string dateTime, string ixVersion, UpdateMigrationType migrationType) :
        base(dateTime, ixVersion, migrationType, MigrationProcessType.Update)
    {
        ApplyInDbOnDebugMode = !_config.AssetForgeVersion.Equals(AssetForgeVersion.CURRENT_VERSION, StringComparison.CurrentCultureIgnoreCase);
    }
}