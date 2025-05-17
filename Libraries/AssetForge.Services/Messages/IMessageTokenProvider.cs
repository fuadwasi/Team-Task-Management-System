using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Messages;
using AssetForge.Core.Domain.Sites;
using AssetForge.Core.Domain.Vendors;

namespace AssetForge.Services.Messages;

/// <summary>
/// Message token provider
/// </summary>
public partial interface IMessageTokenProvider
{
    /// <summary>
    /// Add site tokens
    /// </summary>
    /// <param name="tokens">List of already added tokens</param>
    /// <param name="site">Site</param>
    /// <param name="emailAccount">Email account</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddSiteTokensAsync(IList<Token> tokens, Site site, EmailAccount emailAccount);

    Task AddVendorTokensAsync(IList<Token> tokens, Vendor vendor);

    /// <summary>
    /// Add customer tokens
    /// </summary>
    /// <param name="tokens">List of already added tokens</param>
    /// <param name="customerId">Customer identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddCustomerTokensAsync(IList<Token> tokens, int customerId);

    /// <summary>
    /// Add customer tokens
    /// </summary>
    /// <param name="tokens">List of already added tokens</param>
    /// <param name="customer">Customer</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddCustomerTokensAsync(IList<Token> tokens, Customer customer);

    /// <summary>
    /// Add newsletter subscription tokens
    /// </summary>
    /// <param name="tokens">List of already added tokens</param>
    /// <param name="subscription">Newsletter subscription</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddNewsLetterSubscriptionTokensAsync(IList<Token> tokens, NewsLetterSubscription subscription);

    /// <summary>
    /// Get collection of allowed (supported) message tokens
    /// </summary>
    /// <param name="tokenGroups">Collection of token groups; pass null to get all available tokens</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the collection of allowed message tokens
    /// </returns>
    Task<IEnumerable<string>> GetListOfAllowedTokensAsync(IEnumerable<string> tokenGroups = null);

    /// <summary>
    /// Get token groups of message template
    /// </summary>
    /// <param name="messageTemplate">Message template</param>
    /// <returns>Collection of token group names</returns>
    IEnumerable<string> GetTokenGroups(MessageTemplate messageTemplate);
}