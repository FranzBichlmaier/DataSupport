using DataSupport.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DateTimeFunctions;
using Dapper;

namespace DataSupport
{
    public class DataServices : IDataServices
    {
        private readonly ISqlDataAccess dataAccess;
        private readonly IDropDownServices dropDownServices;
        private DateDifferences dateFunctions = new DateDifferences();
        private Bllw_User windowsUser;
        private string dbName = "BLLWDB";

        public DataServices(ISqlDataAccess dataAccess, IDropDownServices dropDownServices)
        {
            this.dataAccess = dataAccess;
            this.dropDownServices = dropDownServices;

            if (windowsUser is null)
            {
                windowsUser = new()
                {
                    WindowsUser = "Developer"
                };
            }
        }
        #region PeEntity
        public List<PeEntity> GetAllPeEntities()
        {
            return dataAccess.LoadData<PeEntity, dynamic>("spPeEntities_ReadAll", new { }, dbName);
        }
        public PeEntity GetPeEntityById(int peEntityId)
        {
            PeEntity entity = dataAccess.LoadSingleItem<PeEntity, dynamic>("spPeEntities_ReadItem", new {PeEntityId = peEntityId}, dbName);
            entity.Investments = dataAccess.LoadData<Investment, dynamic>("spInvestments_ForPeEntity", new { PeEntityId = peEntityId }, dbName);
            return entity;
        }
        /// <summary>
        /// returns true if PeEntityId has related Records in table Investments
        /// return false if not
        /// </summary>
        /// <param name="PeEntityId"></param>
        /// <returns></returns>
        public bool PeEntityHasInvestments(int PeEntityId)
        {
            return dataAccess.PeEntityHasInvestments(PeEntityId, dbName);
        }

        public List<PeEntityCashflow> GetAllPeEntityCashflowsForPeEntity(int peEntityId)
        {
            return  dataAccess.LoadData<PeEntityCashflow, dynamic>("spPeEntityCashflows_ReadAllForPeEntity", new { PeEntityId = peEntityId }, dbName);
        }
        public PeEntityCashflow GetPeEntityCashflowById(int recordId)
        {
            return dataAccess.LoadSingleItem<PeEntityCashflow, dynamic>("spPeEntityCashflows_GetCashflowById", new { RecordId = recordId }, dbName);
        }

        public  List<PeEntity> GetAllPeEntitiesIncludeInvestments()
        {
            return  dataAccess.LoadFullPeEntities(dbName);
        }
        public List<PeEntityNav> GetAllPeEntityNavsForPeEntity(int peEntityId)
        {
            return dataAccess.LoadData<PeEntityNav, dynamic>("spPeEntityNavs_ReadAllForPeEntity", new { PeEntityId = peEntityId }, dbName);
        }
        public List<PeEntityAccount> GetAllPeEntityAccountsForPeEntiy(int peEntityId)
        {
            var result = dataAccess.LoadData<PeEntityAccount, dynamic>("spPeEntityAccounts_ReadAllForPeEntity", new { PeEntityId = peEntityId }, dbName);
            foreach(PeEntityAccount account in result)
            {
                account.Iban = CustomChecks.FormatIban(account.Iban);
            }
            return result;
        }

        public List<PeEntityInvestor> GetAllPeEntityInvestorsForPeEntity(int peEntityId)
        {
            return dataAccess.LoadData<PeEntityInvestor, dynamic>("spPeEntityInvestors_ReadAllForPeEntity", new { PeEntityId = peEntityId }, dbName);
        }
        #endregion

        public List<Investment> GetAllInvestmentsIncludePeEntity()
        {
            return dataAccess.LoadFullInvestments(dbName);
        }
        public List<InvestmentCashflow> GetAllInvestmentCashflowsForInvestment(int InvestmentId)
        {
            return dataAccess.LoadData<InvestmentCashflow, dynamic>("spInvestmentCashflows_ReadAllForInvestment", new { InvestmentId }, dbName);
        }
        /// <summary>
        /// Return a list of InvestmentCashflow for an InvestmentId beginning with a date for a type (Contribution or Distribution)
        /// The returnlist contains only cashflows of the same type.
        /// In case of a Contribution 5 days are added to the startdate 
        /// </summary>
        /// <param name="InvestmentId">Id of the Investment</param>
        /// <param name="datefrom">null or a date; if null all cashflows are read</param>
        /// <param name="type">"Contribution" or "Distribution"</param>
        /// <returns></returns>
        public List<InvestmentCashflow> GetFilteredInvestmentCashflowsForInvestment(int InvestmentId, DateTime? datefrom, string type)
        {
            if (datefrom == null) datefrom = new DateTime(2000, 1, 1);
            
            DateTime startdate = (DateTime)datefrom;
            // 5 days are only added in case of contributions
            if (type.StartsWith("C")) startdate = startdate.AddDays(5);
            return dataAccess.LoadData<InvestmentCashflow, dynamic>("spInvestmentCashflows_ReadFilteredRecords", new { InvestmentId, DateFrom = startdate, Type=type }, dbName);
        }

        public List<InvestmentNav> GetAllInvestmentNavsForInvestment(int InvestmentId)
        {
            return dataAccess.LoadData<InvestmentNav, dynamic>("spInvestmentNavs_ReadAllForInvestment", new { InvestmentId }, dbName);
        }
        public int InsertNewInvestment(Investment investment)
        {
            investment.UserChanged = windowsUser.WindowsUser;
            investment.DateChanged = DateTime.Now;

            var p = new
            {
                investment.AnzahlObjekte,
                investment.Beteiligungsart,
                investment.Country,
                investment.Currency,
                investment.InvestmentAmount,
                investment.InvestmentShortName, 
                investment.InvestmentName,
                investment.InvestmentTotalSize,
                investment.Nutzungsart,
                investment.PeEntityId, 
                investment.ContactId,
                investment.UserChanged,
                investment.DateChanged
            };
            
            return dataAccess.InsertData<dynamic>("spInvestments_InsertItem", p, dbName);
        }
        public void UpdateInvestment(Investment investment)
        {
            investment.UserChanged = windowsUser.WindowsUser;
            investment.DateChanged = DateTime.Now;
            var p = new
            {
                investment.AnzahlObjekte,
                investment.Beteiligungsart,
                investment.Country,
                investment.Currency,
                investment.InvestmentAmount,
                investment.InvestmentShortName,
                investment.InvestmentName,
                investment.InvestmentTotalSize,
                investment.Nutzungsart,
                investment.PeEntityId, 
                investment.ContactId,
                investment.InvestmentId,
                investment.UserChanged,
                investment.DateChanged
            };
            dataAccess.SaveData<dynamic>("spInvestments_UpdateItem", p, dbName);
        }

