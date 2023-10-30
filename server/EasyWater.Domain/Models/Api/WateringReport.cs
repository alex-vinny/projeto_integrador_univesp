using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace EasyWater.Domain.Models.Api
{
    public class WateringReport : PaginationReport
    {
        public WateringReport(List<Watering> list)
        {
            waterings = list;
        }

        public Watering max => waterings.Maxima(c => c.date).Where(c => c.value).First();
        public Watering min => waterings.Minima(c => c.date).Where(c => c.value).First();
        public List<Watering> average => waterings.Where(c => c.value && c.date <= max.date && c.date >= min.date).ToList();
        public object control => new
        {
            max = max.date,
            min = min.date
        };

        public List<Watering> waterings { get;set; }
    }
}
