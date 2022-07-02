using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class Waehrung
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int NumCode { get; set; }
        public int decimals { get; set; }
        public string Name { get; set; }
    }
}
