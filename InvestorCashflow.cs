using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class InvestorCashflow
    {
        public int InvestorCashflowId { get; set; }
        public int PeEntityCashflowId { get; set; }
        public int? PeEntityCashflowId2 { get; set; }
        public int InvestorId { get; set; }
        public decimal CommitmentAmount { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountExcpected { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal CallAmount { get; set; }
        public decimal DistributionAmount { get; set; }
        public decimal ReturnOfCapital { get; set; }
        public decimal CapitalGain { get; set; }
        public decimal Interests { get; set; }
        public decimal Dividends { get; set; }
        public decimal OtherIncome { get; set; }
        public decimal Recallable { get; set; }
        public decimal PartnershipExpenses { get; set; }
        public decimal LookbackInterests { get; set; }
        public decimal WithholdingTax { get; set; }
        public decimal OpenBalance { get; set; }
        public decimal RowDifference { get; set; }
        public string Remarks { get; set; }
        public string Waehrung { get; set; }
        public string UserChanged { get; set; }
        public DateTime DateChanged { get; set; }
        
        public Investor Investor { get; set; }
        public PeEntity PeEntity { get; set; }

    }
}
