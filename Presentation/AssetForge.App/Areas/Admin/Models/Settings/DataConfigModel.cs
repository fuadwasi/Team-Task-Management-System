using AssetForge.Web.Framework.Models;
using AssetForge.Web.Framework.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AssetForge.App.Areas.Admin.Models.Settings;

public partial record DataConfigModel : BaseModel, IConfigModel
{
    #region Properties

    [ResourceDisplayName("Admin.Configuration.AppSettings.Data.ConnectionString")]
    public string ConnectionString { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.Data.DataProvider")]
    public int DataProvider { get; set; }
    public SelectList DataProviderTypeValues { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.Data.SQLCommandTimeout")]
    [UIHint("Int32Nullable")]
    public int? SQLCommandTimeout { get; set; }

    [ResourceDisplayName("Admin.Configuration.AppSettings.Data.WithNoLock")]
    public bool WithNoLock { get; set; }

    #endregion
}