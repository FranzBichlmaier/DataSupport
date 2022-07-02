using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class BundesbankInformationen
    {
        public DateTime CreatedOn { get; set; }
        public List<BundesbankPeEntity> Entities { get; set; }
    }
}
