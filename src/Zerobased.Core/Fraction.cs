using System;
using Zerobased.Extensions;

namespace Zerobased
{
    /// <summary>
    ///     Static methods for parsing natural fractions
    /// </summary>
    public static class Fraction
    {
        private const char NegativeFractionPrefix = '-';
        private const char SkipableDelimeter = ' ';
        private const char Divider = '/';

        /// <summary>
        ///     Converts the string representation of a fraction to its double-precision floating-point
        ///     number equivalent.
        /// </summary>
        /// <param name="textToParse">A string that contains a fraction to convert.</param>
        /// <returns>
        ///     A double-precision floating-point number that is equivalent to the numeric value
        ///     or symbol specified in <paramref name="textToParse"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="textToParse"/> is null.</exception>
        /// <exception cref="FormatException">
        ///     <paramref name="textToParse"/> does not represent a fraction in a valid format.
        /// </exception>
        public static double Parse(string textToParse)
        {
            textToParse = Check.NotNull(textToParse, nameof(textToParse));
            if (TryParse(textToParse, out double d))
            {
                return d;
            }
            throw new FormatException($"'{textToParse}' is not a fraction.");
        }

        /// <summary>
        ///     Converts the string representation of a fraction to its double-precision floating-point
        ///     number equivalent. If the conversion failed, returns <paramref name="fallbackValue"/>
        /// </summary>
        /// <param name="textToParse">A string that contains a fraction to convert.</param>
        /// <param name="fallbackValue">Value to return in case of the failed conversion.</param>
        /// <returns></returns>
        public static double ParseSafe(string textToParse, double fallbackValue = default(double))
        {
            if (TryParse(textToParse, out double d))
            {
                return d;
            }
            return fallbackValue;
        }

