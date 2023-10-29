using EasyWater.Service.Core.Entities;

namespace EasyWater.Domain
{
    public class Temperatura : OwnerEntity, ISensorEntity
    {
        public double Valor { get; set; }
    }
}
