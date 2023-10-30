using EasyWater.Domain.Extensions;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace EasyWater.Domain.Models.Api
{
    public abstract class BaseMeasurementReport<TEntity> : PaginationReport
        where TEntity : BaseMeasurement
    {
        protected List<TEntity> items;
        public BaseMeasurementReport(List<TEntity> list)
        {
            items = list;
        }

        public TEntity max => items.Maxima(c => c.value).First();
        public TEntity min => items.Minima(c => c.value).First();
        public List<TEntity> average => items.Where(c => c.value == items.Select(b => b.value).Median()).ToList();
        public object control => new
        {
            max = max.value,
            min = min.value
        };
    }
}
