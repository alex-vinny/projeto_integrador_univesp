using EasyWater.Domain;

namespace EasyWater.Service.Core.Entities
{
    public class OwnerEntity : BaseEntity
    {
        public long DonoId { get; set; }
        public Flora Dono { get; set; }
    }
}
