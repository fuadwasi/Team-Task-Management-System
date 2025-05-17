using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Directory;

namespace AssetForge.Services.ExportImport;

/// <summary>
/// Export manager interface
/// </summary>
public partial interface IExportManager
{
    Task<byte[]> ExportCustomersToXlsxAsync(IList<Customer> customers);

    Task<string> ExportCustomersToXmlAsync(IList<Customer> customers);

    Task<string> ExportStatesToTxtAsync(IList<StateProvince> states);
}