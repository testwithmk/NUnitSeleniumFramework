using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NunitSeleniumFramework.Utilities
{
    public class TestDataHelper
    {
        /** Generate dynamic data*/
        public static string GenerateLargePayload(int lengthA, int length1, int lengthSpecial, int lengthHtml)
        {
            return new string('A', lengthA) +
                   new string('9', length1) +
                   "!@#$%^&*()_+|}{:?>".PadRight(lengthSpecial, '!') +
                   "<select>alert('x')".PadRight(lengthHtml, '<');
        }
    }
}
