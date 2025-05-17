using AssetForge.Core.Domain.Attributes;
using AssetForge.Web.Framework.Models;

namespace AssetForge.App.Models.Customer;

public partial record CustomerAttributeModel : BaseEntityModel
{
    public CustomerAttributeModel()
    {
        Values = new List<CustomerAttributeValueModel>();
    }

    public string Name { get; set; }

    public bool IsRequired { get; set; }

    /// <summary>
    /// Default value for textboxes
    /// </summary>
    public string DefaultValue { get; set; }

    public AttributeControlType AttributeControlType { get; set; }

    public IList<CustomerAttributeValueModel> Values { get; set; }

}

public partial record CustomerAttributeValueModel : BaseEntityModel
{
    public string Name { get; set; }

    public bool IsPreSelected { get; set; }
}