using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class InvestorSignature
    {
        public int InvestorSignatureId { get; set; }
        public int InvestorId { get; set; } = 0;
        public string ManagerName { get; set; } = string.Empty;
        public string ManagerRole { get; set; } = string.Empty;
        public int Sequence { get; set; } = 0;
    }
}
