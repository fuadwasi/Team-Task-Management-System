using AssetForge.Core;
using AssetForge.Core.Configuration;
using AssetForge.Core.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;
using WebOptimizer;

namespace AssetForge.Web.Framework.WebOptimizer;

public partial class WebOptimizerConfig : IWebOptimizerOptions, IConfig
{
    #region Properties

    /// <summary>
    /// A value indicating whether JS file bundling and minification is enabled
    /// </summary>
    public bool EnableJavaScriptBundling { get; protected set; } = true;

    /// <summary>
    /// A value indicating whether CSS file bundling and minification is enabled
    /// </summary>
    public bool EnableCssBundling { get; protected set; } = true;

    /// <summary>
    /// Gets or sets a suffix for the js-file name of generated bundles
    /// </summary>
    public string JavaScriptBundleSuffix { get; protected set; } = ".scripts";

    /// <summary>
    /// Gets or sets a suffix for the css-file name of generated bundles
    /// </summary>
    public string CssBundleSuffix { get; protected set; } = ".styles";

    /// <summary>
    /// Gets a section name to load configuration
    /// </summary>
    [JsonIgnore]
    public string Name => "WebOptimizer";

    /// <summary>
    /// Gets an order of configuration
    /// </summary>
    /// <returns>Order</returns>
    public int GetOrder() => 2;

    #region WebOptimizer options

    public bool? EnableCaching { get; set; } = true;
    public bool? EnableMemoryCache { get; set; } = true;
    public bool? EnableDiskCache { get; set; } = true;

    private string _cacheDirectory = string.Empty;
    public string CacheDirectory
    {
        get
        {
            if (string.IsNullOrEmpty(_cacheDirectory))
            {
                var fileProvider = EngineContext.Current.Resolve<IAssetForgeFileProvider>() ?? CommonHelper.DefaultFileProvider;
                _cacheDirectory = fileProvider.Combine(fileProvider.MapPath("~/"), @"wwwroot\bundles");
            }

            return _cacheDirectory;
        }
        set => _cacheDirectory = value;
    }
    public bool? EnableTagHelperBundling { get; set; } = false;
    public string CdnUrl { get; set; } = "";
    public bool? AllowEmptyBundle { get; set; } = true;
    public HttpsCompressionMode HttpsCompression { get; set; } = HttpsCompressionMode.Compress;

    #endregion

    #endregion
}