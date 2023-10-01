using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities.EmailUtils
{
    public class EmailModel
    {
        public string TO { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public MemoryStream Stream { get; set; }
        public string AttachmentName { get; set; }
    }
}
