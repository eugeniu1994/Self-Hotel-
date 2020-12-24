using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatiFacturiOnline
{
    public static class AIMExtensions
    {
        public static bool IsNullOrWhiteSpace(string s)
        {
            return (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(s.Trim()));
        }
    }
}
