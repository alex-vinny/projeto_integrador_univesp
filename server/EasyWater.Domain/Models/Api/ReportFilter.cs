using System;

namespace EasyWater.Domain.Models.Api
{
    public class ReportFilter
    {
        public ReportFilter()
        {
            page = 1;
            pageSize = 50;
        }

        public DateTime dataIni { get; set; }
        public DateTime dataFin { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
