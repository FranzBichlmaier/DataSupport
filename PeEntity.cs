
using System;
using System.Collections.Generic;

namespace DataSupport
{
    public class PeEntity 
    {
        public int PeEntityId { get; set; }
        public string PortfolioName { get; set; }
        public string PortfolioLegalEntity { get; set; } = string.Empty;
        public decimal TotalCommitment { get; set; }
        public decimal TotalCommitmentInEuro { get; set; }
        public string Currency { get; set; }
        public string TaxNumber { get; set; }
        public string Finanzamt { get; set; }
        public string SteuerlicheEinordnung { get; set; }
        public string Lei { get; set; }
        public string AwvNummer { get; set; }
        public string BbkInstitutsnummer { get; set; }
        public string Recht { get; set; }
        public string Organisation { get; set; }
        public string Typ { get; set; }
        public string Art_Inhaber { get; set; }
        public string Art_Mittelanlage { get; set; }
        public string Art_Ertragsverwendung { get; set; }
        public string Art_Laufzeit { get; set; }
        public string Art_Ruecknahme { get; set; }
        public string Art_Notiz { get; set; }
        public string Wertgesichert { get; set; }
        public DateTime? Laufzeitbeginn { get; set; }
        public DateTime? Laufzeitende { get; set; }
        public DateTime? FinalClosing { get; set; }
        public string Strasse { get; set; }
        public string Postfach { get; set; }
        public string Plz { get; set; }
        public string Ort { get; set; }
        public string Country { get; set; }
        public string GIIN { get; set; } = string.Empty;
        public string UserChanged { get; set; }
        public DateTime DateChanged { get; set; }
        public string Handelsregisternummer { get; set; } = string.Empty;
        public string Amtsgericht { get; set; } = string.Empty;
        public string BaFinNummer { get; set; } = string.Empty;
        public string PsPlusBeteiligungsnummer { get; set; } = string.Empty;
        public string EIN { get; set; } = string.Empty;
        public string Strategie { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public int AuflageJahr { get; set; } = 0;
        public int KVG { get; set; }


        public List<Investment> Investments { get; set; } = null;
        public decimal SummeInvestorCommitments { get; set; }

    }
}
