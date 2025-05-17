using AssetForge.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AssetForge.Web.Framework.Mvc.ModelBinding.Binders;

/// <summary>
/// Represents a model binder provider for specific properties
/// </summary>
public partial class ModelBinderProvider : IModelBinderProvider
{
    IModelBinder IModelBinderProvider.GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.PropertyName == nameof(BaseModel.CustomProperties) && context.Metadata.ModelType == typeof(Dictionary<string, string>))
            return new CustomPropertiesModelBinder();

        if (!context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(string))
            return new StringModelBinder();

        return null;
    }
}