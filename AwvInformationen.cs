using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class AwvInformationen
    {
        public DateTime Datum { get; set; }
        public string CashflowType { get; set; }
        public decimal Wechselkurs { get; set; }
        public decimal CashflowBetrag { get; set; }
        public string Waehrung { get; set; }
        public decimal ReturnOfCapital { get; set; }
        public decimal CapitalGain { get; set; }
        public string Comment { get; set; }
        public int InvestmentId { get; set; }
        public int PeEntityId { get; set; }
        public int InvestmentCashflowId { get; set; }
        public string AwvNummer { get; set; }
        public decimal CashflowBetragMeldung { get; set; }
        public decimal ReturnOfCapitalMeldung { get; set; }
        public decimal CapitalGainMeldung { get; set; }

    }
}