        public void RemoveInvestment(int investmentId)
        {
            var p = new
            {
                InvestmentId = investmentId
            };
            dataAccess.SaveData<dynamic>("spInvestments_RemoveItem", p, dbName);
        }
        public int InsertNewInvestmentNav(InvestmentNav nav)
        {
            nav.UserChanged = windowsUser.WindowsUser;
            nav.DateChanged = DateTime.Now;
            var p = new
            {
                nav.Amount,
                nav.NavDate,
                nav.InvestmentId,
                nav.UserChanged,
                nav.DateChanged
            };
            return dataAccess.InsertData<dynamic>("spInvestmentNavs_InsertItem", p, dbName);
        }

        public void UpdateInvestmentNav(InvestmentNav nav)
        {
            nav.UserChanged = windowsUser.WindowsUser;
            nav.DateChanged = DateTime.Now;
            var p = new
            {
                nav.Amount,
                nav.NavDate,
                nav.InvestmentId,
                nav.InvestmentNavId,
                nav.UserChanged,
                nav.DateChanged
            };
            dataAccess.SaveData<dynamic>("spInvestmentNavs_UpdateItem", p, dbName);
        }

        public void RemoveInvestmentNav(int investmentNavId)
        {
            var p = new
            {
                InvestmentNavId = investmentNavId
            };
           dataAccess.SaveData<dynamic>("spInvestmentNavs_RemoveItem", p, dbName);
        }

        public int InsertNewPeEntityAccount(PeEntityAccount account)
        {
            account.UserChanged = windowsUser.WindowsUser;
            account.DateChanged = DateTime.Now;
            var p = new
            {
                account.Country,
                account.Currency,
                account.Fristigkeit,
                account.Iban,
                account.Name,
                account.PeEntityId,
                account.Sector,
                account.UserChanged,
                account.DateChanged
            };
            return dataAccess.InsertData<dynamic>("spPeEntityAccounts_InsertItem", p, dbName);
        }
        public void UpdatePeEntiyAccount(PeEntityAccount account)
        {
            account.UserChanged = windowsUser.WindowsUser;
            account.DateChanged = DateTime.Now;
            var p = new
            {
                account.PeEntityAccountId,
                account.Country,
                account.Currency,
                account.Fristigkeit,
                account.Iban,
                account.Name,
                account.PeEntityId,
                account.Sector,
                account.UserChanged,
                account.DateChanged
            };
            dataAccess.SaveData<dynamic>("spPeEntityAccounts_UpdateItem", p, dbName);
        }
        public void RemovePeEntityAccount(int peEntityAccountId)
        {
            var p = new
            {
                PeEntityAccountId = peEntityAccountId
            };
            dataAccess.SaveData<dynamic>("spPeEntityAccounts_RemoveItem", p, dbName);
        }

        public int InsertNewPeEntityInvestor(PeEntityInvestor investor)
        {
            investor.UserChanged = windowsUser.WindowsUser;
            investor.DateChanged = DateTime.Now;
            var p = new
            {
               investor.PeEntityId,
               investor.Commitment,
               investor.Sektor,
               investor.Country, 
               investor.UserChanged,
               investor.DateChanged
            };
            return dataAccess.InsertData<dynamic>("spPeEntityInvestors_InsertItem", p, dbName);
        }
        public void UpdatePeEntiyInvestor(PeEntityInvestor investor)
        {
            investor.UserChanged = windowsUser.WindowsUser;
            investor.DateChanged = DateTime.Now;
            var p = new
            {
               investor.PeEntityInvestorId,
               investor.PeEntityId,
               investor.Commitment,
               investor.Country,
               investor.Sektor,
               investor.UserChanged,
               investor.DateChanged
            };
            dataAccess.SaveData<dynamic>("spPeEntityInvestors_UpdateItem", p, dbName);
        }
        public void RemovePeEntityInvestor(int peEntityInvestorId)
        {
            var p = new
            {
                PeEntityInvestorId = peEntityInvestorId
            };
            dataAccess.SaveData<dynamic>("spPeEntityInvestors_RemoveItem", p, dbName);
        }
 

