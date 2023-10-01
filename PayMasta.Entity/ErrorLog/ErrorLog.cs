using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.ErrorLog
{
    public class ErrorLog
    {
        public long ErrorId { get; set; }
        public string ErrorMessage { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string JsonData { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
