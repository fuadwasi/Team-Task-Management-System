namespace AssetForge.Core.Domain.Messages;

/// <summary>
/// Represents message template system names
/// </summary>
public static partial class MessageTemplateSystemNames
{
    #region Customer

    /// <summary>
    /// Represents system name of notification about new registration
    /// </summary>
    public const string CUSTOMER_REGISTERED_SITE_OWNER_NOTIFICATION = "NewCustomer.Notification";

    /// <summary>
    /// Represents system name of customer welcome message
    /// </summary>
    public const string CUSTOMER_WELCOME_MESSAGE = "Customer.WelcomeMessage";

    /// <summary>
    /// Represents system name of email validation message
    /// </summary>
    public const string CUSTOMER_EMAIL_VALIDATION_MESSAGE = "Customer.EmailValidationMessage";

    /// <summary>
    /// Represents system name of email revalidation message
    /// </summary>
    public const string CUSTOMER_EMAIL_REVALIDATION_MESSAGE = "Customer.EmailRevalidationMessage";

    /// <summary>
    /// Represents system name of password recovery message
    /// </summary>
    public const string CUSTOMER_PASSWORD_RECOVERY_MESSAGE = "Customer.PasswordRecovery";

    /// <summary>
    /// Represents system name of delete customer request notification
    /// </summary>
    public const string DELETE_CUSTOMER_REQUEST_SITE_OWNER_NOTIFICATION = "Customer.Gdpr.DeleteRequest";

    #endregion Customer

    #region Newsletter

    /// <summary>
    /// Represents system name of subscription activation message
    /// </summary>
    public const string NEWSLETTER_SUBSCRIPTION_ACTIVATION_MESSAGE = "NewsLetterSubscription.ActivationMessage";

    /// <summary>
    /// Represents system name of subscription deactivation message
    /// </summary>
    public const string NEWSLETTER_SUBSCRIPTION_DEACTIVATION_MESSAGE = "NewsLetterSubscription.DeactivationMessage";

    #endregion Newsletter

    #region Forum

    /// <summary>
    /// Represents system name of notification about new forum topic
    /// </summary>
    public const string NEW_FORUM_TOPIC_MESSAGE = "Forums.NewForumTopic";

    /// <summary>
    /// Represents system name of notification about new forum post
    /// </summary>
    public const string NEW_FORUM_POST_MESSAGE = "Forums.NewForumPost";

    /// <summary>
    /// Represents system name of notification about new private message
    /// </summary>
    public const string PRIVATE_MESSAGE_NOTIFICATION = "Customer.NewPM";

    #endregion Forum

    #region Misc

    /// <summary>
    /// Represents system name of notification Site owner about new blog comment
    /// </summary>
    public const string BLOG_COMMENT_SITE_OWNER_NOTIFICATION = "Blog.BlogComment";

    /// <summary>
    /// Represents system name of notification Site owner about new news comment
    /// </summary>
    public const string NEWS_COMMENT_SITE_OWNER_NOTIFICATION = "News.NewsComment";

    /// <summary>
    /// Represents system name of 'Contact us' message
    /// </summary>
    public const string CONTACT_US_MESSAGE = "Service.ContactUs";

    #endregion Misc
}