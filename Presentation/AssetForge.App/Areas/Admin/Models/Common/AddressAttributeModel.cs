using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;

namespace AssetForge.App.Areas.Admin.Models.Common;

/// <summary>
/// Represents an address attribute model
/// </summary>
public partial record AddressAttributeModel : BaseEntityModel, ILocalizedModel<AddressAttributeLocalizedModel>
{
    #region Ctor

    public AddressAttributeModel()
    {
        Locales = new List<AddressAttributeLocalizedModel>();
        AddressAttributeValueSearchModel = new AddressAttributeValueSearchModel();
    }

    #endregion

    #region Properties

    [ResourceDisplayName("Admin.Address.AddressAttributes.Fields.Name")]
    public string Name { get; set; }

    [ResourceDisplayName("Admin.Address.AddressAttributes.Fields.IsRequired")]
    public bool IsRequired { get; set; }

    [ResourceDisplayName("Admin.Address.AddressAttributes.Fields.AttributeControlType")]
    public int AttributeControlTypeId { get; set; }

    [ResourceDisplayName("Admin.Address.AddressAttributes.Fields.AttributeControlType")]
    public string AttributeControlTypeName { get; set; }

    [ResourceDisplayName("Admin.Address.AddressAttributes.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    public IList<AddressAttributeLocalizedModel> Locales { get; set; }

    public AddressAttributeValueSearchModel AddressAttributeValueSearchModel { get; set; }

    #endregion
}

public partial record AddressAttributeLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [ResourceDisplayName("Admin.Address.AddressAttributes.Fields.Name")]
    public string Name { get; set; }
}