        public int InsertNewInvestmentCashflow(InvestmentCashflow cashflow)
        {
            cashflow.UserChanged = windowsUser.WindowsUser;
            cashflow.DateChanged = DateTime.Now;
            var p = new
            {
                cashflow.CapitalGain,
                cashflow.CashflowAmount,
                cashflow.CashflowDate,
                cashflow.CashflowDescription,
                cashflow.CashflowType,
                cashflow.Dividends,
                cashflow.Interests,
                cashflow.InvestmentId,
                cashflow.LookbackInterests,
                cashflow.OtherIncome,
                cashflow.PartnershipExpenses,
                cashflow.Recallable,
                cashflow.CarriedInterests,
                cashflow.ReturnOfCapital,
                cashflow.WithholdingTax, 
                cashflow.UserChanged,
                cashflow.DateChanged
            };
            return  dataAccess.InsertData<dynamic>("spInvestmentCashflows_InsertItem", p, dbName);
        }
        public void UpdateInvestmentCashflow(InvestmentCashflow cashflow)
        {
            cashflow.UserChanged = windowsUser.WindowsUser;
            cashflow.DateChanged = DateTime.Now;
            var p = new
            {
                cashflow.InvestmentCashflowId,
                cashflow.CapitalGain,
                cashflow.CashflowAmount,
                cashflow.CashflowDate,
                cashflow.CashflowDescription,
                cashflow.CashflowType,
                cashflow.Dividends,
                cashflow.Interests,
                cashflow.InvestmentId,
                cashflow.LookbackInterests,
                cashflow.OtherIncome,
                cashflow.PartnershipExpenses,
                cashflow.Recallable,
                cashflow.CarriedInterests,
                cashflow.ReturnOfCapital,
                cashflow.WithholdingTax,
                cashflow.UserChanged,
                cashflow.DateChanged
            };
             dataAccess.SaveData<dynamic>("spInvestmentCashflows_UpdateItem", p, dbName);
        }
        public void RemoveInvestmentCashflow(int investmentCashflowId)
        {
            var p = new
            {
                InvestmentCashflowId = investmentCashflowId
            };
             dataAccess.SaveData<dynamic>("spInvestmentCashflows_RemoveItem", p, dbName);
        }

        public int InsertNewPeEntityCashflow(PeEntityCashflow cashflow)
        {
            cashflow.UserChanged = windowsUser.WindowsUser;
            cashflow.DateChanged = DateTime.Now;
            var p = new
            {
                cashflow.CapitalGain,
                cashflow.CashflowAmount,
                cashflow.CashflowDate,
                cashflow.CashflowDescription,
                cashflow.CashflowType,
                cashflow.Dividends,
                cashflow.Interests,
                cashflow.PeEntityId,
                cashflow.LookbackInterests,
                cashflow.OtherIncome,
                cashflow.PartnershipExpenses,
                cashflow.Recallable,
                cashflow.ReturnOfCapital,
                cashflow.WithholdingTax,
                cashflow.UserChanged,
                cashflow.DateChanged
            };
            return  dataAccess.InsertData<dynamic>("spPeEntityCashflows_InsertItem", p, dbName);
        }
        public void UpdatePeEntityCashflow(PeEntityCashflow cashflow)
        {
            cashflow.UserChanged = windowsUser.WindowsUser;
            cashflow.DateChanged = DateTime.Now;
            var p = new
            {
                cashflow.PeEntityCashflowId,
                cashflow.CapitalGain,
                cashflow.CashflowAmount,
                cashflow.CashflowDate,
                cashflow.CashflowDescription,
                cashflow.CashflowType,
                cashflow.Dividends,
                cashflow.Interests,
                cashflow.PeEntityId,
                cashflow.LookbackInterests,
                cashflow.OtherIncome,
                cashflow.PartnershipExpenses,
                cashflow.Recallable,
                cashflow.ReturnOfCapital,
                cashflow.WithholdingTax,
                cashflow.UserChanged,
                cashflow.DateChanged
            };
             dataAccess.SaveData<dynamic>("spPeEntityCashflows_UpdateItem", p, dbName);
        }
        public void RemovePeEntityCashflow(int PeEntityCashflowId)
        {
            var p = new
            {
                PeEntityCashflowId = PeEntityCashflowId
            };
             dataAccess.SaveData<dynamic>("spPeEntityCashflows_RemoveItem", p, dbName);
        }


        public PeEntity GetNewDefaultPeEntity()
        {
            PeEntity entity = new PeEntity();
            entity.Organisation = "GmbH & Co.KG";
            entity.Recht = "DE";
            entity.Strasse = "Am Pilgerrain 17";
            entity.Plz = "61352";
            entity.Ort = "Bad Homburg";
            entity.Country = "DE";
            entity.Art_Mittelanlage =  dropDownServices.GetDefaultValue("art_mittel");
            entity.Art_Ertragsverwendung = dropDownServices.GetDefaultValue("art_ertrag");
            entity.Art_Notiz = dropDownServices.GetDefaultValue("notierung");
            entity.Art_Ruecknahme = "nur Laufzeitende";
            entity.Wertgesichert = "nein";
            entity.Currency = "EUR";
            entity.Typ = dropDownServices.GetDefaultValue("Typ");
            return entity;
        }

        public Investment GetNewDefaultInvestment()
        {
            Investment investment = new Investment();
            investment.Beteiligungsart = dropDownServices.GetDefaultValue("art_beteiligung");
            investment.Country = "LU";
            return investment;
        }
        public bool CheckIbanSum(string iban)
        {
            return CustomChecks.IbanChecksumCheck(iban);
        }

        public string FormatIban(string iban)
        {
            return CustomChecks.FormatIban(iban);
        }

        public decimal GetPeEntityInvestorSum(int peEntityId)
        {
            return dataAccess.GetSumPeEntiyInvestorSum(peEntityId, dbName);
        }

        public int InsertNewPeEntity(PeEntity entity)
        {
            entity.UserChanged = windowsUser.WindowsUser;
            entity.DateChanged = DateTime.Now;
            var p = new
            {
                 entity.PortfolioName,
                 entity.PortfolioLegalEntity,
                 entity.TotalCommitment,
                 entity.TotalCommitmentInEuro,
                 entity.Currency,
                 entity.TaxNumber,
                 entity.Lei,
                 entity.BbkInstitutsnummer,
                 entity.Recht,
                 entity.Organisation,
                 entity.Typ,
                 entity.Art_Inhaber,
                 entity.Art_Mittelanlage,
                 entity.Art_Ertragsverwendung,
                 entity.Art_Laufzeit,
                 entity.Art_Ruecknahme,
                 entity.Art_Notiz,
                 entity.Wertgesichert,
                 entity.Laufzeitbeginn,
                 entity.Laufzeitende,
                 entity.Strasse,
                 entity.Postfach,
                 entity.Plz,
                 entity.Ort,
                 entity.Country,
                 entity.GIIN, 
                 entity.Finanzamt,
                 entity.SteuerlicheEinordnung,
                 entity.AwvNummer, 
                 entity.UserChanged,
                 entity.DateChanged, 
                 entity.Handelsregisternummer,
                 entity.Amtsgericht,
                 entity.BaFinNummer,
                 entity.PsPlusBeteiligungsnummer,
                 entity.EIN,
                 entity.Strategie,
                 entity.Region,
                 entity.AuflageJahr,
                 entity.FinalClosing,
                 entity.KVG
            };
            return  dataAccess.InsertData<dynamic>("spPeEntities_InsertItem", p, dbName);
        }

