using System;

namespace DataSupport
{
    public interface ICashflow
    {
        string CashflowType { get; set; }
        decimal CashflowAmount { get; set; }
        DateTime CashflowDate { get; set; }
         decimal ReturnOfCapital { get; set; }
         decimal CapitalGain { get; set; }
         decimal Interests { get; set; }
         decimal Dividends { get; set; }
         decimal OtherIncome { get; set; }
         decimal Recallable { get; set; }
         decimal PartnershipExpenses { get; set; }
         decimal LookbackInterests { get; set; }
         decimal WithholdingTax { get; set; }
    }
}