using FreeSql.DataAnnotations;
using System;

namespace EasyWater.Service.Core.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CriadoEm = DateTime.Now;
        }

        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }
        public DateTime? CriadoEm { get; set; }
        public DateTime? AtualizadoEm { get; set; }
        public bool Deletado { get; set; }
    }
}
