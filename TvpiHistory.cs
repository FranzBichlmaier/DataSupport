using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class TvpiHistory
    {
        public DateTime Quartalsende { get; set; }
        public string MonthYearString { get; set; }
        public decimal Contributions { get; set; }
        public decimal Distributions { get; set; }
        public decimal DistributionsPi { get; set; }
        public decimal Valuation { get; set; }
        public decimal ValuationPi { get; set; }
        public decimal Tvpi { get; set; }
        public double NetCashflowInQuarter { get; set; }
    }
}
