using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.Faq
{
    public class FaqMaster : BaseEntity
    {
       
        public string QuestionText { get; set; }
        public string Detail { get; set; }

    }
}
