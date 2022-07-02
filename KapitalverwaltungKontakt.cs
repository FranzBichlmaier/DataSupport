using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class KapitalverwaltungKontakt
    {
        public int KvgContactId { get; set; }
        public int KvgId { get; set; }
        public int KvgSequence { get; set; } = 0;
        public string KvgContactName { get; set; }
        public string KvgContactTelefon { get; set; }
        public string KvgContactEmail { get; set; }
    }
}
