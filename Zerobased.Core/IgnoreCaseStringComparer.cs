using System;
using System.Collections.Generic;

namespace Zerobased
{
    public class IgnoreCaseStringComparer : IEqualityComparer<string>
    {
        private static readonly Lazy<IgnoreCaseStringComparer> _instance = new Lazy<IgnoreCaseStringComparer>();

        public static IgnoreCaseStringComparer Instance => _instance.Value;

        public bool Equals(string x, string y)
        {
            return x?.Equals(y, StringComparison.CurrentCultureIgnoreCase) ?? y == null;
        }

        public int GetHashCode(string obj)
        {
            return obj?.ToUpper().GetHashCode() ?? int.MinValue;
        }
    }
}
