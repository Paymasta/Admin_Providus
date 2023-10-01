using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Entity.SubCategory
{
    public class SubCategory : BaseEntity
    {
        public long MainCategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}
