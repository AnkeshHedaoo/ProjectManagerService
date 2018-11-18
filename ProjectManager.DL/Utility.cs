using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.DL
{
    public static class Utility
    {
        public static DateTime? GetFormattedDate(string date)
        {
            if (string.IsNullOrEmpty(date)) { return null; }
            return Convert.ToDateTime(date);
        }
    }
}
