using AssetForge.Core.Caching;

namespace AssetForge.Services.Customers;

/// <summary>
/// Represents default values related to customer services
/// </summary>
public static partial class AssetForgeCustomerServicesDefaults
{
    /// <summary>
    /// Gets a password salt key size
    /// </summary>
    public static int PasswordSaltKeySize => 5;

    /// <summary>
    /// Gets a max username length
    /// </summary>
    public static int CustomerUsernameLength => 100;

    /// <summary>
    /// Gets a default hash format for customer password
    /// </summary>
    public static string DefaultHashedPasswordFormat => "SHA512";

    /// <summary>
    /// Gets default prefix for customer
    /// </summary>
    public static string CustomerAttributePrefix => "customer_attribute_";

    #region Caching defaults

    #region Customer

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : system name
    /// </remarks>
    public static CacheKey CustomerBySystemNameCacheKey => new("AssetForge.customer.bysystemname.{0}");

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : customer GUID
    /// </remarks>
    public static CacheKey CustomerByGuidCacheKey => new("AssetForge.customer.byguid.{0}");

    #endregion

    #region Customer roles

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : system name
    /// </remarks>
    public static CacheKey CustomerRolesBySystemNameCacheKey => new("AssetForge.customerrole.bysystemname.{0}", CustomerRolesBySystemNamePrefix);

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string CustomerRolesBySystemNamePrefix => "AssetForge.customerrole.bysystemname.";

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : customer identifier
    /// </remarks>
    public static CacheKey CustomerRolesCacheKey => new("AssetForge.customer.customerrole.{0}", CustomerCustomerRolesPrefix);

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string CustomerCustomerRolesPrefix => "AssetForge.customer.customerrole.";

    #endregion

    #region Addresses

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : customer identifier
    /// </remarks>
    public static CacheKey CustomerAddressesCacheKey => new("AssetForge.customer.addresses.{0}", CustomerAddressesPrefix);

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : customer identifier
    /// {1} : address identifier
    /// </remarks>
    public static CacheKey CustomerAddressCacheKey => new("AssetForge.customer.addresses.{0}-{1}", CustomerAddressesByCustomerPrefix, CustomerAddressesPrefix);

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string CustomerAddressesPrefix => "AssetForge.customer.addresses.";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    /// <remarks>
    /// {0} : customer identifier
    /// </remarks>
    public static string CustomerAddressesByCustomerPrefix => "AssetForge.customer.addresses.{0}";

    #endregion

    #region Customer password

    /// <summary>
    /// Gets a key for caching current customer password lifetime
    /// </summary>
    /// <remarks>
    /// {0} : customer identifier
    /// </remarks>
    public static CacheKey CustomerPasswordLifetimeCacheKey => new("AssetForge.customerpassword.lifetime.{0}");

    #endregion

    #endregion
}