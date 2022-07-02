using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class Investor
    {
        public int InvestorId { get; set; }
        public int DatevId { get; set; }
        public int InvestorNummer { get; set; }
        public int InvestorAdressId { get; set; }
        public int InvestorKontaktId { get; set; }
        public string InvestorName { get; set; }
        public string SteuerId { get; set; }
        public string Steuernummer { get; set; }
        public string Finanzamt { get; set; }
        public string Rechtsform { get; set; }
        public string Registergericht { get; set; }
        public string Handelsregisternummer { get; set; }
        public string Geburtsort { get; set; }
        public DateTime? Geburtsdatum { get; set; }
        public string Telefon { get; set; }
        public string Telefax { get; set; }
        public string Email_1 { get; set; }
        public string Email_2 { get; set; }
        public string Email_3 { get; set; }
        public string Signatur_1 { get; set; }
        public string Position_1 { get; set; }
        public string Signatur_2 { get; set; }
        public string Position_2 { get; set; }
        public string Kontaktname { get; set; }
        public string KontoTyp { get; set; }
        public bool Confidential { get; set; }
        public bool UseMail { get; set; }
        public bool UseEmail { get; set; }
        public bool UseHqtSave { get; set; }
        public bool UseTransferForm { get; set; }
        public string Remarks { get; set; }
        public string UserChanged { get; set; }
        public DateTime DateChanged { get; set; }
        public string Datenraum { get; set; }
        public string Kundennummer { get; set; } 
        public string Mifid { get; set; }
        public string WPHG { get; set; }
        public string Sektor { get; set; }
        public string Zeile_1 { get; set; }
        public string Zeile_2 { get; set; }
        public string Zeile_3 { get; set; }
        public string AdressZeile_1 { get; set; }
        public string AdressZeile_2 { get; set; }
        public string AdressZeile_3 { get; set; }
        public string AdressZeile_4 { get; set; }
        public string AdressZeile_5 { get; set; }
        public string AdressZeile_6 { get; set; }        
        public string Anrede_1 { get; set; }
        public string Anrede_2 { get; set; }

        public virtual InvestorKontakt InvestorInfo { get; set; }
        public virtual InvestorKontakt KontaktInfo { get; set; }
    }
}
