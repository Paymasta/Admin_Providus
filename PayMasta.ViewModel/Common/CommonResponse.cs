using PayMasta.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.ViewModel.Common
{
    public class InvoiceNumberResponse
    {
        public InvoiceNumberResponse()
        {
            this.InvoiceNumber = string.Empty;
            this.AutoDigit = string.Empty;


        }
        public long Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string AutoDigit { get; set; }
        public Guid Guid { get; set; }
    }

    public class RandomInvoiceNumber : BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public string Pattern { get; set; }

    }
}
