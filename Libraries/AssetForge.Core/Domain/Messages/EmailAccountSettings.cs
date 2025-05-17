using AssetForge.Core.Configuration;

namespace AssetForge.Core.Domain.Messages;

/// <summary>
/// Email account settings
/// </summary>
public partial class EmailAccountSettings : ISettings
{
    /// <summary>
    /// Gets or sets a Site default email account identifier
    /// </summary>
    public int DefaultEmailAccountId { get; set; }
}