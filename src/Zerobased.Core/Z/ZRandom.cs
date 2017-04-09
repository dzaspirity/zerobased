using System;
using System.Runtime;

namespace Zerobased
{
    /// <summary>
    /// Represent singleton System.Random object
    /// </summary>
    public static class ZRandom
    {
        private static readonly Random _rand = new Random();

        /// <summary>
        ///     Returns a nonnegative random number.
        /// </summary>
        /// <returns>
        ///     A 32-bit signed integer greater than or equal to zero and less than System.Int32.MaxValue.
        /// </returns>
        public static int Next() { return _rand.Next(); }

        /// <summary>
        ///     Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        ///     The exclusive upper bound of the random number to be generated. maxValue
        ///     must be greater than or equal to zero.
        /// </param>
        /// <returns>
        ///     A 32-bit signed integer greater than or equal to zero, and less than maxValue;
        ///     that is, the range of return values ordinarily includes zero but not maxValue.
        ///     However, if maxValue equals zero, maxValue is returned.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="maxValue"/> is less than zero.
        /// </exception>
        public static int Next(int maxValue) { return _rand.Next(maxValue); }

        /// <summary>
        ///     Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">
        ///     The inclusive lower bound of the random number returned.
        /// </param>
        /// <param name="maxValue">
        ///     The exclusive upper bound of the random number returned. maxValue must be
        ///     greater than or equal to minValue.
        /// </param>
        /// <returns>
        ///     A 32-bit signed integer greater than or equal to minValue and less than maxValue;
        ///     that is, the range of return values includes minValue but not maxValue. If
        ///     minValue equals maxValue, minValue is returned.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
        /// </exception>
        public static int Next(int minValue, int maxValue) { return _rand.Next(minValue, maxValue); }

        /// <summary>
        ///     Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">
        ///     An array of bytes to contain random numbers.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="buffer"/> is null.
        /// </exception>
        public static void NextBytes(byte[] buffer) { _rand.NextBytes(buffer); }

        /// <summary>
        ///     Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>
        ///     A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
        /// </returns>
        public static double NextDouble() { return _rand.NextDouble(); }

        /// <summary>
        ///     Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">
        ///     The exclusive upper bound of the random number to be generated. maxValue
        ///     must be greater than or equal to zero.
        /// </param>
        /// <returns>
        ///     A double greater than or equal to zero, and less than maxValue;
        ///     that is, the range of return values ordinarily includes zero but not maxValue.
        ///     However, if maxValue equals zero, maxValue is returned.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="maxValue"/> is less than zero.
        /// </exception>
        public static double NextDouble(double maxValue)
        {
            if (maxValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue must be greater than or equal to zero.");
            }
            return _rand.NextDouble() * maxValue;
        }

        /// <summary>
        ///     Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="minValue">
        ///     The inclusive lower bound of the random number returned.
        /// </param>
        /// <param name="maxValue">
        ///     The exclusive upper bound of the random number returned. maxValue must be
        ///     greater than or equal to minValue.
        /// </param>
        /// <returns>
        ///     A double greater than or equal to minValue and less than maxValue;
        ///     that is, the range of return values includes minValue but not maxValue. If
        ///     minValue equals maxValue, minValue is returned.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///     <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
        /// </exception>
        public static double NextDouble(double minValue, double maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentException("maxValue must be greater than or equal to minValue.");
            }
            return _rand.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}
