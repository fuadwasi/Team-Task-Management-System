//using FluentMigrator;
//using AssetForge.Core.Domain.Attributes;
//using AssetForge.Core.Domain.Cms;
//
//using AssetForge.Data.Extensions;

//namespace AssetForge.Data.Migrations.Upgrade.UpgradeTo1002;

//[SchemaMigration("2024-04-20 00:00:29", "Update schema migration for 1.0025")]
//public class SchemaMigration : ForwardOnlyMigration
//{
//    public override void Up()
//    {
//        if (!Schema.Table(nameof(WidgetPicture)).Exists())
//            Create.TableFor<WidgetPicture>();

//        //if (!Schema.Table(nameof(WidgetLayoutMap)).Exists())
//        //    Create.TableFor<WidgetLayoutMap>();

//        var widgetAttributeTableName = $"{typeof(WidgetAttribute).Name}";

//        if (!Schema.Table(widgetAttributeTableName).Column(nameof(WidgetAttribute.SystemName)).Exists())
//        {
//            Alter.Table(widgetAttributeTableName)
//            .AddColumn(nameof(WidgetAttribute.SystemName)).AsString().NotNullable().SetExistingRowsTo("Systemname");
//        }

//        var newsItemTableName = $"{typeof(NewsItem).Name}";

//        if (!Schema.Table(newsItemTableName).Column(nameof(NewsItem.PictureId)).Exists())
//        {
//            Alter.Table(newsItemTableName)
//            .AddColumn(nameof(NewsItem.PictureId)).AsInt32().NotNullable().SetExistingRowsTo("0");
//        }

//        if (!Schema.Table(newsItemTableName).Column(nameof(NewsItem.PictureId)).Exists())
//        {
//            Alter.Table(newsItemTableName)
//            .AddColumn(nameof(NewsItem.PictureId)).AsInt32().NotNullable().SetExistingRowsTo("0");
//        }
//    }
//}