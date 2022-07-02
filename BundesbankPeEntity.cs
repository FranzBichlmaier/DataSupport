using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class BundesbankPeEntity
    {
        public PeEntity Entity { get; set; }
        public List<PeEntityAccount> EntityAccounts { get; set; }
        public List<PeEntityCashflow> EntityCashflows { get; set; }
        public List<PeEntityInvestor> EntityInvestors { get; set; }
        public List<PeEntityNav> EntityNavs { get; set; }
        public List<BundesbankInvestment> EntityInvestments { get; set; }
    }
}
