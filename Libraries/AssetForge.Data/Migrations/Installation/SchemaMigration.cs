using FluentMigrator;
using AssetForge.Core.Domain.Attributes;
using AssetForge.Core.Domain.Catalog;
using AssetForge.Core.Domain.Common;
using AssetForge.Core.Domain.Configuration;
using AssetForge.Core.Domain.Customers;
using AssetForge.Core.Domain.Directory;
using AssetForge.Core.Domain.Localization;
using AssetForge.Core.Domain.Logging;
using AssetForge.Core.Domain.Media;
using AssetForge.Core.Domain.Messages;
using AssetForge.Core.Domain.ScheduleTasks;
using AssetForge.Core.Domain.Security;
using AssetForge.Core.Domain.Seo;
using AssetForge.Core.Domain.Sites;
using AssetForge.Core.Domain.Topics;
using AssetForge.Core.Domain.Vendors;
using AssetForge.Data.Extensions;

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
        Create.TableFor<ExternalAuthenticationRecord>();
        Create.TableFor<RewardPointsHistory>();
        Create.TableFor<Site>();
        Create.TableFor<SiteMapping>();
        Create.TableFor<LocaleStringResource>();
        Create.TableFor<LocalizedProperty>();
        Create.TableFor<Manufacturer>();
        Create.TableFor<ManufacturerTemplate>();
        Create.TableFor<Download>();
        Create.TableFor<Picture>();
        Create.TableFor<PictureBinary>();
        Create.TableFor<Video>();
        Create.TableFor<Setting>();
        Create.TableFor<ActivityLogType>();
        Create.TableFor<ActivityLog>();
        Create.TableFor<Log>();
        Create.TableFor<EmailAccount>();
        Create.TableFor<MessageTemplate>();
        Create.TableFor<NewsLetterSubscription>();
        Create.TableFor<QueuedEmail>();
        Create.TableFor<AclRecord>();
        Create.TableFor<PermissionRecord>();
        Create.TableFor<PermissionRecordCustomerRoleMapping>();
        Create.TableFor<UrlRecord>();
        Create.TableFor<ScheduleTask>();
        Create.TableFor<WidgetAttribute>();
        Create.TableFor<PredefinedWidgetAttributeValue>();
        Create.TableFor<WidgetAttributeMapping>();
        Create.TableFor<WidgetAttributeValue>();
        Create.TableFor<WidgetAttributeValuePicture>();
        Create.TableFor<TopicTemplate>();
        Create.TableFor<Topic>();
        Create.TableFor<Vendor>();
        Create.TableFor<VendorAttribute>();
        Create.TableFor<VendorAttributeValue>();
        Create.TableFor<VendorNote>();
    }
}