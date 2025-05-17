using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Customers;

/// <summary>
/// Represents a customer attribute value model
/// </summary>
public partial record CustomerAttributeValueModel : BaseEntityModel, ILocalizedModel<CustomerAttributeValueLocalizedModel>
{
    #region Ctor

    public CustomerAttributeValueModel()
    {
        Locales = new List<CustomerAttributeValueLocalizedModel>();
    }

    #endregion

    #region Properties

    public int AttributeId { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerAttributes.Values.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerAttributes.Values.Fields.IsPreSelected")]
    public bool IsPreSelected { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerAttributes.Values.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<CustomerAttributeValueLocalizedModel> Locales { get; set; }

    #endregion
}

public partial record CustomerAttributeValueLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.Customers.CustomerAttributes.Values.Fields.Name")]
    public string Name { get; set; }
}