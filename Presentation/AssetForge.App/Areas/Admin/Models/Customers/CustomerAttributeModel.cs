using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer attribute model
/// </summary>
public partial record CustomerAttributeModel : BaseEntityModel, ILocalizedModel<CustomerAttributeLocalizedModel>
{
    #region Ctor

    public CustomerAttributeModel()
    {
        Locales = new List<CustomerAttributeLocalizedModel>();
        CustomerAttributeValueSearchModel = new CustomerAttributeValueSearchModel();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.IsRequired")]
    public bool IsRequired { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.AttributeControlType")]
    public int AttributeControlTypeId { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.AttributeControlType")]
    public string AttributeControlTypeName { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<CustomerAttributeLocalizedModel> Locales { get; set; }

    public CustomerAttributeValueSearchModel CustomerAttributeValueSearchModel { get; set; }

    #endregion
}

public partial record CustomerAttributeLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerAttributes.Fields.Name")]
    public string Name { get; set; }
}