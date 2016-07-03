using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshyCore.Debug
{
    public static class Critical
    {
        public static T NoThrow<T>(this Func<T> func) where T : class
        {
            try
            {
                return func();
            }
            catch
            {
                return null;
            }
        }

        public static void NoThrow(this Action func)
        {
            try
            {
                func();
            }
            catch
            {
            }
        }
    }
}
