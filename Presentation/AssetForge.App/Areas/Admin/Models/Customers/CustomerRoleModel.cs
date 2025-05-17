using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer role model
/// </summary>
public partial record CustomerRoleModel : BaseEntityModel
{
    #region Properties

    [ResourceDisplayName("Admin.Customers.CustomerRoles.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerRoles.Fields.Active")]
    public bool Active { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerRoles.Fields.IsSystemRole")]
    public bool IsSystemRole { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerRoles.Fields.SystemName")]
    public string SystemName { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerRoles.Fields.EnablePasswordLifetime")]
    public bool EnablePasswordLifetime { get; set; }

    #endregion
}