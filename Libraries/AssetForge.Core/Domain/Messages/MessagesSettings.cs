using AssetForge.Core.Configuration;

namespace AssetForge.Core.Domain.Messages;

/// <summary>
/// Messages settings
/// </summary>
public partial class MessagesSettings : ISettings
{
    /// <summary>
    /// A value indicating whether popup notifications set as default 
    /// </summary>
    public bool UsePopupNotifications { get; set; }

    /// <summary>
    /// A value indicating whether to use the default email account to send emails for Site owner
    ///
    /// If set to false the message template email address will be use
    /// </summary>
    public bool UseDefaultEmailAccountForSendSiteOwnerEmails { get; set; }
}