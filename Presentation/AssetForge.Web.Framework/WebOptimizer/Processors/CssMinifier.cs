using AssetForge.Core.Infrastructure;
using AssetForge.Services.Logging;
using NUglify;
using NUglify.Css;
using WebOptimizer;

namespace AssetForge.Web.Framework.WebOptimizer.Processors;

/// <summary>
/// Represents a class of processor that handle style assets
/// </summary>
/// <remarks>Implementation has taken from WebOptimizer to add logging</remarks>
public partial class CssMinifier : Processor
{
    #region Methods

    /// <summary>
    /// Executes the processor on the specified configuration.
    /// </summary>
    /// <param name="context">The context used to perform processing to WebOptimizer.IAsset instances</param>
    public override async Task ExecuteAsync(IAssetContext context)
    {
        var content = new Dictionary<string, byte[]>();

        foreach (var key in context.Content.Keys)
        {
            if (key.EndsWith(".min.css", StringComparison.InvariantCultureIgnoreCase))
            {
                content[key] = context.Content[key];
                continue;
            }

            var input = context.Content[key].AsString();
            var result = Uglify.Css(input, new CssSettings());

            var minified = result.Code;

            if (result.HasErrors)
            {
                await EngineContext.Current.Resolve<ILogger>()
                    .WarningAsync($"Stylesheet minification: {key}", new(string.Join(Environment.NewLine, result.Errors)));
            }

            content[key] = minified.AsByteArray();
        }

        context.Content = content;

        return;
    }

    #endregion

}