        public void UpdatePeEntiy(PeEntity entity)
        {
            entity.UserChanged = windowsUser.WindowsUser;
            entity.DateChanged = DateTime.Now;
            var p = new
            {
                entity.PeEntityId,
                entity.PortfolioName,
                entity.PortfolioLegalEntity,
                entity.TotalCommitment,
                entity.TotalCommitmentInEuro,
                entity.Currency,
                entity.TaxNumber,
                entity.Lei,
                entity.BbkInstitutsnummer,
                entity.Recht,
                entity.Organisation,
                entity.Typ,
                entity.Art_Inhaber,
                entity.Art_Mittelanlage,
                entity.Art_Ertragsverwendung,
                entity.Art_Laufzeit,
                entity.Art_Ruecknahme,
                entity.Art_Notiz,
                entity.Wertgesichert,
                entity.Laufzeitbeginn,
                entity.Laufzeitende,
                entity.Strasse,
                entity.Postfach,
                entity.Plz,
                entity.Ort,
                entity.Country,
                entity.GIIN,
                entity.Finanzamt,
                entity.SteuerlicheEinordnung,
                entity.AwvNummer,
                entity.UserChanged,
                entity.DateChanged,
                entity.Handelsregisternummer,
                entity.Amtsgericht,
                entity.BaFinNummer,
                entity.PsPlusBeteiligungsnummer,
                entity.EIN,
                entity.Strategie,
                entity.Region,
                entity.AuflageJahr,
                entity.FinalClosing,
                entity.KVG
            };
             dataAccess.SaveData<dynamic>("spPeEntities_UpdateItem", p, dbName);
        }

        public void RemovePeEntity(int peEntityId)
        {
            var p = new
            {
                PeEntityId = peEntityId
            };
             dataAccess.SaveData<dynamic>("spPeEntities_RemoveItem", p, dbName);
        }
        public int InsertNewPeEntityNav(PeEntityNav nav)
        {
            nav.UserChanged = windowsUser.WindowsUser;
            nav.DateChanged = DateTime.Now;
            var p = new
            {
                 nav.Amount,
                 nav.NavDate,
                 nav.PeEntityId,
                 nav.UserChanged,
                 nav.DateChanged
 
            };
            return  dataAccess.InsertData<dynamic>("spPeEntityNavs_InsertItem", p, dbName);
        }

        public void UpdatePeEntityNav(PeEntityNav nav)
        {
            nav.UserChanged = windowsUser.WindowsUser;
            nav.DateChanged = DateTime.Now;
            var p = new
            {
                nav.Amount,
                nav.NavDate,
                nav.PeEntityId,
                nav.PeEntityNavId,
                 nav.UserChanged,
                nav.DateChanged
            };
             dataAccess.SaveData<dynamic>("spPeEntityNavs_UpdateItem", p, dbName);
        }

        public void RemovePeEntityNav(int peEntityNavId)
        {
            var p = new
            {
                PeEntityNavId = peEntityNavId
            };
             dataAccess.SaveData<dynamic>("spPeEntityNavs_RemoveItem", p, dbName);
        }

        /// <summary>
        /// The function expects the date of the last Nav (or null)
        /// it returns the end of the quarter following the last NAV or (if last Nav was null) the end of the previous quarter
        /// </summary>
        /// <param name="lastNavDate"></param>
        /// <returns></returns>
        public DateTime? GetNextNav(DateTime? lastNavDate)
        {             
            DateTime lastQuarter = dateFunctions.PreviousQuarter(DateTime.Now);
            if (lastNavDate == null) return lastQuarter;
            lastQuarter = dateFunctions.NextQuarter((DateTime)lastNavDate);            

            if (lastQuarter >DateTime.Now) return null;
            return lastQuarter;
        }

        public List<Cashflow> ConvertPeEntityCashflow(List<PeEntityCashflow> cashflows)
        {
            List<Cashflow> c = new();
            foreach (PeEntityCashflow cf in cashflows)
            {
                c.Add(new Cashflow()
                {
                    CapitalGain = cf.CapitalGain,
                    CashflowAmount = cf.CashflowAmount,
                    CashflowDate = cf.CashflowDate,
                    CashflowType = cf.CashflowType,
                    Dividends = cf.Dividends,
                    Interests = cf.Interests,
                    LookbackInterests = cf.LookbackInterests,
                    OtherIncome = cf.OtherIncome,
                    PartnershipExpenses = cf.PartnershipExpenses,
                    Recallable = cf.Recallable,
                    ReturnOfCapital = cf.ReturnOfCapital,
                    WithholdingTax = cf.WithholdingTax
                });
            }
            return c;
        }

        public List<Nav> ConvertPeEntityNav(List<PeEntityNav> navs)
        {
            List<Nav> n = new();
            foreach(PeEntityNav nav in navs)
            {
                n.Add(new Nav
                {
                    Amount = nav.Amount,
                    NavDate = nav.NavDate
                });
            }
            return n;

        }

