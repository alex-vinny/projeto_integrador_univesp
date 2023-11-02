using System;

namespace EasyWater.Domain.Models.Api
{
    public class ReportTesting
    {
        public ReportTesting()
        {
            dataIni = DateTime.Now.AddDays(-30);
            dataFin = DateTime.Now;
            total = 10000;
        }

        public DateTime dataIni { get; set; }
        public DateTime dataFin { get; set; }       
        public int total { get; set; }
    }
}
