using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class Reconciliation
    {
        public int AccountReconciliationId { get; set; }
        public int PeEntityCashflowId { get; set; }
        public int InvestorId { get; set; }
        public string Currency { get; set; }
        public string Iban { get; set; }
        public decimal AmountExpected { get; set; }
        public decimal AmountBooked { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? DateBooked { get; set; }
        public string InvestorName { get; set; }
        public int InvestorNummer { get; set; }
        public string PortfolioLegalEntity { get; set; }
        public bool IsReconciled { get; set; } = false;

    }
}
