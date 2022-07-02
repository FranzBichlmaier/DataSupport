using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class InvestorDocument
    {
        public int InvestorDocumentId { get; set; }
        public int InvestorId { get; set; }
        public int DocumentId { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public int DocumentTypeId { get; set; }
        public string FileType { get; set; }
        public string UserChanged { get; set; }
        public DateTime? DateChanged { get; set; }
    }
}
