using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMasta.Utilities
{
    public static class CommonUtils
    {
        public static string GenerateOtp()
        {
            string code = "0";
            Random rnd = new Random();
            code = rnd.Next(1000, 9999).ToString();
            return code;
        }
    }
}
