using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using LinqToDB.DataProvider.PostgreSQL;
using System.Data.Common;

namespace AssetForge.Data.DataProviders.LinqToDB;

/// <summary>
/// Represents a data provider for PostgreSQL
/// </summary>
public partial class LinqToDBPostgreSQLDataProvider : PostgreSQLDataProvider
{
    public LinqToDBPostgreSQLDataProvider() : base(ProviderName.PostgreSQL, PostgreSQLVersion.v95) { }

    public override void SetParameter(DataConnection dataConnection, DbParameter parameter, string name, DbDataType dataType, object value)
    {

        if (value is string && dataType.SystemType == typeof(string))
        {
            dataType = dataType.WithDbType("citext");
        }

        base.SetParameter(dataConnection, parameter, name, dataType, value);
    }
}