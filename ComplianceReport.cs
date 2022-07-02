using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class ComplianceReport
    {
        [Bindable(false)]
        public int RecordId { get; set; }
        [Bindable(false)]
        public string LegalEntity { get; set; }
        [DisplayNameAttribute("Investor")]
        public string InvestorName { get; set; }
        [DisplayNameAttribute("Potenzielles Interesse")]
        public decimal AmountRequested { get; set; }
        [DisplayNameAttribute("in %")]
        public decimal AmountRequestedInPercent { get; set; }
        [DisplayNameAttribute("Commitment")]
        public decimal AmountClosed { get; set; }
        [DisplayNameAttribute("in %")]
        public decimal AmountClosedInPercent { get; set; }
        [DisplayNameAttribute("Versand Präsentation")]
        public DateTime? PresentationSent { get; set; }
        [DisplayNameAttribute("Kunde MIFID")]
        public string Mifid { get; set; }
        [DisplayNameAttribute("Kunde WPHG")]
        public string WPHG { get; set; }
        [DisplayNameAttribute("Versandweg ZU")]
        public string MailType { get; set; }
        [DisplayNameAttribute("Ordner versendet")]
        public DateTime? DirectorySent { get; set; }
        [DisplayNameAttribute("Versand Datum")]
        public DateTime? DocumentsSent { get; set; }
        [DisplayNameAttribute("Erhalt Unterlagen (Scan)")]
        public DateTime? ScannedDocumentsReceived { get; set; }
        [DisplayNameAttribute("Erhalt Unterlagen (Original)")]
        public DateTime? OriginalDocumentsReceived { get; set; }
        [DisplayNameAttribute("ZU unterschrieben am")]
        public DateTime? DocumentsAccepted { get; set; }
        [DisplayNameAttribute("Besonderheiten")]
        public string Remarks { get; set; }
    }
}
