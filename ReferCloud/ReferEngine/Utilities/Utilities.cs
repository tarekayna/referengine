using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReferEngine.Utilities
{
    public static class Util
    {
        public static bool TryConvertToInt(string str, out int result)
        {
            int actualResult = 0;
            try
            {
                actualResult = Convert.ToInt32(str);
                result = actualResult;
                return true;
            }
            catch (FormatException)
            {
                result = actualResult;
                return false;
            }
        }
    }
}