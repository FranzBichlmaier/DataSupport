using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class AppState
    {
        public int AppStateId { get; set; }
        public int UserId { get; set; }
        public string ControlId { get; set; }
        public string Controlstate { get; set; }

    }
}
