using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSupport
{
    public interface IDataServices
    {
        
        List<PeEntity> GetAllPeEntities();
        PeEntity GetPeEntityById(int peEntityId);
        List<PeEntity> GetAllPeEntitiesIncludeInvestments();
        List<PeEntityNav> GetAllPeEntityNavsForPeEntity(int peEntityId);
        List<PeEntityCashflow> GetAllPeEntityCashflowsForPeEntity(int peEntityId);
        PeEntityCashflow GetPeEntityCashflowById(int peEntityId);
        List<PeEntityAccount> GetAllPeEntityAccountsForPeEntiy(int peEntityId);
        List<PeEntityInvestor> GetAllPeEntityInvestorsForPeEntity(int peEntityId);
        bool PeEntityHasInvestments(int peEntityId);
        int InsertNewPeEntityAccount(PeEntityAccount account);
        void UpdatePeEntiyAccount(PeEntityAccount account);
        void RemovePeEntityAccount(int peEntityAccountId);
        int InsertNewPeEntity(PeEntity entity);
        void UpdatePeEntiy(PeEntity entity);
        void RemovePeEntity(int peEntityId);
        int InsertNewPeEntityInvestor(PeEntityInvestor investor);
        void UpdatePeEntiyInvestor(PeEntityInvestor investor);
        void RemovePeEntityInvestor(int peEntityInvestorId);
        List<AwvInformationen> GetAwvInformationenAsync(DateTime from, DateTime to);
        List<Investment> GetAllInvestmentsIncludePeEntity();
        List<InvestmentCashflow> GetAllInvestmentCashflowsForInvestment(int InvestmentId);
        List<InvestmentCashflow> GetFilteredInvestmentCashflowsForInvestment(int InvestmentId, DateTime? datefrom, string type);
        List<InvestmentNav> GetAllInvestmentNavsForInvestment(int InvestmentId);
        int InsertNewInvestment(Investment investment);
        int InsertNewInvestmentNav(InvestmentNav nav);
        void UpdateInvestmentNav(InvestmentNav nav);
        void RemoveInvestmentNav(int InvestmentNavId);
        PeEntity GetNewDefaultPeEntity();
        Investment GetNewDefaultInvestment();
        int InsertNewInvestmentCashflow(InvestmentCashflow cashflow);
        void UpdateInvestmentCashflow(InvestmentCashflow cashflow);
        void RemoveInvestmentCashflow(int investmentCashflowId);
        int InsertNewPeEntityNav(PeEntityNav nav);
        void UpdatePeEntityNav(PeEntityNav nav);
        void RemovePeEntityNav(int peEntityNavId);
        int InsertNewPeEntityCashflow(PeEntityCashflow cashflow);
        void UpdatePeEntityCashflow(PeEntityCashflow cashflow);
        void RemovePeEntityCashflow(int PeEntityCashflowId);

        #region Investors

        int InsertNewInvestor(Investor investor);
        int InsertNewInvestorKontakt(InvestorKontakt contact);
        int InsertNewInvestorAccount(InvestorAccount account);
        void UpdateInvestorAccount(InvestorAccount account);
        void RemoveInvestorAccount(int InvestorAccountId);
        void UpdateInvestor(Investor investor);
        void UpdateInvestorKontakt(InvestorKontakt contact);
        List<InvestorAccount> GetAllInvestorAccountsForInvestorId(int investorId);
        List<Investor> GetAllInvestors();
        InvestorKontakt GetInvestorKontaktById(int contactId);
        Investor GetInvestorById(int investorId);        
        List<InvestorKontakt> GelAllInvestorKontakts();
        int InsertInvestorSignature(InvestorSignature investorSignature);
        void UpdateInvestorSignature(InvestorSignature investorSignature);
        void RemoveInvestorSignature(int investorSignatureId);
        List<InvestorSignature> GetAllInvestorSignaturesForId(int investorSignatureId);
        void TruncateAllInvestorTables();

        int InsertWirtBerechtigter(WirtBerechtigter wirtBerechtigter);
        void UpdateWirtBerechtigter(WirtBerechtigter wirtBerechtigter);
        List<WirtBerechtigter> GetAllWirtBerechtigterForInvestorId(int InvestorId);
        void RemoveWirtBerechtigter(int wirtBerechtigterId);
        #endregion
        #region Commitments
        int InsertCommitment(Commitment commitment);
        void UpdateCommitment(Commitment commitment);
        void RemoveCommitment(int CommitmentId);
        List<Commitment> GetCommitmentsForPeEntity(int PeEntityId);
        List<Commitment> GetCommitmentsForInvestor(int InvestorId);
        #endregion
        List<Cashflow> ConvertPeEntityCashflow(List<PeEntityCashflow> cashflows);
        List<Nav> ConvertPeEntityNav(List<PeEntityNav> navs);
        List<Cashflow> ConvertInvestmentCashflow(List<InvestmentCashflow> cashflows);
        List<Nav> ConvertInvestmentNav(List<InvestmentNav> navs);
        string GetUserName();
        Bllw_User GetWindowsUser(string username);
        List<Bllw_User> GetAllUsers();
        List<Bllw_Role> GetRolesForUser(int userId);
        void UpdateRolesForUser(int userId, List<Bllw_Role> roles);
        List<Bllw_Role> GetAllRoles();
        void UpdateBllwUser(Bllw_User user);
        void DeleteBllwUser(int userId);     
        decimal GetPeEntityInvestorSum(int peEntityId);
        bool CheckIbanSum(string iban);
        string FormatIban(string iban);
        DateTime? GetNextNav(DateTime? lastNavDate);
        void UpdateInvestment(Investment editInvestment);
        void RemoveInvestment(int investmentId);
        string? GetAppState(AppState appState);
        void InsertOrUpdateAppState(AppState appState);

        #region Kvgs
        int InsertKapitalverwaltungsGesellschaft(Kapitalverwaltung kapitalverwaltung);
        void UpdateKapitalverwaltungsGesellschaft(Kapitalverwaltung kapitalverwaltung);
        List<Kapitalverwaltung> GetAllKapitalverwaltungsGesellschaften();
        Kapitalverwaltung GetKvgById(int KvgId);
        int InsertKvgContact(KapitalverwaltungKontakt contact);
        void UpdateKvgContact(KapitalverwaltungKontakt contact);
        List<KapitalverwaltungKontakt> GetKapitalverwaltungKontaktsForKvg(int kvgId);
        #endregion

        #region ComplianceReports
        string GetComplianceInformationForPeEntity(int peEntityId);
        void SaveComplianceInformationForPeEntity(int PeEntityId, string jsonText);

        #endregion
    }
}