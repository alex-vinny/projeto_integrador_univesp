using EasyWater.Service.Core.Entities;

namespace EasyWater.Domain
{
    public class HumidadeSolo : OwnerEntity, ISensorEntity
    {
        public double Valor { get; set; }
        public bool LigouIrrigacao { get; set; }
    }
}
