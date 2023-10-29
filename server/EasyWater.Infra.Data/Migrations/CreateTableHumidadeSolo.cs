using EasyWater.Service.Core.Extensions;
using FluentMigrator;

namespace EasyWater.Service.Core.Migrations
{
    [Migration(202310291226)]
    public class CreateTableHumidadeSolo : Migration
    {
        public override void Up()
        {
            Create.Table("HumidadeSolo")
                .IncludeBaseColumns()
                 .WithColumn("DonoId").AsInt64().NotNullable()
                 .WithColumn("LigouIrrigacao").AsBoolean().WithDefaultValue(false)
                 .WithColumn("Valor").AsDouble().WithDefaultValue(0);

            Create.ForeignKey()
                .FromTable("HumidadeSolo").ForeignColumn("DonoId")
                .ToTable("Flora").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("HumidadeSolo");
        }
    }
}
