using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.TransactionLog
{
    public class TransactionLog : BaseEntity
    {
        public string LogType { get; set; }
        public string CreditAccount { get; set; }
        public string DebitAccount { get; set; }
        public string TransactionAmount { get; set; }
        public string TransactionReference { get; set; }
        public string TransactionName { get; set; }
        public string LogJson { get; set; }
        public string Detail { get; set; }
        public DateTime LogDate { get; set; }
    }
}
