using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel.FinancialFunctions;
using DateTimeFunctions;

namespace DataSupport
{
    public class CalculationService : ICalculationService
    {
        DateDifferences dateFunctions = new DateDifferences();        
        private readonly IDataServices dataServices;
        private CashflowSummary Summary;

        public CalculationService(IDataServices dataServices)
        {
            this.dataServices = dataServices;
        }
        /// <summary>
        /// return CashflowSummary  for an Investment
        /// </summary>
        /// <param name="clientSummary"></param>
        /// <returns>CashflowSummary</returns>
        public CashflowSummary GetPerformanceSummary(CashflowSummary clientSummary)
        {
            CashflowSummary summary = new CashflowSummary
            {
                EntityName = clientSummary.EntityName,
                Commitment = clientSummary.Commitment,
                Valuation = clientSummary.Valuation,
                ValuationDate = clientSummary.ValuationDate,
                VintageYear = clientSummary.VintageYear,
                Currency = clientSummary.Currency,
                CalculationDate = clientSummary.CalculationDate,
                Cashflows = clientSummary.Cashflows
            };
            // sort cashflows by cashflowdate ascending
            summary.Cashflows = summary.Cashflows.OrderBy(c => c.CashflowDate).ToList();

            if (clientSummary.Commitment == 0 || clientSummary.Cashflows.Count==0)
            {
                summary.IRR = null;
                return summary;
            };

            // if no CalculationDate or ValuationDate is provided use monthend of previous month
            DateTime defaultValuationDate = dateFunctions.MonthEnd(DateTime.Now.AddMonths(-1));

            if (summary.ValuationDate == null) summary.ValuationDate = defaultValuationDate;
            if (summary.CalculationDate == null) summary.CalculationDate = defaultValuationDate;           
           

            // adjust Valuation with cashflows between ValuationDate and CalculationDate
            foreach(Cashflow c in summary.Cashflows)
            {
                if (c.CashflowDate <= summary.ValuationDate) continue;
                if (c.CashflowDate > summary.CalculationDate) continue;
                summary.Valuation -= c.CashflowAmount;
            }

            // Calculate TotalCalls, TotalDistributions, TotalRecallable, TotalNetDistributions

            summary.TotalCalls = summary.Cashflows.Where(c => c.CashflowType.StartsWith("C")).Sum(c => c.CashflowAmount)*-1;
            summary.TotalDistributions = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.CashflowAmount);
            summary.TotalWithholdingTax = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.WithholdingTax);
            summary.TotalRecallables = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.Recallable);
            summary.TotalReturnOfCapital = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.ReturnOfCapital);
            summary.TotalCapitalGain = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.CapitalGain);
            summary.TotalDividends = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.Dividends);
            summary.TotalInterests = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.Interests);
            summary.TotalOtherIncome = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.OtherIncome);
            summary.TotalRecallables = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.Recallable);
            summary.TotalLookbackInterests = summary.Cashflows.Sum(c => c.LookbackInterests);  
            summary.TotalLookbackInterestsReceived = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.LookbackInterests);
            summary.TotalNetDistributions = summary.TotalDistributions + summary.TotalWithholdingTax - summary.TotalRecallables;
            summary.TotalCarriedInterests = summary.Cashflows.Where(c => c.CashflowType.StartsWith("D")).Sum(c => c.CarriedInterests);
            summary.TotalCalls = summary.TotalCalls - summary.TotalRecallables;
            summary.OpenCommitment = summary.Commitment - summary.TotalCalls;
            summary.OpenCommitmentToCommitment = Math.Round(summary.OpenCommitment / summary.Commitment * 100, 2);

            // Calculate Calls To Commitments, Distributions to Commitments and Distributions To Calls

            summary.CallsToCommitment = Math.Round(summary.TotalCalls / summary.Commitment * 100, 2);
            summary.NetDistributionsToCommitment = Math.Round((summary.TotalNetDistributions-summary.TotalWithholdingTax) / summary.Commitment * 100, 2);
            summary.DistributionsToCommitment = Math.Round((summary.TotalNetDistributions) / summary.Commitment * 100, 2);
            summary.DistributionsToCalls = Math.Round((summary.TotalNetDistributions + summary.TotalWithholdingTax) / summary.TotalCalls * 100, 2);
            summary.GrossDistributionToCommitment=Math.Round((summary.TotalDistributions+summary.TotalWithholdingTax) / summary.Commitment * 100, 2);

            // Calculate TVPI, DPI and RVPI

            decimal tv = summary.Valuation + summary.TotalNetDistributions;
            summary.TVPI = Math.Round(tv / summary.TotalCalls , 2);
            summary.DPI = Math.Round(summary.DistributionsToCalls / 100, 2);  // is equal to Distributions to Calls
            summary.RVPI = Math.Round(summary.Valuation / summary.TotalCalls , 2);

            // Calculate IRR

            if (summary.Cashflows.Count == 0)
            {
                summary.IRR = null;
                return summary;
            }

            if (summary.Cashflows.Count ==1 && summary.Cashflows.ElementAt(0).CashflowDate == (DateTime)summary.CalculationDate)
            {
                summary.IRR = null;
                return summary;
            }

            // add Valuation as final cashflow

            summary.Cashflows.Add(new Cashflow
            {
                CashflowAmount = summary.Valuation,
                CashflowDate = (DateTime)summary.CalculationDate
            });

            List<DateTime> dates = new List<DateTime>();
            List<double> values = new List<double>();

            foreach (ICashflow cf in summary.Cashflows)
            {
                dates.Add(cf.CashflowDate);
                values.Add((double)cf.CashflowAmount);
            }

            try
            {
                
                summary.IRR = Math.Round((decimal)Financial.XIrr(values, dates) * 100, 2);
            }
            catch (Exception )
            {
                summary.IRR = null;
            }

            return summary;
        }
        /// <summary>
        /// Calculates CashflowSummary for PeEntity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cDate"></param>
        /// <returns>CashflowSummary</returns>
        public CashflowSummary GetCashflowSummaryForEntity(PeEntity entity, DateTime? cDate)
        {
            if (cDate == null) cDate = DateTime.Now;

            Summary = new CashflowSummary();
            Summary.EntityName = entity.PortfolioLegalEntity;
            Summary.Commitment = entity.TotalCommitment;
            Summary.VintageYear = entity.AuflageJahr;
            Summary.Currency = entity.Currency;
            List<PeEntityCashflow> cashflows = dataServices.GetAllPeEntityCashflowsForPeEntity(entity.PeEntityId);
            List<PeEntityNav> navs = dataServices.GetAllPeEntityNavsForPeEntity(entity.PeEntityId);

            // remove all Cashflows and Navs with Date >CalcDate
            cashflows = cashflows.Where(c => c.CashflowDate <= cDate).ToList();
            navs = navs.Where(n => n.NavDate <= cDate).ToList();

            if (navs.Count > 0)
            {
                Summary.Valuation = navs.ElementAt(0).Amount;
                Summary.ValuationDate = navs.ElementAt(0).NavDate;
            }
            if (navs.Count == 0)
            {
                Summary.Valuation = 0;
                Summary.ValuationDate = new DateTime(2000, 1, 1);
            }
            CreateSummaryCashflowsForPeEntity(cashflows);
            Summary.CalculationDate = cDate;
            Summary = GetPerformanceSummary(Summary);
            return Summary;
        }
        /// <summary>
        /// transforms List<PeEntityCashflow> to List<Cashflow>
        /// </summary>
        /// <param name="cashflows"></param>
        private void CreateSummaryCashflowsForPeEntity(List<PeEntityCashflow> cashflows)
        {
            Summary.Cashflows = new();
            foreach (PeEntityCashflow cf in cashflows)
            {
                if (cf.CashflowDate > Summary.CalculationDate) continue;    // ignore cashflows after calculationdate.
                Summary.Cashflows.Add(new Cashflow
                {
                    CashflowType = cf.CashflowType,
                    CashflowDescription = cf.CashflowDescription,
                    CashflowAmount = cf.CashflowAmount,
                    CashflowDate = cf.CashflowDate,
                    Recallable = cf.Recallable, 
                    WithholdingTax = cf.WithholdingTax,
                    ReturnOfCapital = cf.ReturnOfCapital,
                    CapitalGain = cf.CapitalGain,
                    Dividends = cf.Dividends,
                    Interests = cf.Interests,
                    OtherIncome = cf.OtherIncome,
                    LookbackInterests = cf.LookbackInterests
                });
            }
        }
        /// <summary>
        /// transforms List<InvestmentCashflow> to List<Cashflow>
        /// </summary>
        /// <param name="cashflows"></param>
        public CashflowSummary GetCashflowSummaryForInvestment(Investment investment, DateTime? cDate)
        {
            if (cDate == null) cDate = DateTime.Now;

            Summary = new CashflowSummary();
            Summary.EntityName = investment.InvestmentName;
            Summary.Commitment = investment.InvestmentAmount;
            List<InvestmentCashflow> cashflows = dataServices.GetAllInvestmentCashflowsForInvestment(investment.InvestmentId);
            List<InvestmentNav> navs = dataServices.GetAllInvestmentNavsForInvestment(investment.InvestmentId);

            // remove all Cashflows and Navs with Date >CalcDate
            cashflows = cashflows.Where(c => c.CashflowDate <= cDate).ToList();
            navs = navs.Where(n => n.NavDate <= cDate).ToList();

            if (navs.Count > 0)
            {
                Summary.Valuation = navs.ElementAt(0).Amount;
                Summary.ValuationDate = navs.ElementAt(0).NavDate;
            }
            if (navs.Count == 0)
            {
                Summary.Valuation = 0;
                Summary.ValuationDate = new DateTime(2000, 1, 1);
            }
            CreateSummaryCashflows(cashflows);
            Summary.CalculationDate = cDate;
            Summary = GetPerformanceSummary(Summary);
            return Summary;
        }
        private void CreateSummaryCashflows(List<InvestmentCashflow> cashflows)
        {
            Summary.Cashflows = new List<Cashflow>();
            foreach (InvestmentCashflow cf in cashflows)
            {
                if (cf.CashflowDate > Summary.CalculationDate) continue;    // ignore cashflows after calculationdate.
                Summary.Cashflows.Add(new Cashflow
                {
                    CashflowType = cf.CashflowType,
                    CashflowDescription = cf.CashflowDescription,
                    CashflowAmount = cf.CashflowAmount,
                    CashflowDate = cf.CashflowDate,
                    CarriedInterests = cf.CarriedInterests,
                    Recallable = cf.Recallable, 
                    WithholdingTax = cf.WithholdingTax,
                    ReturnOfCapital = cf.ReturnOfCapital,
                    CapitalGain = cf.CapitalGain,
                    Dividends = cf.Dividends,
                    Interests = cf.Interests,
                    OtherIncome = cf.OtherIncome,
                    LookbackInterests = cf.LookbackInterests
                });
            }
        }
        /// <summary>
        /// returns List<TvpiHistory> for each quarterend of a given time period
        /// </summary>
        /// <param name="cashflows"></param>
        /// <param name="navs"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<TvpiHistory> GetTvpiHistory(List<Cashflow> cashflows, List<Nav> navs, DateTime? endDate = null)
        {
            // order lists by date ascending
           
            if (endDate == null) endDate = DateTime.Now;
            DateTime reportEndDate = (DateTime)endDate;
            reportEndDate = reportEndDate.AddDays(1);
           
            cashflows = cashflows.OrderBy(c => c.CashflowDate).ToList();
            List<TvpiHistory> history = new List<TvpiHistory>();
            navs = navs.OrderBy(n => n.NavDate).ToList();

            // remove all nav until the first nav where the amount is greater than 0
            bool loop = true;
            do
            {
                if (navs.Count > 0)
                {
                    if (navs.ElementAt(0).Amount == 0)
                    {
                        navs.RemoveAt(0);
                    }
                    else
                    {
                        loop = false;
                    }
                }
                else
                {
                    loop = false;
                }
            } while (loop);

            // if navs.count == 0 return empty history
            if (navs.Count == 0) return history;

            List<Bll_QuarterDescription> quarterends = dateFunctions.GetQuarterList(cashflows.ElementAt(0).CashflowDate, dateFunctions.PreviousQuarter(reportEndDate));
            
            foreach(Bll_QuarterDescription q in quarterends)
            {
                history.Add(GetQuarterEndStatus(cashflows, navs, q));
            }
            // add valuation to the NetCashflow property for the last entry
            history.ElementAt(history.Count - 1).NetCashflowInQuarter += (double)history.ElementAt(history.Count - 1).Valuation;
            return history;
        }

        private TvpiHistory GetQuarterEndStatus(List<Cashflow> cashflows, List<Nav> navs, Bll_QuarterDescription quarter)
        {
            decimal totalRecallable = cashflows.Where(c => c.CashflowDate <= quarter.QuarterEnd && c.CashflowType.StartsWith("D")).Sum(c => c.Recallable);
            TvpiHistory h = new TvpiHistory();
            h.Quartalsende = quarter.QuarterEnd;
            h.MonthYearString = quarter.MonthYearString;
            h.Distributions = cashflows.Where(c => c.CashflowDate <= quarter.QuarterEnd && c.CashflowType.StartsWith("D")).Sum(c => c.CashflowAmount) - totalRecallable;               
            h.Contributions = cashflows.Where(c => c.CashflowDate <= quarter.QuarterEnd && c.CashflowType.StartsWith("C")).Sum(c => c.CashflowAmount) * -1 - totalRecallable;
            h.Valuation = GetValuation(cashflows, navs, quarter.QuarterEnd);
            h.ValuationPi = Math.Round((h.Valuation) / h.Contributions, 4);
            h.DistributionsPi = Math.Round((h.Distributions) / h.Contributions, 4);
            h.Tvpi=Math.Round((h.Valuation + h.Distributions) / h.Contributions, 2);
            h.NetCashflowInQuarter = (double)GetNetCashflowForQuarter(cashflows, quarter.QuarterEnd);
            return h;
        }

        private decimal GetNetCashflowForQuarter(List<Cashflow> cashflows, DateTime quarterEnd)
        {
            DateTime Quarterstart = dateFunctions.PreviousQuarter(quarterEnd);
            decimal balance = cashflows.Where(c => c.CashflowDate > Quarterstart && c.CashflowDate <= quarterEnd && c.CashflowType.StartsWith("C")).Sum(c => c.CashflowAmount);
            balance += cashflows.Where(c => c.CashflowDate > Quarterstart && c.CashflowDate <= quarterEnd && c.CashflowType.StartsWith("D")).Sum(c => c.CashflowAmount) ;
            balance -= cashflows.Where(c => c.CashflowDate > Quarterstart && c.CashflowDate <= quarterEnd && c.CashflowType.StartsWith("D")).Sum(c => c.Recallable) ;
            return balance;
        }

        private decimal GetValuation(List<Cashflow> cashflows, List<Nav> navs, DateTime quarterend)
        {
            Nav nav = navs.FirstOrDefault(n => n.NavDate == quarterend);
            if (nav != null && nav.Amount!=0) return nav.Amount;
            Nav lastNav = null;
            // check whether the first navdate is after requested quarterend
            // if yes set new NAV (Date=quarterend Amount =0)
            if (navs.ElementAt(0).NavDate <= quarterend)
            {
                foreach (Nav n in navs)
                {
                    lastNav = n;
                    if (n.NavDate > quarterend) break;
                }
            }
            else
            {
                lastNav = new Nav
                {
                    Amount = 0,
                    NavDate = quarterend
                };
            }
  
            decimal val = lastNav.Amount;
            foreach(Cashflow c in cashflows)
            {
                if (c.CashflowDate <= lastNav.NavDate) continue;
                if (c.CashflowDate > quarterend) continue;
                val -= c.CashflowAmount;
            }
            return val;
        }

        public string NumberToWords(int number)
        {
            if (number == 0)
                return "Null";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                if (number / 1000000 ==1)
                {
                    if (number == 1000000)
                    {
                        words = "eine Million ";
                    }
                    else
                    {
                        words = "eine Million ";
                    }                   
                }
                else
                {
                    words += NumberToWords(number / 1000000) + " Millionen ";
                }                
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + "tausend";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + "hundert";
                number %= 100;
            }

            if (number > 0)
            {
                //if (words != "")
                //    words += "and ";

                var unitsMap = new[] { "zero", "ein", "zwei", "drei", "vier", "fünf", "sechs", "sieben", "acht", "neun", "zehn", "elf", "zwölf", "dreizehn", "vierzehn", "fünfzehn", "sechzehn", "siebzehn", "achtzehn", "neunzehn" };
                var tensMap = new[] { "zero", "zehn", "zwanzig", "dreißig", "vierzig", "fünfzig", "sechzig", "siebzig", "achtzig", "neunzig" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    if (number % 10 == 0)
                    {
                        words += tensMap[number / 10];
                    }
                    else
                    {
                        words += unitsMap[number %  10] + "und" + tensMap[number / 10];
                    }                  
                }
            }

            return words;
        }
    }
}
