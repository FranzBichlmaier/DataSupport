using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class PeComplianceReport
    {
        public int PeEntityId { get; set; }
        public List<ComplianceReport> ComplianceReports { get; set; } = new();
    }
}
