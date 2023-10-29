using EasyWater.Service.Core.Extensions;
using FluentMigrator;
using System;

namespace EasyWater.Service.Core.Migrations
{
    [Migration(202310291326)]
    public class CreateTableToken : Migration
    {
        public override void Up()
        {
            Create.Table("Token")
                .IncludeBaseColumns()
                .WithColumn("DonoId").AsInt64().NotNullable()
                .WithColumn("Expirado").AsBoolean().WithDefaultValue(false)
                .WithColumn("Expiracao").AsDateTime2().NotNullable()
                .WithColumn("Chave").AsGuid().Unique().WithDefaultValue(Guid.NewGuid());

            Create.ForeignKey()
                .FromTable("Token").ForeignColumn("DonoId")
                .ToTable("Flora").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Token");
        }
    }
}
