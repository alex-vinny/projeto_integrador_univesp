using EasyWater.Service.Core.Entities;

namespace EasyWater.Domain
{
    public class Humidade : OwnerEntity, ISensorEntity
    {
        public double Valor { get; set; }
    }
}
