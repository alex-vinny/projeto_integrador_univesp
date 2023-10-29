using EasyWater.Service.Core.Extensions;
using FluentMigrator;

namespace EasyWater.Service.Core.Migrations
{
    [Migration(202310291225)]
    public class CreateTableTemperatura : Migration
    {
        public override void Up()
        {
            Create.Table("Temperatura")
                .IncludeBaseColumns()
                .WithColumn("DonoId").AsInt64().NotNullable()
                .WithColumn("Valor").AsDouble().WithDefaultValue(0);

            Create.ForeignKey()
                .FromTable("Temperatura").ForeignColumn("DonoId")
                .ToTable("Flora").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Temperatura");
        }
    }
}
