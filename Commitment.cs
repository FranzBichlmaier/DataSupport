using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class Commitment: INotifyPropertyChanged
    {
        public int CommitmentId { get; set; }
        public int PeEntityId { get; set; }
        public int InvestorId { get; set; }
        public CommitmentStatus CommitmentStatus { get; set; }
        private DateTime? presentationSent;

        public DateTime? PresentationSent
        {
            get { return presentationSent; }
            set { presentationSent = value; }
        }

        public DateTime? DateSent { get; set; }
        private DateTime? dateSigned;

        public DateTime? DateSigned
        {
            get { return dateSigned; }
            set { dateSigned = value; NotifyPropertyChanged("DateSigned") ; }
        }

        public DateTime? DateClosed { get; set; }
        private decimal amountRequested;

        public decimal AmountRequested
        {
            get { return amountRequested; }
            set { amountRequested = value; NotifyPropertyChanged("AmountRequested"); }
        }

        private decimal amountClosed;

        public decimal AmountClosed
        {
            get { return amountClosed; }
            set { amountClosed = value; NotifyPropertyChanged("AmountClosed"); }
        }

        private decimal amountClosedInEuro;

        public decimal AmountClosedInEuro
        {
            get { return amountClosedInEuro; }
            set { amountClosedInEuro = value; NotifyPropertyChanged("AmountClosedInEuro"); }
        }
        private decimal amountInPercent;

        public decimal AmountInPercent
        {
            get { return amountInPercent; }
            set { amountInPercent = value; }
        }
        private decimal inPercentForInvestor;

        public decimal InPercentForInvestor
        {
            get { return inPercentForInvestor; }
            set { inPercentForInvestor = value; }
        }
        private decimal investorOpenBalance;

        public decimal InvestorOpenBalance
        {
            get { return investorOpenBalance; }
            set { investorOpenBalance = value; }
        }

        public CashflowSummary InvestorSummary { get; set; }

        public string StatusAsText { get; set; }

        public string UserChanged { get; set; }
        public DateTime? DateChanged { get; set; }
        public virtual PeEntity Entity { get; set; }
        public virtual Investor Investor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
