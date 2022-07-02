using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class InvestmentNav : INav
    {
        public int InvestmentNavId { get; set; }
        public int InvestmentId { get; set; }
        public DateTime NavDate { get; set; }
        public decimal Amount { get; set; }
        public string UserChanged { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
