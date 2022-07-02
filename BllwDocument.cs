using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    public class BllwDocument
    {
        public int DocumentId { get; set; }
        public string Extension { get; set; }
        public string Filename { get; set; }
        public byte[] Content { get; set; }
    }
}