        public List<Cashflow> ConvertInvestmentCashflow(List<InvestmentCashflow> cashflows)
        {
            List<Cashflow> c = new List<Cashflow>();
            foreach (InvestmentCashflow cf in cashflows)
            {
                c.Add(new Cashflow()
                {
                    CapitalGain = cf.CapitalGain,
                    CashflowAmount = cf.CashflowAmount,
                    CashflowDate = cf.CashflowDate,
                    CashflowType = cf.CashflowType,
                    Dividends = cf.Dividends,
                    Interests = cf.Interests,
                    LookbackInterests = cf.LookbackInterests,
                    OtherIncome = cf.OtherIncome,
                    PartnershipExpenses = cf.PartnershipExpenses,
                    Recallable = cf.Recallable,
                    ReturnOfCapital = cf.ReturnOfCapital,
                    WithholdingTax = cf.WithholdingTax
                });
            }
            return c;
        }

        public List<Nav> ConvertInvestmentNav(List<InvestmentNav> navs)
        {
            List<Nav> n = new();
            foreach (InvestmentNav nav in navs)
            {
                n.Add(new Nav()
                {
                    Amount = nav.Amount,
                    NavDate = nav.NavDate
                });
            }
            return n;
        }

        public string GetUserName()
        {
            return  dataAccess.GetUserName(dbName);
        }

        public List<AwvInformationen> GetAwvInformationenAsync(DateTime from, DateTime to)
        {
            // dataAccess.LoadData<AwvInformationen, dynamic>("spInvestorCashflows_AwvMeldung", new { StartDate = from, EndDate=to }, dbName);
            return  dataAccess.LoadAwvInformation(from, to, dbName);
           
        }

        public List<Bllw_User> GetAllUsers()
        {
            return dataAccess.LoadData<Bllw_User, dynamic>("spBllw_Users_ReadAll", new { }, dbName);
        }

        /// <summary>
        /// This functions gets a User from Table Bllw_Users including the Roles for the user
        /// If the user is not found, a new Bllw_User record is added to the database
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>

        public Bllw_User GetWindowsUser(string username)
        {
            Bllw_User user = dataAccess.LoadSingleItem<Bllw_User, dynamic>("spBllw_Users_ReadUser", new { WindowsUser = username }, dbName);
            if (user is null)
            {
                // insert new User
                user = new Bllw_User();
                user.WindowsUser = username;

                var p = new
                {
                    user.DisplayName,
                    user.WindowsUser
                };
                user.UserId = dataAccess.InsertData<dynamic>("spBllw_Users_InsertItem", p, dbName);
                user.UserRoles = new();
                windowsUser = user;
                return user;
            }
            // user found ==> add roles
            List<Bllw_Role> roles = GetRolesForUser(user.UserId);
            user.UserRoles = roles;
            windowsUser = user;
            return user;
        }

        public List<Bllw_Role> GetRolesForUser(int userId)
        {
            return dataAccess.LoadData<Bllw_Role, dynamic>("spBllw_Roles_ReadAllForUser", new { UserId = userId }, dbName);
        }

        public List<Bllw_Role> GetAllRoles()
        {
            return dataAccess.LoadData<Bllw_Role, dynamic>("spBllw_Roles_ReadAll", new { }, dbName);
        }
        public void UpdateBllwUser(Bllw_User user)
        {
            var p = new
            {
                user.UserId,
                user.WindowsUser,
                user.DisplayName        
            };
            dataAccess.SaveData<dynamic>("spBllw_Users_UpdateItem", p, dbName);
        }

        public void DeleteBllwUser(int userId)
        {
            var p = new
            {
                UserId = userId
            };
            dataAccess.SaveData<dynamic>("spBllw_Users_RemoveItem", p, dbName);
        }

        public void UpdateRolesForUser(int userId, List<Bllw_Role> roles)
        {
            dataAccess.UpdateUserRoles(userId, roles, dbName);
        }

        public string GetAppState(AppState appState)
        {
            DynamicParameters p = new();
            p.Add("UserId", appState.UserId);
            p.Add("ControlId", appState.ControlId);

            return dataAccess.GetAppstate("spAppStateGetByUserIdAndControlId", p, dbName);
        }

        public void InsertOrUpdateAppState(AppState appState)
        {
            DynamicParameters p = new();
            p.Add("UserId", appState.UserId);
            p.Add("ControlId", appState.ControlId);
            p.Add("Controlstate", appState.Controlstate);

            try
            {
                dataAccess.SaveData<dynamic>("spAppState_InsertOrUpdate", p, dbName);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }
        }

