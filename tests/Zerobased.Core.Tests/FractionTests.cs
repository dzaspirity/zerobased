using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Zerobased.Core.Tests
{
    public class FractionTests
    {
        [Fact(DisplayName = "Should parse integer '123'")]
        public void ParseInteger()
        {
            Fraction.TryParse("123", out double actual);
            Assert.Equal(123d, actual);
        }

        [Fact(DisplayName = "Should parse fraction without integer part '2/5'")]
        public void ParseFractionWithoutIntegerPart()
        {
            Fraction.TryParse("2/5", out double actual);
            Assert.Equal(2d / 5, actual);
        }

        [Fact(DisplayName = "Should parse negative fraction without integer part '-2/5'")]
        public void ParseNegativeFractionWithoutIntegerPart()
        {
            Fraction.TryParse("-2/5", out double actual);
            Assert.Equal(-2d / 5, actual);
        }

        [Fact(DisplayName = "Should parse fraction without integer part with spaces '2 / 5'")]
        public void ParseFractionWithoutIntegerPartWithSpaces()
        {
            Fraction.TryParse("2 / 5", out double actual);
            Assert.Equal(2d / 5, actual);
        }

        [Fact(DisplayName = "Should parse fraction with integer part '12 2/5'")]
        public void ParseFractionWithIntegerPart()
        {
            Fraction.TryParse("12 2/5", out double actual);
            Assert.Equal(12 + 2d / 5, actual);
        }

        [Fact(DisplayName = "Should parse negative fraction with integer part '-12 2/5'")]
        public void ParseNegativeFractionWithIntegerPart()
        {
            Fraction.TryParse("-12 2/5", out double actual);
            Assert.Equal(-12 - 2d / 5, actual);
        }

        [Fact(DisplayName = "Should parse fraction with integer part with spaces '12   2  /  5'")]
        public void TryParseFractionWithIntegerPartWithSpaces()
        {
            Fraction.TryParse("12   2  /  5", out double actual);
            Assert.Equal(12 + 2d / 5, actual);
        }

        [Fact(DisplayName = "Should throw ArgumentNullException on parsing NULL")]
        public void ParseNull()
        {
            ArgumentNullException exc = Assert.Throws<ArgumentNullException>(() => Fraction.Parse(null));
            Assert.Equal("textToParse", exc.ParamName);
        }

        [Fact(DisplayName = "Should throw FormatException on parsing empty sting")]
        public void ParseEmptyString()
        {
            Assert.Throws<FormatException>(() => Fraction.Parse(string.Empty));
        }

        [Fact(DisplayName = "Should throw FormatException on parsing invalid fraction 'q'")]
        public void ParseInvalid()
        {
            Assert.Throws<FormatException>(() => Fraction.Parse("q"));
        }
    }
}
