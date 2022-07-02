using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class InvestorAccount
    {
        public int InvestorAccountId { get; set; }
        public int InvestorId { get; set; }
        public string Iban { get; set; }
        public string Bic { get; set; }
        public string Bankname { get; set; }
        public string Waehrung { get; set; }
        public string UserChanged { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
