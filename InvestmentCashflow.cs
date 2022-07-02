using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class InvestmentCashflow: ICashflow
    {
        public int InvestmentCashflowId { get; set; }
        public int InvestmentId { get; set; }
        public string CashflowType { get; set; }
        public string CashflowDescription { get; set; }
        public DateTime CashflowDate { get; set; }
        public decimal CashflowAmount { get; set; }
        public decimal ReturnOfCapital { get; set; }
        public decimal CapitalGain { get; set; }
        public decimal Interests { get; set; }
        public decimal Dividends { get; set; }
        public decimal OtherIncome { get; set; }
        public decimal Recallable { get; set; }
        public decimal CarriedInterests { get; set; }
        public decimal PartnershipExpenses { get; set; }
        public decimal LookbackInterests { get; set; }
        public decimal WithholdingTax { get; set; }
        public string UserChanged { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
