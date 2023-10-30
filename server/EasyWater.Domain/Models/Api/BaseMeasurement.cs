using System;

namespace EasyWater.Domain.Models.Api
{
    public abstract class BaseMeasurement
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public double value { get; set; }
    }
}
