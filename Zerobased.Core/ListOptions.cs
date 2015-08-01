using System;

namespace Zerobased
{
    /// <summary>
    ///     Describes paging and sorting for collection.
    /// </summary>
    public class ListOptions
    {
        /// <summary>
        /// Returns Zerobased.ListOptions for default page size, first page without sorting
        /// </summary>
        public static ListOptions GetDefault() { return new ListOptions { Count = CountValue.Default }; }

        /// <summary>
        /// Returns Zerobased.ListOptions for first item without sorting
        /// </summary>
        public static ListOptions GetFirst() { return new ListOptions { Count = CountValue.First }; }

        /// <summary>
        /// Returns Zerobased.ListOptions for all items without sorting
        /// </summary>
        public static ListOptions GetNoLimit() { return new ListOptions { Count = CountValue.NoLimit }; }

        /// <summary>
        /// Sorting fields.
        /// Formats:
        ///     1. Sort ascending: "propName" / "+propName"
        ///     2. Sort descending: "-propName"
        ///     3. Multiple: "prop1Name, -prop2Name"
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Index of first returned item. Optional, overrides Page property if both have values.
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Max items count to return.
        /// Key values:
        ///     1. NoLimit - returns all items (form offset)
        ///     2. Default = 10
        ///     3. First = 1
        /// If Count less then 1 - return Default = 10
        /// </summary>
        public CountValue Count { get; set; }

        /// <summary>
        /// Page number to return (1 based). Optional, overrided by Offset property if both have values.
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// Return offset calculated from Offset and Page properties.
        /// </summary>
        /// <returns>
        /// Index of first returned item.
        /// </returns>
        public int GetOffset()
        {
            if (Offset != null)
            {
                return Offset.Value;
            }
            else if (Page != null)
            {
                int? countValue = Count.ToInt32();

                if (countValue != null)
                {
                    return countValue.Value * (Page.Value - 1);
                }
            }

            return 0;
        }

        public override string ToString()
        {
            return "{0} from {1} sorted by {2}".FormatWith(Count, Offset, Sort);
        }
    }

    /// <summary>
    /// Predefined values for ListOptions.Count
    /// Allows integer values.
    /// </summary>
    public enum CountValue
    {
        NoLimit = -0xAAAA,
        First = 1,
        Default = 10
    }

    /// <summary>
    /// Extensions for enum Zerobased.CountValue
    /// </summary>
    public static class CountValueExtensions
    {
        /// <summary>
        /// Convert Zerobased.CountValue to System.Int32 value.
        /// </summary>
        /// <param name="topValue">Value to convert.</param>
        /// <returns>
        /// Returns <value>Defaul</value> if value less then <value>1</value>.
        /// Returns <value>NULL</value> if <paramref name="topValue"/> is <value>NoLimit</value>.
        /// </returns>
        public static int? ToInt32(this CountValue topValue)
        {
            int? value = null;

            if (topValue != CountValue.NoLimit)
            {
                value = (int)(topValue < CountValue.First ? CountValue.Default : topValue);
            }

            return value;
        }
    }
}
