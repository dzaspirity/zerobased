using System;
using System.Collections.Generic;
using System.Linq;

namespace Zerobased
{
    public static class ListInfoExtensions
    {
        /// <summary>
        /// Applys paging and sorting to sequence.
        /// </summary>
        /// <typeparam name="T">Type of elements of sequence.</typeparam>
        /// <param name="items">Original sequence.</param>
        /// <returns>One page of sequence with sorting as System.Collections.Generic.List<T>.</returns>
        public static List<T> Apply<T>(this ListOptions options, IEnumerable<T> items)
        {
            if (options == null)
            {
                options = ListOptions.Default();
            }

            var query = items;
            IEnumerator<SortDesc> sortsEnumerator = GetSorts(options.Sort).GetEnumerator();

            if (sortsEnumerator.MoveNext())
            {
                try
                {
                    IOrderedEnumerable<T> ordered = query.OrderBy(sortsEnumerator.Current.PropertyName, sortsEnumerator.Current.Desc);

                    while (sortsEnumerator.MoveNext())
                    {
                        ordered = ordered.ThenBy(sortsEnumerator.Current.PropertyName, sortsEnumerator.Current.Desc);
                    }

                    query = ordered;
                }
                catch (MissingMemberException)
                {
                    //info.Sort = string.Empty;
                }
            }

            if (options.GetOffset() > 0)
            {
                query = query.Skip(options.GetOffset());
            }

            if (options.Count.ToInt32() != null)
            {
                query = query.Take(options.Count.ToInt32().Value);
            }

            return query.ToList();
        }

        private static IEnumerable<SortDesc> GetSorts(string sort)
        {
            if (!sort.IsNullOrWhiteSpace())
            {
                foreach (string str in sort.Split(new[] { ZChar.Comma, ZChar.Space }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var sd = new SortDesc { PropertyName = str };

                    if (str[0] == ZChar.Plus || str[0] == ZChar.Minus)
                    {
                        sd.Desc = str[0] == ZChar.Minus;
                        sd.PropertyName = str.Substring(1);
                    }

                    yield return sd;
                }
            }
        }

        private class SortDesc
        {
            public string PropertyName { get; set; }
            public bool Desc { get; set; }
        }
    }
}
