using EasyWater.Service.Core.Extensions;
using FluentMigrator;

namespace EasyWater.Service.Core.Migrations
{
    [Migration(202310291224)]
    public class CreateTableHumidade : Migration
    {
        public override void Up()
        {
            Create.Table("Humidade")
                .IncludeBaseColumns()
                .WithColumn("DonoId").AsInt64().NotNullable()
                .WithColumn("Valor").AsDouble().WithDefaultValue(0);

            Create.ForeignKey()
                .FromTable("Humidade").ForeignColumn("DonoId")
                .ToTable("Flora").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Humidade");
        }
    }
}
