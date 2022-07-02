using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class Bllw_User
    {
        public int UserId { get; set; }
        public string WindowsUser { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public List<Bllw_Role> UserRoles { get; set; } = new();
    }
}
