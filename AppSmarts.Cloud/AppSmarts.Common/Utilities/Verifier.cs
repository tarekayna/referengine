using System;

namespace AppSmarts.Common.Utilities
{
    public static class Verifier
    {
        public static void IsNotNullOrEmpty(object obj, string message)
        {
            if (obj is string)
            {
                if (string.IsNullOrEmpty((string) obj)) throw new ArgumentNullException(message);
            }
            else
            {
                if (obj == null) throw new ArgumentNullException(message);
            }
        }
    }
}
