using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class BundesbankInvestment
    {
        public Investment Investment { get; set; }
        public List<InvestmentCashflow> InvestmentCashflows { get; set; }
        public List<InvestmentNav> InvestmentNavs { get; set; }
    }
}
