using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class PeEntityAccount
    {
        public int PeEntityAccountId { get; set; }
        public int PeEntityId { get; set; }
        public string Iban { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string Sector { get; set; }
        public string Fristigkeit { get; set; }
        public string Name { get; set; }
        public string UserChanged { get; set; }
        public DateTime DateChanged { get; set; }

    }
}
