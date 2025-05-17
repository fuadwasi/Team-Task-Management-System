namespace AssetForge.App.Areas.Admin.Helpers;

public partial interface ITinyMceHelper
{
    Task<string> GetTinyMceLanguageAsync();
}