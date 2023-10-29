using EasyWater.Service.Core.Extensions;
using FluentMigrator;

namespace EasyWater.Service.Core.Migrations
{
    [Migration(202310291223)]
    public class CreateTableFlora : Migration
    {
        public override void Up()
        {
            Create.Table("Flora")
                .IncludeBaseColumns()
                .WithColumn("Nome").AsString(50).NotNullable()
                .WithColumn("Detalhes").AsString(200).Nullable()
                .WithColumn("Codigo").AsInt32();
        }

        public override void Down()
        {
            Delete.Table("Flora");
        }
    }
}
