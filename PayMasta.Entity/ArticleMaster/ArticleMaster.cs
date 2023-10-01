using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity
{
    public class ArticleMaster : BaseEntity
    {
        public string ArticleText { get; set; }
        public double PriceMoney { get; set; }
        public string Option1Text { get; set; }
        public string Option2Text { get; set; }
        public string Option3Text { get; set; }
        public string Option4Text { get; set; }
        public int CorrectOption { get; set; }
    }
}
