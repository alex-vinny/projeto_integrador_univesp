using EasyWater.Service.Core.Entities;
using System;

namespace EasyWater.Domain
{
    public class Token : OwnerEntity
    {
        public Token()
        {
            Chave = Guid.NewGuid();
        }

        public Guid Chave { get; set; }
        public bool Expirado { get; set; }
        public DateTime Expiracao { get; set; }
    }
}
