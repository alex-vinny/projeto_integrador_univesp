using System.Collections.Generic;

namespace EasyWater.Domain.Models.Api
{
    public class TemperatureReport : BaseMeasurementReport<Temperature>
    {
        public TemperatureReport(List<Temperature> items)
            : base(items)
        {
        }

        public List<Temperature> temperatures => items;
    }
}
