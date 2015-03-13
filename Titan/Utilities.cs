using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Titan
{
    internal static class Utilities
    {
        public static float TryParse(string s, float defaultValue)
        {
            float value;
            if (!float.TryParse(s, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        public static bool TryParse(string s, bool defaultValue)
        {
            bool value;
            if (!bool.TryParse(s, out value))
            {
                value = defaultValue;
            }

            return value;
        }

        public static bool IsNull<T>(this T @object)
        {
            return Equals(@object, null);
        }
    }
}
