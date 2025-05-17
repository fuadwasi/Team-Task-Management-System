using AssetForge.Core.Infrastructure;
using AssetForge.Data.Configuration;
using AssetForge.Data.DataProviders;

namespace AssetForge.Data;

/// <summary>
/// Represents the data provider manager
/// </summary>
public partial class DataProviderManager : IDataProviderManager
{
    #region Methods

    /// <summary>
    /// Gets data provider by specific type
    /// </summary>
    /// <param name="dataProviderType">Data provider type</param>
    /// <returns></returns>
    public static IDataProvider GetDataProvider(DataProviderType dataProviderType)
    {
        return dataProviderType switch
        {
            DataProviderType.SqlServer => new MsSqlAssetForgeDataProvider(),
            DataProviderType.MySql => new MySqlAssetForgeDataProvider(),
            DataProviderType.PostgreSQL => new PostgreSqlDataProvider(),
            _ => throw new Core.AssetForgeException($"Not supported data provider name: '{dataProviderType}'"),
        };
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets data provider
    /// </summary>
    public IDataProvider DataProvider
    {
        get
        {
            var dataProviderType = Singleton<DataConfig>.Instance.DataProvider;

            return GetDataProvider(dataProviderType);
        }
    }

    #endregion
}