        public int InsertNewInvestor(Investor investor)
        {
            investor.UserChanged = windowsUser.WindowsUser;
            investor.DateChanged = DateTime.Now;

            var p = new
            {
                investor.InvestorName,
                investor.DatevId,
                investor.InvestorNummer,
                investor.InvestorAdressId,
                investor.InvestorKontaktId,
                investor.SteuerId,
                investor.Steuernummer,
                investor.Finanzamt,
                investor.Rechtsform,
                investor.Registergericht,
                investor.Handelsregisternummer,
                investor.Geburtsort,
                investor.Geburtsdatum,
                investor.Telefon,
                investor.Telefax,
                investor.Email_1,
                investor.Email_2,
                investor.Email_3,
                investor.Signatur_1,
                investor.Signatur_2,
                investor.Position_1,
                investor.Position_2,
                investor.Kontaktname,
                investor.KontoTyp,
                investor.Confidential,
                investor.UseEmail,
                investor.UseMail,
                investor.UseHqtSave,
                investor.UseTransferForm,
                investor.Remarks,
                investor.UserChanged,
                investor.DateChanged,
                investor.Datenraum,
                investor.Kundennummer,
                investor.Mifid,
                investor.WPHG,
                investor.Sektor,
                investor.Zeile_1,
                investor.Zeile_2,
                investor.Zeile_3, 
                investor.AdressZeile_1,
                investor.AdressZeile_2,
                investor.AdressZeile_3,
                investor.AdressZeile_4,
                investor.AdressZeile_5,
                investor.AdressZeile_6,
                investor.Anrede_1,
                investor.Anrede_2
            };

            return dataAccess.InsertData<dynamic>("spInvestors_InsertItem", p, dbName);
        }
        public void UpdateInvestor(Investor investor)
        {
            investor.UserChanged = windowsUser.WindowsUser;
            investor.DateChanged = DateTime.Now;

            var p = new
            {
                investor.InvestorName,
                investor.DatevId,
                investor.InvestorNummer,
                investor.InvestorAdressId,
                investor.InvestorKontaktId,
                investor.SteuerId,
                investor.Steuernummer,
                investor.Finanzamt,
                investor.Rechtsform,
                investor.Registergericht,
                investor.Handelsregisternummer,
                investor.Geburtsort,
                investor.Geburtsdatum,
                investor.Telefon,
                investor.Telefax,
                investor.Email_1,
                investor.Email_2,
                investor.Email_3,
                investor.Signatur_1,
                investor.Signatur_2,
                investor.Position_1,
                investor.Position_2,
                investor.Kontaktname,
                investor.KontoTyp,
                investor.Confidential,
                investor.UseEmail,
                investor.UseMail,
                investor.UseHqtSave,
                investor.UseTransferForm,
                investor.Remarks,
                investor.UserChanged,
                investor.DateChanged,
                investor.Datenraum,
                investor.Kundennummer,
                investor.Mifid,
                investor.WPHG,
                investor.Sektor,
                investor.Zeile_1,
                investor.Zeile_2,
                investor.Zeile_3,
                investor.AdressZeile_1,
                investor.AdressZeile_2,
                investor.AdressZeile_3,
                investor.AdressZeile_4,
                investor.AdressZeile_5,
                investor.AdressZeile_6,
                investor.Anrede_1,
                investor.Anrede_2,
                investor.InvestorId               
            };

            dataAccess.SaveData<dynamic>("spInvestors_UpdateItem", p, dbName);
        }

        public int InsertNewInvestorKontakt(InvestorKontakt contact)
        {
            contact.UserChanged = windowsUser.WindowsUser;
            contact.DateChanged = DateTime.Now;

            var p = new
            {
                contact.AdressZusatz,
                contact.DateChanged,
                contact.Firma,
                contact.Kontaktname,
                contact.Land,
                contact.Ort,
                contact.Postleitzahl,
                contact.Strasse,
                contact.UserChanged, 
                contact.Email,
                contact.Telefon, 
                contact.Title,
                contact.Vorname,
                contact.Familienname
            };
            return dataAccess.InsertData<dynamic>("spInvestorKontakte_InsertItem", p, dbName);
        }
        public void UpdateInvestorKontakt(InvestorKontakt contact)
        {
            contact.UserChanged = windowsUser.WindowsUser;
            contact.DateChanged = DateTime.Now;

            var p = new
            {
                contact.AdressZusatz,
                contact.DateChanged,
                contact.Firma,
                contact.Kontaktname,
                contact.Land,
                contact.Ort,
                contact.Postleitzahl,
                contact.Strasse,
                contact.UserChanged,
                contact.InvestorKontaktId,
                contact.Email,
                contact.Telefon,
                contact.Title,
                contact.Vorname,
                contact.Familienname
            };
            dataAccess.SaveData<dynamic>("spInvestorKontakte_UpdateItem", p, dbName);
        }
        public int InsertNewInvestorAccount(InvestorAccount account)
        {
            account.UserChanged = windowsUser.WindowsUser;
            account.DateChanged = DateTime.Now;

            var p = new
            {
               account.Bankname,
               account.Bic,
               account.DateChanged,
               account.Iban,
               account.InvestorId,
               account.UserChanged,
               account.Waehrung
            };
            return dataAccess.InsertData<dynamic>("spInvestorAccounts_InsertItem", p, dbName);
        }
        public void UpdateInvestorAccount(InvestorAccount account)
        {
            account.UserChanged = windowsUser.WindowsUser;
            account.DateChanged = DateTime.Now;

            var p = new
            {
                account.Bankname,
                account.Bic,
                account.DateChanged,
                account.Iban,
                account.InvestorId,
                account.UserChanged,
                account.Waehrung,
                account.InvestorAccountId
            };
             dataAccess.SaveData<dynamic>("spInvestorAccounts_UpdateItem", p, dbName);
        }

        public List<Investor> GetAllInvestors()
        {
            var result = dataAccess.LoadData<Investor, dynamic>("spInvestors_GetAllInvestors", new { }, dbName);
            foreach(Investor investor in result)
            {
                investor.KontaktInfo = GetInvestorKontaktById(investor.InvestorKontaktId);
                investor.InvestorInfo = GetInvestorKontaktById(investor.InvestorAdressId);
            }
            return result;
        }

        public InvestorKontakt GetInvestorKontaktById(int InvestorKontaktId)
        {
            return dataAccess.LoadSingleItem<InvestorKontakt, dynamic>("spInvestorKontakte_GetInvestorKontaktById", new { InvestorKontaktId }, dbName);
        }

        public Investor GetInvestorById(int InvestorId)
        {
            return dataAccess.LoadSingleItem<Investor, dynamic>("spInvestors_GetInvestorById", new { InvestorId }, dbName);
        }

        public List<InvestorAccount> GetInvestorAccountsForInvestor(int investorId)
        {
            var result = dataAccess.LoadData<InvestorAccount, dynamic>("spInvestorAccounts_ReadAllForInvestor", new { InvestorId = investorId }, dbName);
            foreach (InvestorAccount account in result)
            {
                account.Iban = CustomChecks.FormatIban(account.Iban);
            }
            return result;
        }

        public List<InvestorKontakt> GelAllInvestorKontakts()
        {
            var result = dataAccess.LoadData<InvestorKontakt, dynamic>("spInvestorKontakte_GetAllInvestorKontakte", new {  }, dbName);
            return result;
        }

        public void TruncateAllInvestorTables()
        {
            dataAccess.TruncateAllInvestorTables("spInvestoren_TruncateAllTables", dbName);
        }

