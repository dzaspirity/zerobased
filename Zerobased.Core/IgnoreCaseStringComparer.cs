using System;
using System.Collections.Generic;

namespace Zerobased
{
    public class IgnoreCaseStringComparer : IEqualityComparer<string>
    {
        private static readonly Lazy<IgnoreCaseStringComparer> _instance = new Lazy<IgnoreCaseStringComparer>();
        public static IgnoreCaseStringComparer Instance { get { return _instance.Value; } }

        public bool Equals(string x, string y)
        {
            return x == null ? (y == null) : x.Equals(y, StringComparison.CurrentCultureIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return obj == null ? int.MinValue : obj.ToUpper().GetHashCode();
        }
    }
}
