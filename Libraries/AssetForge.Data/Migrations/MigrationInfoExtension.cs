using FluentMigrator.Infrastructure;
using System.Reflection;

namespace AssetForge.Data.Migrations;

public static partial class MigrationInfoExtension
{
    /// <summary>
    /// Gets the flag which indicate whether the migration should be applied into DB on the debug mode
    /// </summary>
    public static bool IsNeedToApplyInDbOnDebugMode(this IMigrationInfo info)
    {
        var applyInDbOnDebugMode = info.Migration.GetType().GetCustomAttribute<MigrationAttribute>()?.ApplyInDbOnDebugMode;

        return applyInDbOnDebugMode ?? true;
    }
}
