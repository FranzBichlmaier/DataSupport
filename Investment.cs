using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class Investment
    {
        public int InvestmentId { get; set; }
        public int PeEntityId { get; set; }
        public int ContactId { get; set; }
        public string InvestmentShortName { get; set; }
        public string InvestmentName { get; set; }
        public string Country { get; set; }
        public decimal InvestmentAmount { get; set; }
        public string Currency { get; set; }
        public decimal InvestmentTotalSize { get; set; }
        public string Beteiligungsart { get; set; }
        public string Nutzungsart { get; set; }
        public int AnzahlObjekte { get; set; }
        public string UserChanged { get; set; }
        public DateTime DateChanged { get; set; }
        public PeEntity PeEntity { get; set; }
        public InvestorKontakt Contact { get; set; }
    }
}
