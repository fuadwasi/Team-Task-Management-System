using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Directory;
using AssetForge.Core.Domain.Messages;

namespace AssetForge.Services.ExportImport;

/// <summary>
/// Export manager interface
/// </summary>
public partial interface IExportManager
{
    ///// <summary>
    ///// Export category list to XML
    ///// </summary>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the result in XML format
    ///// </returns>
    //Task<string> ExportCategoriesToXmlAsync();

    ///// <summary>
    ///// Export categories to XLSX
    ///// </summary>
    ///// <param name="categories">Categories</param>
    ///// <returns>A task that represents the asynchronous operation</returns>
    //Task<byte[]> ExportCategoriesToXlsxAsync(IList<Category> categories);

    /// <summary>
    /// Export customer list to XLSX
    /// </summary>
    /// <param name="customers">Customers</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task<byte[]> ExportCustomersToXlsxAsync(IList<Customer> customers);

    /// <summary>
    /// Export customer list to XML
    /// </summary>
    /// <param name="customers">Customers</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result in XML format
    /// </returns>
    Task<string> ExportCustomersToXmlAsync(IList<Customer> customers);

    /// <summary>
    /// Export newsletter subscribers to TXT
    /// </summary>
    /// <param name="subscriptions">Subscriptions</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result in TXT (string) format
    /// </returns>
    Task<string> ExportNewsletterSubscribersToTxtAsync(IList<NewsLetterSubscription> subscriptions);

    /// <summary>
    /// Export states to TXT
    /// </summary>
    /// <param name="states">States</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the result in TXT (string) format
    /// </returns>
    Task<string> ExportStatesToTxtAsync(IList<StateProvince> states);

    ///// <summary>
    ///// Export customer info (GDPR request) to XLSX 
    ///// </summary>
    ///// <param name="customer">Customer</param>
    ///// <param name="storeId">Store identifier</param>
    ///// <returns>
    ///// A task that represents the asynchronous operation
    ///// The task result contains the customer GDPR info
    ///// </returns>
    //Task<byte[]> ExportCustomerGdprInfoToXlsxAsync(Customer customer, int storeId);
}