using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class WirtBerechtigter
    {
        public int WirtBerechtigterId { get; set; }
        public int InvestorId { get; set; }
        public string Title { get; set; }
        public string Vorname { get; set; }
        public string Familienname { get; set; }
        public string VollName { get; set; }
        public int Sequence { get; set; }
        public string Funktion { get; set; }
        public string Strasse { get; set; }
        public string Adresszusatz { get; set; }
        public string Postleitzahl { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
        public string Geburtsort { get; set; }
        public DateTime? Geburtstag { get; set; }
        public string Steuernummer { get; set; }
        public string SteuerLand { get; set; }

    }
}
