using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class PeEntityInvestor
    {
        public int PeEntityInvestorId { get; set; }
        public int PeEntityId { get; set; }
        public decimal Commitment { get; set; }
        public string Sektor { get; set; }
        public string Country { get; set; }
        public string UserChanged { get; set; }
        public DateTime DateChanged { get; set; }
    }
}
