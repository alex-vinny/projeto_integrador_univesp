using System;

namespace EasyWater.Domain.Models.Api
{
    public class Watering
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public bool value { get; set; }
    }
}
