using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class CashflowSummary
    {
        public string EntityName { get; set; }
        public decimal Commitment { get; set; }
        public decimal OpenCommitment { get; set; }
        public decimal OpenCommitmentToCommitment { get; set; }
        public decimal Valuation { get; set; }
        public DateTime? ValuationDate { get; set; }
        public DateTime? CalculationDate { get; set; }
        public List<Cashflow> Cashflows { get; set; }
        public decimal TotalCalls { get; set; }
        public decimal TotalDistributions { get; set; }
        public decimal TotalRecallables { get; set; }
        public decimal TotalNetDistributions { get; set; }
        public decimal TotalWithholdingTax { get; set; }
        public decimal TotalCarriedInterests { get; set; }
        public decimal TotalReturnOfCapital { get; set; }
        public decimal TotalCapitalGain { get; set; }
        public decimal TotalDividends { get; set; }
        public decimal TotalInterests { get; set; }
        public decimal TotalOtherIncome { get; set; }
        public decimal TotalLookbackInterests { get; set; }
        public decimal TotalLookbackInterestsReceived { get; set; }
        public decimal CallsToCommitment { get; set; }
        public decimal DistributionsToCommitment { get; set; }
        public decimal NetDistributionsToCommitment { get; set; }
        public decimal GrossDistributionToCommitment { get; set; }
        public decimal DistributionsToCalls { get; set; }
        
        public decimal TVPI { get; set; }
        public decimal RVPI { get; set; }
        public decimal DPI { get; set; }
        public decimal? IRR { get; set; }
        public int VintageYear { get; set; }
        public string Currency { get; set; }

    }
}
