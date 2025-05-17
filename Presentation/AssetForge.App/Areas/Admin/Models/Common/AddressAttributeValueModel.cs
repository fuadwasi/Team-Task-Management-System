using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Common;

/// <summary>
/// Represents an address attribute value model
/// </summary>
public partial record AddressAttributeValueModel : BaseEntityModel, ILocalizedModel<AddressAttributeValueLocalizedModel>
{
    #region Ctor

    public AddressAttributeValueModel()
    {
        Locales = new List<AddressAttributeValueLocalizedModel>();
    }

    #endregion

    #region Properties

    public int AttributeId { get; set; }

    [ResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.IsPreSelected")]
    public bool IsPreSelected { get; set; }

    [ResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<AddressAttributeValueLocalizedModel> Locales { get; set; }

    #endregion
}

public partial record AddressAttributeValueLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.Name")]
    public string Name { get; set; }
}