        /// <summary>
        ///     Converts the string representation of a fraction to its double-precision floating-point
        ///     number equivalent. A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="textToParse">A string that contains a fraction to convert.</param>
        /// <param name="result">
        ///     When this method returns, contains the double-precision floating-point number
        ///     equivalent of the <paramref name="textToParse"/> parameter, if the conversion succeeded,
        ///     or zero if the conversion failed. The conversion fails if the <paramref name="textToParse"/>
        ///     parameter is null or <see cref="String.Empty"/>, is not a natural fraction in a valid format.
        ///     This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns>
        ///     true if <paramref name="textToParse"/> was converted successfully; otherwise, false.
        /// </returns>
        /// <remarks>
        ///     This is CSharp adaptation of the Java code from http://www.java2s.com/Code/Java/Data-Type/ParseFraction.htm
        /// </remarks>
        public static bool TryParse(string textToParse, out double result)
        {
            result = default(double);
            if (textToParse.IsNullOrEmpty())
            {
                return false;
            }
            if (double.TryParse(textToParse, out result))
            {
                return true;
            }

            int iLength;
            int iIndex;
            int iIndexStart;
            int iIndexEnd;
            int iNumber;
            int sign = 1;

            // lets use "123 456 / 789" as example or
            // lets use "123 / 789" as example

            iIndexStart = 0;
            iLength = textToParse.Length;
            if (!char.IsDigit(textToParse[iIndexStart]) && textToParse[iIndexStart] != NegativeFractionPrefix)
            {
                return false;
            }
            if (textToParse[0] == NegativeFractionPrefix)
            {
                iIndexStart = 1;
                sign = -1;
            }

            // We should be at first digit
            if (iIndexStart < iLength)
            {
                // Find end of the number
                iIndex = iIndexStart;
                while ((iIndex < iLength) && (char.IsDigit(textToParse[iIndex])))
                {
                    iIndex++;
                }
                iIndexEnd = iIndex;
                // We skipped "123", iIndexStart is at "123 456 / 789"
                // iIndexEnd is at " 456 / 789"

                if (iIndexStart != iIndexEnd)
                {
                    // There was at least some digits
                    iNumber = int.Parse(textToParse.Substring(iIndexStart, iIndexEnd - iIndexStart));
                    // iNumber is 123

                    // There was at least one digit, now is it whole part or quotient?
                    // Skip spaces
                    while (iIndex < iLength && textToParse[iIndex] == SkipableDelimeter)
                    {
                        iIndex++;
                    }
                    // We skipped "123", iIndex is at "456 / 789"

                    // Now we have stopped because of 2 things, we either reached end of
                    // string or we have found something other than space, if it is /
                    // then it was quotient, if it is digit, then it was whole part
                    if (iIndex == iLength)
                    {
                        // it was a whole part and we are done
                        result = sign * iNumber;
                        return true;
                    }
                    else
                    {
                        int iQuotient;
                        int iDivisor;

                        if (char.IsDigit(textToParse[iIndex]))
                        {
                            // it was a whole part and we continue to look for the quotient
                            int iWholePart = iNumber;

                            // Find end of the number
                            iIndexStart = iIndex; // Remember start
                            while ((iIndex < iLength) && (char.IsDigit(textToParse[iIndex])))
                            {
                                iIndex++;
                            }
                            iIndexEnd = iIndex;
                            // We skipped "456", iStartIndex is at "456 / 789"
                            // And iIndexEnd is at " / 789"

                            iQuotient = int.Parse(textToParse.Substring(iIndexStart, iIndexEnd - iIndexStart));
                            // iQuotient is 456

                            // Skip spaces
                            while (iIndex < iLength && textToParse[iIndex] == SkipableDelimeter)
                            {
                                iIndex++;
                            }
                            // And iIndex is at "/ 789"

                            if (iIndex < iLength && textToParse[iIndex] == '/')
                            {
                                // It was a quotient and we continue to look for divisor

                                iIndexStart = iIndex + 1;
                                while (iIndexStart < iLength && textToParse[iIndexStart] == SkipableDelimeter)
                                {
                                    iIndexStart++;
                                }
                                // And iIndexStart is at "789"

                                // We should be at next digit
                                if (iIndexStart < iLength)
                                {
                                    // Find end of the number
                                    iIndex = iIndexStart;
                                    while ((iIndex < iLength) && (char.IsDigit(textToParse[iIndex])))
                                    {
                                        iIndex++;
                                    }
                                    iIndexEnd = iIndex;
                                    // We skipped "789", iStartIndex is at "789"
                                    // And iIndexEnd is at the end of string

                                    if (iIndexStart != iIndexEnd)
                                    {
                                        iDivisor = int.Parse(textToParse.Substring(iIndexStart, iIndexEnd - iIndexStart));
                                        // iDivisor is 789
                                        if (iDivisor != 0)
                                        {
                                            if (iIndexEnd == iLength)
                                            {
                                                // And we are at the end of the string
                                                // Or we can ignore what is after
                                                result = sign * (iWholePart + iQuotient / ((double)iDivisor));
                                                return true;
                                            }
                                            else
                                            {
                                                // there was something else we don't know what so
                                                // return the default value
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // The divisor is missing, return default value
                                    }
                                }
                                else
                                {
                                    // The divisor is missing, return default value
                                }
                            }
                            else
                            {
                                // The divisor is missing, return default value
                            }
                        }
                        else
                        {
                            if (textToParse[iIndex] == '/')
                            {
                                // And iIndex is at "/ 456"

                                // It was a quotient and we continue to look for divisor
                                iQuotient = iNumber;
                                // iQuotient is 123

                                iIndexStart = iIndex + 1;
                                while (iIndexStart < iLength && textToParse[iIndexStart] == SkipableDelimeter)
                                {
                                    iIndexStart++;
                                }
                                // And iIndexStart is at "456"

                                // We should be at next digit
                                if (iIndexStart < iLength)
                                {
                                    // Find end of the number
                                    iIndex = iIndexStart;
                                    while ((iIndex < iLength) && (char.IsDigit(textToParse[iIndex])))
                                    {
                                        iIndex++;
                                    }
                                    iIndexEnd = iIndex;
                                    // We skipped "456", iIndexStart is at "456"
                                    // iIndexEnd is at the end of string

                                    if (iIndexStart != iIndexEnd)
                                    {
                                        iDivisor = int.Parse(textToParse.Substring(iIndexStart, iIndexEnd - iIndexStart));
                                        // iDivisor is 456

                                        if (iDivisor != 0)
                                        {
                                            if (iIndexEnd == iLength)
                                            {
                                                // And we are at the end of the string
                                                // Or we can ignore what is after
                                                result = sign * ((double)iQuotient) / iDivisor;
                                                return true;
                                            }
                                            else
                                            {
                                                // there was something else we don't know what so
                                                // return the default value
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // The divisor is missing, return default value
                                    }
                                }
                                else
                                {
                                    // The divisor is missing, return default value
                                }
                            }
                            else
                            {
                                // It was a whole part and there is something else
                                // return the default value
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
