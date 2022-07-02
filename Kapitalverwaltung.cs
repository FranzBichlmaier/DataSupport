using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class Kapitalverwaltung
    {
        public int KvgId { get; set; }
        public string KvgName { get; set; }
        public string KvgStrasse { get; set; }
        public string KvgPlz { get; set; }
        public string KvgOrt { get; set; }
        public string KvgLand { get; set; }
        public string KvgAbsender { get; set; }
        public byte[] KvgLogo { get; set; }
        public string KvgFooter1 { get; set; }
        public string KvgFooter2 { get; set; }
        public string KvgFooter3 { get; set; }       
        public byte[] KvgAnnahmeLetter { get; set; } = null;
        public byte[] KvgClosingLetter { get; set; } = null;
        public byte[] KvgCapitalCallLetter { get; set; }= null;
        public byte[] KvgDistributionLetter { get; set; } = null;
        public byte[] KvgCombinationLetter { get; set; } = null;
        public string KvgAnnahmeFilename { get; set; }=string.Empty;
        public string KvgClosingFilename { get; set; }=string.Empty;
        public string KvgCapitalCallFilename { get; set; } = string.Empty;
        public string KvgDistributionFilename { get; set; } = string.Empty;
        public string KvgCombinationFilename { get; set; } = string.Empty;
    }
}


