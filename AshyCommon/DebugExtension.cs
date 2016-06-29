using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AshyCommon
{
    public static class DebugExtension
    {
        public static bool IsSet(this object o) => !ReferenceEquals(o, null);

        public static bool IsNotSet(this object o) => ReferenceEquals(o, null);
    }
}
