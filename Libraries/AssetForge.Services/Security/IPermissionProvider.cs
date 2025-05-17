using AssetForge.Core.Domain.Security;

namespace AssetForge.Services.Security;

/// <summary>
/// Permission provider
/// </summary>
public partial interface IPermissionProvider
{
    /// <summary>
    /// Get permissions
    /// </summary>
    /// <returns>Permissions</returns>
    IEnumerable<PermissionRecord> GetPermissions();

    /// <summary>
    /// Get default permissions
    /// </summary>
    /// <returns>Default permissions</returns>
    HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions();
}