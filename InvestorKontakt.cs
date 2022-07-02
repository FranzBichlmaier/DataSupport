using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class InvestorKontakt
    {
        public int InvestorKontaktId { get; set; }
        public string Firma { get; set; }
        public string Title { get; set; }
        public string Vorname { get; set; }
        public string Familienname { get; set; }
        public string Kontaktname { get; set; }
        public string Strasse { get; set; }
        public string AdressZusatz { get; set; }
        public string Postleitzahl { get; set; }
        public string Ort { get; set; }
        public string Land { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string UserChanged { get; set; }
        public DateTime DateChanged { get; set; }
        public string SearchField { get; set; }
    }
}