        public List<InvestorAccount> GetAllInvestorAccountsForInvestorId(int investorId)
        {
            var result = dataAccess.LoadData<InvestorAccount, dynamic>("spInvestorAccounts_ReadAllForInvestor", new { InvestorId = investorId }, dbName);
            foreach (InvestorAccount account in result)
            {
                account.Iban = CustomChecks.FormatIban(account.Iban);
            }
            return result;
        }
        public void RemoveInvestorAccount(int InvestorAccountId)
        {
            dataAccess.SaveData<dynamic>("spInvestorAccounts_RemoveItem", new { InvestorAccountId }, dbName);
        }

        public int InsertCommitment(Commitment commitment)
        {
            commitment.UserChanged = windowsUser.WindowsUser;
            commitment.DateChanged = DateTime.Now;

            var p = new
            {
                commitment.PeEntityId,
                commitment.DateClosed,
                commitment.DateChanged,
                commitment.PresentationSent,
                commitment.DateSent,
                commitment.InvestorId,
                commitment.UserChanged,
                commitment.DateSigned,
                commitment.AmountClosedInEuro,
                commitment.AmountClosed,
                commitment.InvestorOpenBalance,
                commitment.AmountRequested,
                commitment.CommitmentStatus
            };
            return dataAccess.InsertData<dynamic>("spCommitments_InsertItem", p, dbName);
        }

        public void UpdateCommitment(Commitment commitment)
        {
            commitment.UserChanged = windowsUser.WindowsUser;
            commitment.DateChanged = DateTime.Now;

            var p = new
            {
                commitment.PeEntityId,
                commitment.PresentationSent,
                commitment.DateClosed,
                commitment.DateChanged,
                commitment.DateSent,
                commitment.InvestorId,
                commitment.UserChanged,
                commitment.DateSigned,
                commitment.AmountClosedInEuro,
                commitment.AmountClosed,
                commitment.AmountRequested,
                commitment.InvestorOpenBalance,
                commitment.CommitmentStatus, 
                commitment.CommitmentId
            };
            dataAccess.SaveData<dynamic>("spCommitments_UpdateItem", p, dbName);
        }
        public void RemoveCommitment(int CommitmentId)
        {
            dataAccess.SaveData<dynamic>("spCommitments_RemoveItem", new { CommitmentId }, dbName);
        }

        public List<Commitment> GetCommitmentsForPeEntity(int PeEntityId)
        {
            var result = dataAccess.LoadData<Commitment, dynamic>("spCommitments_GetCommitmentsForPeEntityId", new { PeEntityId }, dbName);
            decimal sum = result.Sum(r => r.AmountClosed);
            if (sum > 0)
            {
                foreach(Commitment commitment in result)
                {
                    commitment.AmountInPercent = Math.Round(commitment.AmountClosed / sum,4);
                }
            }
            return CompleteCommitments(result);
        }

        private List<Commitment> CompleteCommitments(List<Commitment> result)
        {
            foreach(Commitment c in result)
            {
                c.Entity = GetPeEntityById(c.PeEntityId);
                c.Investor = GetInvestorById(c.InvestorId);
            }
            return result;
        }

        public List<Commitment> GetCommitmentsForInvestor(int InvestorId)
        {
            var result = dataAccess.LoadData<Commitment, dynamic>("spCommitments_GetCommitmentsForInvestorId", new { InvestorId }, dbName);
            decimal sum = result.Sum(r => r.AmountClosed);
            if (sum > 0)
            {
                foreach (Commitment commitment in result)
                {
                    commitment.InPercentForInvestor = Math.Round(commitment.AmountClosed / sum, 4);
                }
            }
            return CompleteCommitments(result);
        }

        public string GetComplianceInformationForPeEntity(int PeEntityId)
        {
            var p = new DynamicParameters();
            p.Add("PeEntityId", PeEntityId);

            return dataAccess.GetComplianceReport("spComplianceReports_GetReportForPeEntityId", p, dbName);
        }

        public void SaveComplianceInformationForPeEntity(int PeEntityId, string jsonText)
        {
            var p = new DynamicParameters();
            p.Add("ReportId", PeEntityId);
            p.Add("Content", jsonText);

            dataAccess.InsertOrUpdateComplianceReport("spComplianceReports_InsertOrUpdate", p, dbName);
        }
        #region Kvg
        public int InsertKapitalverwaltungsGesellschaft(Kapitalverwaltung kvg)
        {
            var p = new
            {
             kvg.KvgAbsender,
             kvg.KvgStrasse,
             kvg.KvgLand,
             kvg.KvgOrt,
             kvg.KvgLogo,
             kvg.KvgName,
             kvg.KvgPlz,
             kvg.KvgFooter1,
             kvg.KvgFooter2,
             kvg.KvgFooter3,
             kvg.KvgAnnahmeLetter,
             kvg.KvgClosingLetter,
             kvg.KvgCapitalCallLetter,
             kvg.KvgDistributionLetter,
             kvg.KvgCombinationLetter,
             kvg.KvgAnnahmeFilename,
             kvg.KvgClosingFilename,
             kvg.KvgCapitalCallFilename,
             kvg.KvgDistributionFilename,
             kvg.KvgCombinationFilename
            };
            return dataAccess.InsertData<dynamic>("spKapitalverwaltungGesellschaft_InsertItem", p, dbName);
        }

