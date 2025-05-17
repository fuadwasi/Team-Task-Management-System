using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Security;

namespace AssetForge.Data.Mapping;

/// <summary>
/// Base instance of backward compatibility of table naming
/// </summary>
public partial class BaseNameCompatibility : INameCompatibility
{
    public Dictionary<Type, string> TableNames => new()
    {

        { typeof(PermissionRecordCustomerRoleMapping), "PermissionRecord_Role_Mapping" },
    };

    public Dictionary<(Type, string), string> ColumnName => new()
    {
        { (typeof(Customer), "BillingAddressId"), "BillingAddress_Id" },
        { (typeof(Customer), "ShippingAddressId"), "ShippingAddress_Id" },

        { (typeof(PermissionRecordCustomerRoleMapping), "PermissionRecordId"), "PermissionRecord_Id" },
        { (typeof(PermissionRecordCustomerRoleMapping), "CustomerRoleId"), "CustomerRole_Id" },

    };
}