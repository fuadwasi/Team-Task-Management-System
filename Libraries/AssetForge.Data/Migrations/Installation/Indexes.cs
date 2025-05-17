using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Directory;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Logging;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Domain.Seo;
using AssetForge.Core.Domain.Sites;
using FluentMigrator;
using FluentMigrator.SqlServer;

namespace AssetForge.Data.Migrations.Installation;

[SchemaMigration("2020/03/13 09:36:08:9037677", "AssetForge.Data base indexes", MigrationProcessType.Installation)]
public class Indexes : ForwardOnlyMigration
{
    #region Methods

    public override void Up()
    {
        Create.Index("AssetForge_UrlRecord_Slug")
            .OnTable(nameof(UrlRecord))
            .OnColumn(nameof(UrlRecord.Slug))
            .Ascending()
            .WithOptions()
            .NonClustered();

        Create.Index("AssetForge_UrlRecord_Custom_1").OnTable(nameof(UrlRecord))
            .OnColumn(nameof(UrlRecord.EntityId)).Ascending()
            .OnColumn(nameof(UrlRecord.EntityName)).Ascending()
            .OnColumn(nameof(UrlRecord.LanguageId)).Ascending()
            .OnColumn(nameof(UrlRecord.IsActive)).Ascending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_SiteMapping_EntityId_EntityName").OnTable(nameof(SiteMapping))
            .OnColumn(nameof(SiteMapping.EntityId)).Ascending()
            .OnColumn(nameof(SiteMapping.EntityName)).Ascending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_Log_CreatedOnUtc").OnTable(nameof(Log))
            .OnColumn(nameof(Log.CreatedOnUtc)).Descending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_LocaleStringResource").OnTable(nameof(LocaleStringResource))
            .OnColumn(nameof(LocaleStringResource.ResourceName)).Ascending()
            .OnColumn(nameof(LocaleStringResource.LanguageId)).Ascending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_Language_DisplayOrder").OnTable(nameof(Language))
            .OnColumn(nameof(Language.DisplayOrder)).Ascending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_GenericAttribute_EntityId_and_KeyGroup").OnTable(nameof(GenericAttribute))
            .OnColumn(nameof(GenericAttribute.EntityId)).Ascending()
            .OnColumn(nameof(GenericAttribute.KeyGroup)).Ascending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_Customer_Username").OnTable(nameof(Customer))
            .OnColumn(nameof(Customer.Username)).Ascending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_Customer_SystemName").OnTable(nameof(Customer))
            .OnColumn(nameof(Customer.SystemName)).Ascending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_Customer_Email").OnTable(nameof(Customer))
            .OnColumn(nameof(Customer.Email)).Ascending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_Customer_CustomerGuid").OnTable(nameof(Customer))
            .OnColumn(nameof(Customer.CustomerGuid)).Ascending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_Customer_CreatedOnUtc").OnTable(nameof(Customer))
            .OnColumn(nameof(Customer.CreatedOnUtc)).Descending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_Country_DisplayOrder").OnTable(nameof(Country))
            .OnColumn(nameof(Country.DisplayOrder)).Ascending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_ActivityLog_CreatedOnUtc").OnTable(nameof(ActivityLog))
            .OnColumn(nameof(ActivityLog.CreatedOnUtc)).Descending()
            .WithOptions().NonClustered();

        Create.Index("AssetForge_AclRecord_EntityId_EntityName").OnTable(nameof(AclRecord))
            .OnColumn(nameof(AclRecord.EntityId)).Ascending()
            .OnColumn(nameof(AclRecord.EntityName)).Ascending()
            .WithOptions().NonClustered();
    }

    #endregion Methods
}