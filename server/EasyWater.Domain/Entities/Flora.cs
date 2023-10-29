using EasyWater.Service.Core.Entities;

namespace EasyWater.Domain
{
    public class Flora : BaseEntity
    {
        public string Nome { get; set; }
        public string Detalhes { get; set; }
        public int Codigo { get; set; }
    }
}
