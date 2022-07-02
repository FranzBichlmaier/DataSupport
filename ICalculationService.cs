using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSupport
{
    public interface ICalculationService
    {
        CashflowSummary GetPerformanceSummary(CashflowSummary clientSummary);
        CashflowSummary GetCashflowSummaryForInvestment(Investment investment, DateTime? cDate);
        CashflowSummary GetCashflowSummaryForEntity(PeEntity entity, DateTime? cDate);
        List<TvpiHistory> GetTvpiHistory(List<Cashflow> cashflows, List<Nav> navs, DateTime? endDate=null);
        string NumberToWords(int number);
    }
}