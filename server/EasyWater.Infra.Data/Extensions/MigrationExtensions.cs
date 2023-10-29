using FluentMigrator.Builders.Create.Table;
using System;

namespace EasyWater.Service.Core.Extensions
{
    public static class MigrationExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax IncludeBaseColumns(this ICreateTableWithColumnOrSchemaOrDescriptionSyntax table)
        {
            return table
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("CriadoEm").AsDateTime2().WithDefaultValue(DateTime.Today)
                .WithColumn("AtualizadoEm").AsDateTime2().Nullable()
                .WithColumn("Deletado").AsBoolean().WithDefaultValue(false);
        }
    }
}
