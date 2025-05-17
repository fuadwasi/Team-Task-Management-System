using AssetForge.Core;
using AssetForge.Core.Domain.Customers;
using AssetForge.Services.Events;
using AssetForge.Services.Messages;

namespace AssetForge.Services.Customers;

/// <summary>
/// Represents a customer event consumer
/// </summary>
public class CustomerEventConsumer : IConsumer<CustomerChangeWorkingLanguageEvent>
{
    #region Fields

    protected readonly ICustomerService _customerService;
    protected readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
    protected readonly ISiteContext _siteContext;

    #endregion

    #region Ctor

    public CustomerEventConsumer(ICustomerService customerService,
        INewsLetterSubscriptionService newsLetterSubscriptionService,
        ISiteContext siteContext)
    {
        _customerService = customerService;
        _newsLetterSubscriptionService = newsLetterSubscriptionService;
        _siteContext = siteContext;
    }

    #endregion

    #region Methods
    /// <summary>
    /// Handle working language changed event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(CustomerChangeWorkingLanguageEvent eventMessage)
    {
        if (eventMessage.Customer is not Customer customer)
            return;

        if (await _customerService.IsGuestAsync(customer))
            return;

        var site = await _siteContext.GetCurrentSiteAsync();
        var subscription = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndSiteIdAsync(customer.Email, site.Id);
        if (subscription != null && subscription.LanguageId != customer.LanguageId)
        {
            subscription.LanguageId = customer.LanguageId ?? 0;
            await _newsLetterSubscriptionService.UpdateNewsLetterSubscriptionAsync(subscription);
        }
    }

    #endregion
}
