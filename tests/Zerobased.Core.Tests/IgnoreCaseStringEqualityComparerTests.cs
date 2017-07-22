using Xunit;

namespace Zerobased.Core.Tests
{
    public class IgnoreCaseStringEqualityComparerTests
    {
        [Fact(DisplayName = "Instance should always return the save object")]
        public void GetInstance()
        {
            var first = IgnoreCaseStringEqualityComparer.Instance;
            var second = IgnoreCaseStringEqualityComparer.Instance;
            Assert.Equal(first, second);
        }

        [Fact(DisplayName = "Should return TRUE for the same string")]
        public void CompareSameStrings()
        {
            Assert.True(IgnoreCaseStringEqualityComparer.Instance.Equals("string", "string"));
        }

        [Fact(DisplayName = "Should return TRUE for strings which have same chars but in different cases")]
        public void CompareSimilarStrings()
        {
            Assert.True(IgnoreCaseStringEqualityComparer.Instance.Equals("string", "StRing"));
        }

        [Fact(DisplayName = "Should return FALSE for different strings")]
        public void CompareDifferentStrings()
        {
            Assert.False(IgnoreCaseStringEqualityComparer.Instance.Equals("string", "string1"));
        }

        [Fact(DisplayName = "Should return int.MinValue as hash code of the null")]
        public void GetHashCodeOfNull()
        {
            Assert.Equal(int.MinValue, IgnoreCaseStringEqualityComparer.Instance.GetHashCode(null));
        }

        [Fact(DisplayName = "Should return same hash code for the same string")]
        public void GetHashCodeOfSameStrings()
        {
            Assert.Equal(IgnoreCaseStringEqualityComparer.Instance.GetHashCode("string"),
                IgnoreCaseStringEqualityComparer.Instance.GetHashCode("string"));
        }

        [Fact(DisplayName = "Should return same hash code for strings which have same chars but in different cases")]
        public void GetHashCodeOfSimilarStrings()
        {
            Assert.Equal(IgnoreCaseStringEqualityComparer.Instance.GetHashCode("string"),
                IgnoreCaseStringEqualityComparer.Instance.GetHashCode("StRing"));
        }

        [Fact(DisplayName = "Should return different hash codes for different strings")]
        public void GetHashCodeOfDifferentStrings()
        {
            Assert.NotEqual(IgnoreCaseStringEqualityComparer.Instance.GetHashCode("string"),
                IgnoreCaseStringEqualityComparer.Instance.GetHashCode("string1"));
        }

    }
}
