using System.Collections.Generic;

namespace EasyWater.Domain.Models.Api
{
    public class MoistureReport : BaseMeasurementReport<Moisture>
    {
        public MoistureReport(List<Moisture> items)
            : base(items)
        {
        }
        
        public List<Moisture> moistures => items;       
    }
}
