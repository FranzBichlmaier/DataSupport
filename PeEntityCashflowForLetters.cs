using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class PeEntityCashflowForLetters: PeEntityCashflow
    {
        public int InvestorId { get; set; }
        public decimal OverUnderPayment { get; set; }
        public string InvestorName { get; set; }
        public decimal CallAmount { get; set; } = 0;

    }
}