        public void UpdateKapitalverwaltungsGesellschaft(Kapitalverwaltung kvg)
        {
            var p = new
            {
                kvg.KvgAbsender,
                kvg.KvgLand,
                kvg.KvgStrasse,
                kvg.KvgOrt,
                kvg.KvgLogo,
                kvg.KvgName,
                kvg.KvgPlz,
                kvg.KvgFooter1,
                kvg.KvgFooter2,
                kvg.KvgFooter3,
                kvg.KvgAnnahmeLetter,
                kvg.KvgClosingLetter,
                kvg.KvgCapitalCallLetter,
                kvg.KvgDistributionLetter,
                kvg.KvgCombinationLetter,
                kvg.KvgAnnahmeFilename,
                kvg.KvgClosingFilename,
                kvg.KvgCapitalCallFilename,
                kvg.KvgDistributionFilename,
                kvg.KvgCombinationFilename,
                kvg.KvgId
            };
            dataAccess.SaveData<dynamic>("spKapitalverwaltungsGesellschaft_UpdateItem", p, dbName);
        }

        public List<Kapitalverwaltung> GetAllKapitalverwaltungsGesellschaften()
        {
            return dataAccess.LoadData<Kapitalverwaltung, dynamic>("spKapitalverwaltungsGesellschaften_GetAll", new { }, dbName);
        }

        public int InsertKvgContact(KapitalverwaltungKontakt contact)
        {
            var p = new
            {
                contact.KvgContactName,
                contact.KvgContactEmail,
                contact.KvgContactTelefon, 
                contact.KvgId
            };
             return dataAccess.InsertData<dynamic>("spKvgContacts_InsertItem", p, dbName);
        }

        public void UpdateKvgContact(KapitalverwaltungKontakt contact)
        {
            var p = new
            {
                contact.KvgContactName,
                contact.KvgContactEmail,
                contact.KvgContactTelefon, 
                contact.KvgId,
                contact.KvgSequence,
                contact.KvgContactId
            };
            dataAccess.SaveData<dynamic>("spKvgContact_UpdateItem", p, dbName);
        }

        public List<KapitalverwaltungKontakt> GetKapitalverwaltungKontaktsForKvg(int KvgId)
        {
            return dataAccess.LoadData<KapitalverwaltungKontakt, dynamic>("spKvgContacts_GetContactsForKvg", new { KvgId }, dbName);
        }

        public Kapitalverwaltung GetKvgById(int KvgId)
        {
            return dataAccess.LoadSingleItem<Kapitalverwaltung, dynamic>("spKapitalverwaltungsgesellschaften_GetItemById", new { KvgId }, dbName);           
        }

        #endregion

        #region InvestorSignatures
        public int InsertInvestorSignature(InvestorSignature investorSignature)
        {
            var p = new
            {
               investorSignature.InvestorId,
               investorSignature.ManagerName,
               investorSignature.ManagerRole,
               investorSignature.Sequence
            };
            return dataAccess.InsertData<dynamic>("spInvestorSignatures_InsertItem", p, dbName);
        }

        public void UpdateInvestorSignature(InvestorSignature investorSignature)

        {
            var p = new
            {
                investorSignature.InvestorId,
                investorSignature.ManagerName,
                investorSignature.ManagerRole,
                investorSignature.Sequence,
                investorSignature.InvestorSignatureId
            };
            dataAccess.SaveData<dynamic>("spInvestorSignatures_UpdateItem", p, dbName);
        }

        public void RemoveInvestorSignature(int InvestorSignatureId)
        {
          
            dataAccess.SaveData<dynamic>("spInvestorSignatures_RemoveItem", new {InvestorSignatureId}, dbName);
        }

        public List<InvestorSignature> GetAllInvestorSignaturesForId(int InvestorId)
        {
            return dataAccess.LoadData<InvestorSignature, dynamic>("spInvestorSignatures_GetAllInvestorSignaturesForId", new { InvestorId }, dbName);
        }

        public int InsertWirtBerechtigter(WirtBerechtigter wirtBerechtigter)
        {
            var p = new
            {
                wirtBerechtigter.InvestorId,
                wirtBerechtigter.Title,
                wirtBerechtigter.Vorname,
                wirtBerechtigter.Familienname,
                wirtBerechtigter.VollName,
                wirtBerechtigter.Sequence,
                wirtBerechtigter.Funktion,
                wirtBerechtigter.Strasse,
                wirtBerechtigter.Adresszusatz,
                wirtBerechtigter.Postleitzahl,
                wirtBerechtigter.Ort,
                wirtBerechtigter.Land,
                wirtBerechtigter.Geburtsort,
                wirtBerechtigter.Geburtstag,
                wirtBerechtigter.Steuernummer,
                wirtBerechtigter.SteuerLand
            };
            return dataAccess.InsertData<dynamic>("spWirtBerechtigte_InsertItem", p, dbName);
        }

        public void UpdateWirtBerechtigter(WirtBerechtigter wirtBerechtigter)
        {
            var p = new
            {
                wirtBerechtigter.InvestorId,
                wirtBerechtigter.Title,
                wirtBerechtigter.Vorname,
                wirtBerechtigter.Familienname,
                wirtBerechtigter.VollName,
                wirtBerechtigter.Sequence,
                wirtBerechtigter.Funktion,
                wirtBerechtigter.Strasse,
                wirtBerechtigter.Adresszusatz,
                wirtBerechtigter.Postleitzahl,
                wirtBerechtigter.Ort,
                wirtBerechtigter.Land,
                wirtBerechtigter.Geburtsort,
                wirtBerechtigter.Geburtstag,
                wirtBerechtigter.Steuernummer,
                wirtBerechtigter.SteuerLand,
                wirtBerechtigter.WirtBerechtigterId
            };
            dataAccess.SaveData<dynamic>("spWirtBerechtigte_UpdateItem", p, dbName);
        }

        public List<WirtBerechtigter> GetAllWirtBerechtigterForInvestorId(int InvestorId)
        {
            return dataAccess.LoadData<WirtBerechtigter, dynamic>("spWirtBerechtigte_GetAllWirtBerechtigteForId", new { InvestorId }, dbName);
        }

        public void RemoveWirtBerechtigter(int WirtBerechtigterId)
        {
            dataAccess.SaveData<dynamic>("spWirtBerechtigte_RemoveItem", new { WirtBerechtigterId }, dbName);
        }


        #endregion
    }
}
