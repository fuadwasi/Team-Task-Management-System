using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Configuration;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Directory;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Logging;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Domain.ScheduleTasks;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Domain.Seo;
using AssetForge.Core.Domain.Sites;
using AssetForge.Core.Domain.Teams;
using AssetForge.Data.Extensions;
using AssetForge.Data.Mapping.Builders.Teams;
using FluentMigrator;

namespace AssetForge.Data.Migrations.Installation;

[SchemaMigration("2020/01/31 11:24:16:2551771", "AssetForge.Data base schema", MigrationProcessType.Installation)]
public class SchemaMigration : ForwardOnlyMigration
{
    /// <summary>
    /// Collect the UP migration expressions
    /// <remarks>
    /// We use an explicit table creation order instead of an automatic one
    /// due to problems creating relationships between tables
    /// </remarks>
    /// </summary>
    public override void Up()
    {
        Create.TableFor<AddressAttribute>();
        Create.TableFor<AddressAttributeValue>();
        Create.TableFor<GenericAttribute>();
        Create.TableFor<SearchTerm>();
        Create.TableFor<Country>();
        Create.TableFor<StateProvince>();
        Create.TableFor<Address>();
        Create.TableFor<Language>();
        Create.TableFor<CustomerAttribute>();
        Create.TableFor<CustomerAttributeValue>();
        Create.TableFor<Customer>();
        Create.TableFor<CustomerPassword>();
        Create.TableFor<CustomerAddressMapping>();
        Create.TableFor<CustomerRole>();
        Create.TableFor<CustomerCustomerRoleMapping>();
        Create.TableFor<Site>();
        Create.TableFor<SiteMapping>();
        Create.TableFor<LocaleStringResource>();
        Create.TableFor<LocalizedProperty>();
        Create.TableFor<Download>();
        Create.TableFor<Picture>();
        Create.TableFor<PictureBinary>();
        Create.TableFor<Video>();
        Create.TableFor<Setting>();
        Create.TableFor<ActivityLogType>();
        Create.TableFor<ActivityLog>();
        Create.TableFor<Log>();
        Create.TableFor<AclRecord>();
        Create.TableFor<PermissionRecord>();
        Create.TableFor<PermissionRecordCustomerRoleMapping>();
        Create.TableFor<UrlRecord>();
        Create.TableFor<ScheduleTask>();
        Create.TableFor<Team>();
        Create.TableFor<TeamMemberMap>();
        Create.TableFor<TeamTask>();
    }
}