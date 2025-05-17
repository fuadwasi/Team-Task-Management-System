namespace AssetForge.Services.ExportImport;

public partial interface IImportManager
{
    Task<int> ImportStatesFromTxtAsync(Stream stream, bool writeLog = true);

    Task ImportCustomersFromXlsxAsync(Stream stream);
}