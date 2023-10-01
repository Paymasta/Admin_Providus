using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity
{
    public class ArticleOption : BaseEntity
    {
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
