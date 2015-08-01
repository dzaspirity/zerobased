using System;

namespace Zerobased
{
    public static class Fraction
    {
        //from http://www.java2s.com/Code/Java/Data-Type/ParseFraction.htm
        //cannot parse negative fractions
        public static double Parse(string textToParse, bool bIgnoreRest = false)
        {
            double d;

            if (TryParse(textToParse, out d, bIgnoreRest))
            {
                return d;
            }

            throw new FormatException("'" + textToParse + "' is not a fraction.");
        }

        public static double ParseSafe(string textToParse, double defaultValue = default(double), bool bIgnoreRest = false)
        {
            double d;

            if (TryParse(textToParse, out d, bIgnoreRest))
            {
                return d;
            }

            return defaultValue;
        }

        public static bool TryParse(string textToParse, out double parsed, bool bIgnoreRest = false)
        {
            if (double.TryParse(textToParse, out parsed))
            {
                return true;
            }

            parsed = default(double);
            int iLength;
            int iIndex;
            int iIndexStart;
            int iIndexEnd;
            int iNumber;
            int sign = 1;

            // lets use "xxxxxxx 123 456 / 789 yyyyy" as example or
            // lets use "xxxxxxx 123 / 789 yyyyy" as example

            iIndexStart = 0;
            iLength = textToParse.Length;
            if (bIgnoreRest)
            {
                // Skip while not number
                while ((iIndexStart < iLength) && (!char.IsDigit(textToParse[iIndexStart])))
                {
                    iIndexStart++;
                }

                if (iIndexStart > 0)
                {
                    if (textToParse[iIndexStart - 1] == '-')
                    {
                        sign = -1;
                    }
                }
                // We skiped "xxxxxxx", iIndexStart is at "123 456 / 789 yyyyy"
            }
            else if (textToParse[0] == '-')
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
                // We skiped "123", iIndexStart is at "123 456 / 789 yyyyy"
                // iIndexEnd is at " 456 / 789 yyyyy"

                if (iIndexStart != iIndexEnd)
                {
                    // There was at least some digits
                    iNumber = int.Parse(textToParse.Substring(iIndexStart, iIndexEnd - iIndexStart));
                    // iNumber is 123

                    // There was at least one digit, now is it whole part or quotient?
                    // Skip spaces
                    while (iIndex < iLength
                        && (textToParse[iIndex] == ' ' || textToParse[iIndex] == '-' || textToParse[iIndex] == '.'))
                    {
                        iIndex++;
                    }
                    // We skiped "123", iIndex is at "456 / 789 yyyyy"

                    // Now we have stopped because of 2 things, we either reached end of
                    // string or we have found something other than space, if it is /
                    // then it was qoutient, if it is digit, then it was whole part
                    if (iIndex == iLength)
                    {
                        // it was a whole part and we are done
                        parsed = sign * iNumber;
                        return true;
                    }
                    else
                    {
                        int iQuotient = 0;
                        int iDivisor = int.MaxValue;

                        if (char.IsDigit(textToParse[iIndex]))
                        {
                            int iWholePart = 0;

                            // it was a whole part and we continue to look for the quotient
                            iWholePart = iNumber;

                            // Find end of the number
                            iIndexStart = iIndex; // Remember start
                            while ((iIndex < iLength) && (char.IsDigit(textToParse[iIndex])))
                            {
                                iIndex++;
                            }
                            iIndexEnd = iIndex;
                            // We skiped "456", iStartIndex is at "456 / 789 yyyyy"
                            // And iIndexEnd is at " / 789 yyyyy"

                            iQuotient = int.Parse(textToParse.Substring(iIndexStart, iIndexEnd - iIndexStart));
                            // iQuotient is 456

                            // Skip spaces
                            while ((iIndex < iLength) && (textToParse[iIndex] == ' '))
                            {
                                iIndex++;
                            }
                            // And iIndex is at "/ 789 yyyyy"

                            if (iIndex < iLength && textToParse[iIndex] == '/')
                            {
                                // It was a quotient and we continue to look for divisor

                                iIndexStart = iIndex + 1;
                                while ((iIndexStart < iLength) && (textToParse[iIndexStart] == ' '))
                                {
                                    iIndexStart++;
                                }
                                // And iIndexStart is at "789 yyyyy"

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
                                    // We skiped "789", iStartIndex is at "789 yyyyy"
                                    // And iIndexEnd is at " yyyyy"

                                    if (iIndexStart != iIndexEnd)
                                    {
                                        iDivisor = int.Parse(textToParse.Substring(iIndexStart, iIndexEnd - iIndexStart));
                                        // iDivisor is 789
                                        if (iDivisor != 0)
                                        {
                                            if (iIndexEnd == iLength || bIgnoreRest)
                                            {
                                                // And we are at the end of the string
                                                // Or we can ignore what is after
                                                parsed = sign * (((double)(iWholePart)) + (((double)iQuotient) / ((double)iDivisor)));
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
                                // And iIndex is at "/ 456 yyyyy"

                                // It was a quotient and we continue to look for divisor
                                iQuotient = iNumber;
                                // iQuotient is 123

                                iIndexStart = iIndex + 1;
                                while ((iIndexStart < iLength) && (textToParse[iIndexStart] == ' '))
                                {
                                    iIndexStart++;
                                }
                                // And iIndexStart is at "456 yyyyy"

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
                                    // We skipped "456", iIndexStart is at "456 yyyyy"
                                    // iIndexEnd is at " yyyyy"

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
                                                parsed = sign * ((double)iQuotient) / ((double)iDivisor);
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
                                if (bIgnoreRest)
                                {
                                    // and we are done
                                    parsed = sign * iNumber;
                                    return true;
                                }
                                else
                                {
                                    // there was something else we don't know what so
                                    // return the default value
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
