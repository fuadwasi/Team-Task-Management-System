using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Security;

namespace AssetForge.Services.Security;

/// <summary>
/// Standard permission provider
/// </summary>
public partial class StandardPermissionProvider : IPermissionProvider
{
    //admin area permissions
    public static readonly PermissionRecord AccessAdminPanel = new() { Name = "Access admin area", SystemName = "AccessAdminPanel", Category = "Standard" };
    public static readonly PermissionRecord AllowCustomerImpersonation = new() { Name = "Admin area. Allow Customer Impersonation", SystemName = "AllowCustomerImpersonation", Category = "Customers" };
    public static readonly PermissionRecord ManageAttributes = new() { Name = "Admin area. Manage Attributes", SystemName = "ManageAttributes", Category = "Catalog" };
    public static readonly PermissionRecord ManageCustomers = new() { Name = "Admin area. Manage Customers", SystemName = "ManageCustomers", Category = "Customers" };
    public static readonly PermissionRecord ManageTeam = new() { Name = "Admin area. Manage Team", SystemName = "ManageTeam", Category = "Customers" };
    public static readonly PermissionRecord ManageTeamMember = new() { Name = "Admin area. Manage Team Member", SystemName = "ManageTeamMember", Category = "Customers" };
    public static readonly PermissionRecord ManageTeamTask = new() { Name = "Admin area. Manage Team Task", SystemName = "ManageTeamTask", Category = "Customers" };
    public static readonly PermissionRecord ManageCountries = new() { Name = "Admin area. Manage Countries", SystemName = "ManageCountries", Category = "Configuration" };
    public static readonly PermissionRecord ManageLanguages = new() { Name = "Admin area. Manage Languages", SystemName = "ManageLanguages", Category = "Configuration" };
    public static readonly PermissionRecord ManageSettings = new() { Name = "Admin area. Manage Settings", SystemName = "ManageSettings", Category = "Configuration" };
    public static readonly PermissionRecord ManageTaxSettings = new() { Name = "Admin area. Manage Tax Settings", SystemName = "ManageTaxSettings", Category = "Configuration" };
    public static readonly PermissionRecord ManageShippingSettings = new() { Name = "Admin area. Manage Shipping Settings", SystemName = "ManageShippingSettings", Category = "Configuration" };
    public static readonly PermissionRecord ManageActivityLog = new() { Name = "Admin area. Manage Activity Log", SystemName = "ManageActivityLog", Category = "Configuration" };
    public static readonly PermissionRecord ManageAcl = new() { Name = "Admin area. Manage ACL", SystemName = "ManageACL", Category = "Configuration" };
    public static readonly PermissionRecord ManageSites = new() { Name = "Admin area. Manage Sites", SystemName = "ManageSites", Category = "Configuration" };
    public static readonly PermissionRecord ManageSystemLog = new() { Name = "Admin area. Manage System Log", SystemName = "ManageSystemLog", Category = "Configuration" };
    public static readonly PermissionRecord ManageMaintenance = new() { Name = "Admin area. Manage Maintenance", SystemName = "ManageMaintenance", Category = "Configuration" };
    public static readonly PermissionRecord HtmlEditorManagePictures = new() { Name = "Admin area. HTML Editor. Manage pictures", SystemName = "HtmlEditor.ManagePictures", Category = "Configuration" };
    public static readonly PermissionRecord ManageScheduleTasks = new() { Name = "Admin area. Manage Schedule Tasks", SystemName = "ManageScheduleTasks", Category = "Configuration" };
    public static readonly PermissionRecord ManageAppSettings = new() { Name = "Admin area. Manage App Settings", SystemName = "ManageAppSettings", Category = "Configuration" };

    //public site permissions
    public static readonly PermissionRecord PublicSiteManageTask = new() { Name = "Public site. Allow Manage Task", SystemName = "PublicSiteManageTask", Category = "PublicSite" };
    public static readonly PermissionRecord PublicSiteAllowNavigation = new() { Name = "Public site. Allow navigation", SystemName = "PublicSiteAllowNavigation", Category = "PublicSite" };
    public static readonly PermissionRecord AccessClosedSite = new() { Name = "Public site. Access a closed site", SystemName = "AccessClosedSite", Category = "PublicSite" };

    //Security
    public static readonly PermissionRecord EnableMultiFactorAuthentication = new() { Name = "Security. Enable Multi-factor authentication", SystemName = "EnableMultiFactorAuthentication", Category = "Security" };

    /// <summary>
    /// Get permissions
    /// </summary>
    /// <returns>Permissions</returns>
    public virtual IEnumerable<PermissionRecord> GetPermissions()
    {
        return new[]
        {
            AccessAdminPanel,
            AllowCustomerImpersonation,
            ManageAttributes,
            ManageCustomers,
            ManageCountries,
            ManageLanguages,
            ManageSettings,
            ManageTaxSettings,
            ManageShippingSettings,
            ManageActivityLog,
            ManageAcl,
            ManageSites,
            ManageSystemLog,
            ManageMaintenance,
            HtmlEditorManagePictures,
            ManageScheduleTasks,
            ManageAppSettings,
            PublicSiteAllowNavigation,
            AccessClosedSite,
            EnableMultiFactorAuthentication,
            ManageTeam,
            ManageTeamMember,
            ManageTeamTask,
            PublicSiteManageTask,
        };
    }

    /// <summary>
    /// Get default permissions
    /// </summary>
    /// <returns>Permissions</returns>
    public virtual HashSet<(string systemRoleName, PermissionRecord[] permissions)> GetDefaultPermissions()
    {
        return new HashSet<(string, PermissionRecord[])>
        {
            (
                CustomerDefaults.AdministratorsRoleName,
                new[]
                {
                    AccessAdminPanel,
                    AllowCustomerImpersonation,
                    ManageAttributes,
                    ManageCustomers,
                    ManageCountries,
                    ManageLanguages,
                    ManageSettings,
                    ManageTaxSettings,
                    ManageShippingSettings,
                    ManageActivityLog,
                    ManageAcl,
                    ManageSites,
                    ManageSystemLog,
                    ManageMaintenance,
                    HtmlEditorManagePictures,
                    ManageScheduleTasks,
                    ManageAppSettings,
                    PublicSiteAllowNavigation,
                    AccessClosedSite,
                    EnableMultiFactorAuthentication,
                    ManageTeam,
                    ManageTeamMember,
                    ManageTeamTask,
                    PublicSiteManageTask,
                }
            ),
            (
                CustomerDefaults.ManagerRoleName,
                new[]
                {
                    ManageTeam,
                    ManageTeamMember,
                    ManageTeamTask,
                    PublicSiteManageTask,
                    PublicSiteAllowNavigation
                }
            ),
            (
                CustomerDefaults.GuestsRoleName,
                new[]
                {
                    PublicSiteAllowNavigation
                }
            ),
            (
                CustomerDefaults.RegisteredRoleName,
                new[]
                {
                    PublicSiteManageTask,
                    PublicSiteAllowNavigation,
                    EnableMultiFactorAuthentication
                }
            ),
        };
    }
}