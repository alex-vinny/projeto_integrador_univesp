using FluentMigrator;

namespace EasyWater.Service.Core.Migrations
{
    [Migration(202310291257)]
    public class InsertTableFlora_1 : Migration
    {
        public override void Up()
        {
            var row = new 
            {
                Nome = "Vaso de planta",                
                Detalhes = "Localizada em Campo Limpo Paulista",
                Codigo = 1
            };
            Insert.IntoTable("Flora").Row(row);
        }

        public override void Down()
        {
            Delete.FromTable("Flora").AllRows();
        }
    }
}
